using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private LayerMask overlayTileLayer;
    [SerializeField] private LayerMask allyLayer;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private int characterMovementSpeed;

    private bool thisCharacterTurn;
    [HideInInspector] public bool isMoving = false;
    private bool isAttacking = false;
    
    private AstarPathfinder pathFinder;
    private RangeFinder rangeFinder;
    [SerializeField] private BaseArchetype playerToFocus;

    private List<OverlayTile> tilesRange = new List<OverlayTile>();
    private List<OverlayTile> pathInRange = new List<OverlayTile>();
    private List<OverlayTile> pathfindingTiles = new List<OverlayTile>();

    private List<BaseArchetype> enemyInAttackRange = new List<BaseArchetype>();
    private List<OverlayTile> inRangeAttackPhaseTiles = new List<OverlayTile>();

    private BaseArchetype thisCharacter;

    // Start is called before the first frame update
    void Start()
    {
        pathFinder = new AstarPathfinder();
        rangeFinder = new RangeFinder();
        playerToFocus = TurnManager.instance.playableCharacterList[Random.Range(0, TurnManager.instance.playableCharacterList.Count-1)];
        thisCharacter = GetComponent<BaseArchetype>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pathInRange == null)
            return;
        
        
        if (isMoving && pathInRange.Count > 0)
        {
            MoveAlongPath();
        }

        if (pathInRange.Count <= 0 && isMoving)
        {
            Debug.Log("END MOVE");
            GetComponent<BaseArchetype>().hasMovementLeft = false;
            isMoving = false;
            CheckInRangeAttackTiles(CharacterCurrentStandingTile(thisCharacter));
        }
        
    }
    
    public void Movement()
    {
        tilesRange.Clear();
        pathInRange.Clear();
        tilesRange = rangeFinder.GetTilesInRange(CharacterCurrentStandingTile(thisCharacter), thisCharacter.movement);
        pathfindingTiles = pathFinder.FindPath(CharacterCurrentStandingTile(thisCharacter), CharacterCurrentStandingTile(playerToFocus), new List<OverlayTile>());
        
        foreach (OverlayTile tilesInGlobalPath in pathfindingTiles.ToArray())
        {
            foreach (OverlayTile tilesInRange in tilesRange.ToArray())
            {
                if (tilesInRange == tilesInGlobalPath)
                {
                    pathInRange.Add(tilesInRange);
                }
            }
        }

        var tempIndex = 100;
        var tempList = new List<OverlayTile>();
        
        if (pathInRange.Count > 0)
        {
            for (int i = pathInRange.Count-1; i >= 0; i--) //Bug Enemy stacking with player and other enemies
            {
                if(IsCharacterOnThisTile(pathInRange[i]))
                {
                    tempIndex = i;
                }
            }

            for (int i = tempIndex; i < pathInRange.Count; i++)
            {
                tempList.Add(pathInRange[i]);
            }

            foreach (var item in tempList)
            {
                pathInRange.Remove(item);
            }
        }
        
        // if(IsCharacterOnThisTile(pathInRange[pathInRange.Count - 1])) 
        // {
        //     pathInRange.Remove(pathInRange[pathInRange.Count - 1]);
        // }
        tempList.Clear();
        pathfindingTiles.Clear();
        isMoving = true;
        
    }
    
    
    public OverlayTile CharacterCurrentStandingTile(BaseArchetype character)
    {
        if (character != null)
        {
            Physics.Raycast(new Ray(new Vector3(character.transform.position.x, character.transform.position.y + 0.5f, character.transform.position.z), Vector3.down), out RaycastHit overlayTile, 1.5f, overlayTileLayer);
            return overlayTile.collider.GetComponent<OverlayTile>();
        }

        return null;
    }


    public bool IsCharacterOnThisTile(OverlayTile tile)
    {
        Physics.Raycast(new Ray(tile.transform.position, Vector3.up), out RaycastHit character, 1.5f,allyLayer);
        Physics.Raycast(new Ray(tile.transform.position, Vector3.up), out RaycastHit enemyCharacter, 1.5f,enemyLayer);
        if (character.collider != null || enemyCharacter.collider != null)
        {
            Debug.Log("SOMEONE HERE");
            return true;
        }

        return false;
    }
    
    private void MoveAlongPath()
    {
        var step = characterMovementSpeed * Time.deltaTime;

        transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, pathInRange[0].transform.position.x,step),transform.position.y, Mathf.MoveTowards(transform.position.z, pathInRange[0].transform.position.z,step));

        if (Vector2.Distance(new Vector2(transform.position.x,transform.position.z),new Vector2(pathInRange[0].transform.position.x, pathInRange[0].transform.position.z)) < 0.001f)
        {
            transform.position = new Vector3(pathInRange[0].transform.position.x, transform.position.y, pathInRange[0].transform.position.z);
            pathInRange.RemoveAt(0);
        }
        
    }

    public void CheckInRangeAttackTiles(OverlayTile characterStandingTile)
    {

        var rangeMaxWeapon = thisCharacter.equippedWeapon.rangeMax;
        var rangeMinWeapon = thisCharacter.equippedWeapon.rangeMin;
        
        inRangeAttackPhaseTiles = rangeFinder.GetTilesInRange(characterStandingTile,rangeMaxWeapon);

        if (rangeMinWeapon == rangeMaxWeapon)
        {
            var temp = rangeFinder.GetTilesInRange(characterStandingTile, rangeMinWeapon-1);

            foreach (var item in temp)
            {
                inRangeAttackPhaseTiles.Remove(item);
            }

        }
        foreach (var item in inRangeAttackPhaseTiles)
        {
            Collider[] enemy = new Collider[1];//Maybe change to something bigger than 1
            Physics.OverlapBoxNonAlloc(item.transform.position,new Vector3(0.5f,0.5f,0.5f),enemy,new Quaternion(0,0,0,0), allyLayer);
            if (enemy[0] != null)
            { 
                enemyInAttackRange.Add(enemy[0].GetComponent<BaseArchetype>());
            }
        }

        bool playerToFocusIsInRange = false;

        foreach (var item in enemyInAttackRange)
        {
            if (item == playerToFocus)
            {
                playerToFocusIsInRange = true;
                Debug.Log("Target in Range");
                break;
            }
        }

        if (playerToFocusIsInRange)
        {
            playerToFocus.TakeDamage(thisCharacter,1f);
        }
        
        enemyInAttackRange.Clear();
        isAttacking = false;
        thisCharacter.hasActionLeft = false;
        Debug.Log("Target NOT in Range");
    }
    
    
    
}
