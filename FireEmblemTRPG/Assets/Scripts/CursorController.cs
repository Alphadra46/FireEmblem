using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [SerializeField] private bool activeDiagonals;
    public int characterMovementSpeed;
    [SerializeField] private LayerMask overlayTileLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask allyLayer;
    
    private Vector2 moveDirection;

    //private float customDeltaTime = 0.25f;
    //private float deltaTimeTracker;

    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();
    private List<OverlayTile> inRangeAttackTiles = new List<OverlayTile>();
    private RangeFinder rangeFinder;
    [HideInInspector] public List<OverlayTile> inRangeAttackPhaseTiles = new List<OverlayTile>();
    private List<BaseArchetype> enemyInAttackRange = new List<BaseArchetype>();

    private List<BaseArchetype> allyInHealRange = new List<BaseArchetype>();
    private List<OverlayTile> inRangeHealSkillTiles = new List<OverlayTile>();

    private List<OverlayTile> path = new List<OverlayTile>();
    private AstarPathfinder astarPathFinder;

    private ArrowTranslator arrowTranslator;
    
    private BaseArchetype selectedCharacterAchetype;
    [HideInInspector] public BaseArchetype selectedCharacterForAction;

    private BaseArchetype selectedEnemyForAttack;

    private BaseArchetype selectedAllyForHeal;

    private bool isMoving = false;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isUsingSkill = false;
    // Start is called before the first frame update
    void Start()
    {
        InputManagerScript.instance.move.started += MoveCursor;
        InputManagerScript.instance.action.started += SelectCharacter;
        //InputManagerScript.instance.cancelAction.started += DeselectCharacter;
        rangeFinder = new RangeFinder();
        astarPathFinder = new AstarPathfinder();
        arrowTranslator = new ArrowTranslator();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (inRangeTiles.Contains(CurrentHoveredTile()) && !isMoving && selectedCharacterForAction != null && !isAttacking && selectedCharacterForAction.hasActionLeft && !isUsingSkill)
        {
            path = astarPathFinder.FindPath(CharacterCurrentStandingTile(),CurrentHoveredTile(), inRangeTiles);

            foreach (var item in inRangeTiles)
            {
                item.SetArrowSprite(ArrowTranslator.ArrowDirection.None);
            }

            for (int i = 0; i < path.Count; i++)
            {
                var previousTile = i > 0 ? path[i - 1] : CharacterCurrentStandingTile();
                var futureTile = i < path.Count - 1 ? path[i + 1] : null;

                var arrowDir = arrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                path[i].SetArrowSprite(arrowDir);
            }

            if (selectedCharacterForAction != null)
            {
                //Debug.Log(selectedCharacterForAction.gameObject.name);
            }
            
        }
        
        
        if (path.Count > 0 && isMoving)
        {
            MoveAlongPath();
        }

        if (path.Count <= 0 && isMoving)
        {
            isMoving = false;
            //selectedCharacterForAction = null;
            GetInRangeAttackTiles(CharacterCurrentStandingTile());
            ContextMenu.instance.ToggleContextUI();
            ContextMenu.instance.uiAttackValues.selectedCharacter = selectedCharacterForAction;
            ContextMenu.instance.uiSkillValues.selectedCharacter = selectedCharacterForAction;
        }
    }

    
    private void MoveCursor(InputAction.CallbackContext context)
    {
        moveDirection = InputManagerScript.instance.move.ReadValue<Vector2>();

        
        //Option dans l'éditeur pour activer ou désactiver la gestion des diagonales
        if (!activeDiagonals) 
        {
            //Gestion des diagonales, Commenter si les diagonales sont voulues
            if (moveDirection.x != 0 && moveDirection.y !=0) 
                return;
        }

        transform.position += new Vector3(moveDirection.x, 0, moveDirection.y); //TODO - Changer par un déplacement via vélocité pour la gestion des collisions si nécessaire
    }

    /// <summary>
    /// Get the list of tiles in the movement range of the character (white ones) and also display the maximum attack range (blue tiles)
    /// </summary>
    /// <param name="characterStandingTile"></param>
    private void GetInRangeTiles(OverlayTile characterStandingTile)
    {
        foreach (var item in inRangeTiles)
        {
            item.HideTile();
        }

        foreach (var item in inRangeAttackTiles)
        {
            item.HideTile();
        }
        
        inRangeTiles = rangeFinder.GetTilesInRange(characterStandingTile, selectedCharacterAchetype.movement);
        inRangeAttackTiles = rangeFinder.GetTilesInRange(characterStandingTile, selectedCharacterAchetype.movement + selectedCharacterAchetype.equippedWeapon.rangeMax);

        var tempTileList = new List<OverlayTile>();
        foreach (var attackTile in inRangeAttackTiles)
        {
            foreach (var movementTile in inRangeTiles)
            {
                if (movementTile == attackTile)
                {
                    tempTileList.Add(attackTile);
                }
            }
        }

        foreach (var item in tempTileList)
        {
            inRangeAttackTiles.Remove(item);
        }

        tempTileList.Clear();
        
        foreach (var item in inRangeAttackTiles)
        {
            item.ShowTile();
            item.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        
        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
            item.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    
    /// <summary>
    /// Get the list of tiles where the character can Attack an enemy
    /// </summary>
    /// <param name="characterStandingTile"></param>
    private void GetInRangeAttackTiles(OverlayTile characterStandingTile)
    {
        bool enemyInRange = false;
        
        foreach (var item in inRangeAttackPhaseTiles)
        {
            item.HideTile();
        }

        var rangeMaxWeapon = selectedCharacterForAction.equippedWeapon.rangeMax;
        var rangeMinWeapon = selectedCharacterForAction.equippedWeapon.rangeMin;
        
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
            Physics.OverlapBoxNonAlloc(item.transform.position,new Vector3(0.5f,0.5f,0.5f),enemy,new Quaternion(0,0,0,0), enemyLayer);
            if (enemy[0] != null)
            { 
                enemyInRange = true;
                enemyInAttackRange.Add(enemy[0].GetComponent<BaseArchetype>());
            }
        }



        if (enemyInRange)
        {
            ContextMenu.instance.contextUI.transform.GetChild(0).gameObject.SetActive(true);
            ContextMenu.instance.contextUI.transform.GetChild(1).gameObject.SetActive(true);
            foreach (var item in inRangeAttackPhaseTiles)
            {
                item.ShowTile();
                item.GetComponent<SpriteRenderer>().color = Color.blue;
            }
            //isAttacking = true; //May cause some issues in the future TODO - CHANGE THIS <-----------------------------------------------------------------------------------------
        }
        else
        {
            enemyInAttackRange.Clear();
            isAttacking = false;
            
            ContextMenu.instance.contextUI.transform.GetChild(0).gameObject.SetActive(false);
            ContextMenu.instance.contextUI.transform.GetChild(1).gameObject.SetActive(true);
            if (selectedCharacterForAction!=null)
            {
                if (selectedCharacterForAction.equippedSkillList[0].targetLayer == "Enemy")
                {
                    ContextMenu.instance.contextUI.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
        
        //selectedCharacterForAction = null;//TODO - Change this 
        
    }

    public void GetInRangeHealTiles(OverlayTile characterStandingTile)
    {
        bool allyInRange = false;
        
        foreach (var item in inRangeAttackPhaseTiles)
        {
            item.HideTile();
        }

        var rangeMaxWeapon = selectedCharacterForAction.equippedWeapon.rangeMax;
        var rangeMinWeapon = selectedCharacterForAction.equippedWeapon.rangeMin;
        
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
            Collider[] ally = new Collider[1];//Maybe change to something bigger than 1
            Physics.OverlapBoxNonAlloc(item.transform.position,new Vector3(0.5f,0.5f,0.5f),ally,new Quaternion(0,0,0,0), allyLayer);
            if (ally[0] != null)
            { 
                allyInRange = true;
                allyInHealRange.Add(ally[0].GetComponent<BaseArchetype>());
            }
        }



        if (allyInRange)
        {
            ContextMenu.instance.contextUI.transform.GetChild(0).gameObject.SetActive(true);
            ContextMenu.instance.contextUI.transform.GetChild(1).gameObject.SetActive(true);
            isUsingSkill = true;
            // foreach (var item in inRangeAttackPhaseTiles)
            // {
            //     item.ShowTile();
            //     item.GetComponent<SpriteRenderer>().color = Color.blue;
            // }
            //isAttacking = true; //May cause some issues in the future TODO - CHANGE THIS <-----------------------------------------------------------------------------------------
        }
        else
        {
            allyInHealRange.Clear();
            isUsingSkill = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NonPlayableCharacter") && isAttacking)
        {
            selectedEnemyForAttack = other.GetComponent<BaseArchetype>();
        }
        
        if (isAttacking)
            return;
        
        if ((selectedCharacterForAction == null || selectedCharacterForAction.gameObject == other.gameObject) && other.GetComponent<BaseArchetype>().hasMovementLeft)
        {
            selectedCharacterAchetype = other.GetComponent<BaseArchetype>();
            GetInRangeTiles(CharacterCurrentStandingTile());
        }

        if (isUsingSkill && selectedAllyForHeal == null)
        {
            selectedAllyForHeal = other.GetComponent<BaseArchetype>();
        }

    }

    public OverlayTile CharacterCurrentStandingTile()
    {
        if (selectedCharacterAchetype != null)
        {
            Physics.Raycast(new Ray(new Vector3(selectedCharacterAchetype.transform.position.x,selectedCharacterAchetype.transform.position.y+0.5f,selectedCharacterAchetype.transform.position.z),Vector3.down), out RaycastHit overlayTile, 1.5f, overlayTileLayer);
            return overlayTile.collider.GetComponent<OverlayTile>();
        }
        else if (selectedCharacterForAction != null)
        {
            Physics.Raycast(new Ray(new Vector3(selectedCharacterForAction.transform.position.x,selectedCharacterForAction.transform.position.y+0.5f,selectedCharacterForAction.transform.position.z),Vector3.down), out RaycastHit overlayTile2, 1.5f, overlayTileLayer);
            return overlayTile2.collider.GetComponent<OverlayTile>();
        }

        return null;
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
    
    private void OnTriggerExit(Collider other)
    {
        if (selectedCharacterForAction == null && selectedCharacterAchetype != null)
        {
            foreach (var item in inRangeTiles) 
            {
                item.HideTile();
            }
            foreach (var item in inRangeAttackTiles) 
            {
                item.HideTile();
            }
        }
        selectedCharacterAchetype = null; //Can be a problem for some situations

        if (other.CompareTag("NonPlayableCharacter") && isAttacking)
        {
            selectedEnemyForAttack = null;
        }
        
        if (isUsingSkill && selectedAllyForHeal != null)
        {
            selectedAllyForHeal = null;
        }
    }


    private void SelectCharacter(InputAction.CallbackContext context)
    {
        if (selectedCharacterAchetype == null && selectedCharacterForAction == null)
            return;
        
        
        if (selectedCharacterAchetype != null && selectedCharacterForAction == null && selectedCharacterAchetype.CompareTag("PlayableCharacter"))
        {
            selectedCharacterForAction = selectedCharacterAchetype;
            Debug.Log("Character Selected");
        }
        else if (selectedCharacterAchetype == null && selectedCharacterForAction != null && selectedCharacterForAction.hasMovementLeft)
        {
            selectedCharacterForAction.hasMovementLeft = false;
            
            foreach (var item in inRangeTiles)
            {
                item.HideTile();
                item.SetArrowSprite(ArrowTranslator.ArrowDirection.None);
            }
            
            foreach (var item in inRangeAttackTiles)
            {
                item.HideTile();
            }

            isMoving = true;
        }

        if (isAttacking && enemyInAttackRange.Contains(selectedEnemyForAttack) && !isUsingSkill)
        {
            selectedCharacterForAction.canCounter = false;
            CombatManager.instance.StartAttack(selectedCharacterForAction,selectedEnemyForAttack, 1);
            selectedCharacterForAction.hasActionLeft = false;
            selectedCharacterForAction = null;
            selectedEnemyForAttack = null;
            isAttacking = false;
            foreach (var item in inRangeAttackPhaseTiles)
            {
                item.HideTile();
            }
        }

        if (isUsingSkill && selectedCharacterForAction.equippedSkillList[0].skillName == "Fromagie" && selectedAllyForHeal != null)
        {
            Heal healSkill = (Heal)selectedCharacterForAction.equippedSkillList[0];
            healSkill.Effect(selectedAllyForHeal);
            selectedCharacterForAction.hasActionLeft = false;
            isUsingSkill = false;
            selectedCharacterForAction = null;
            selectedAllyForHeal = null;
            foreach (var item in inRangeAttackPhaseTiles)
            {
                item.HideTile();
            }
        }

        if (isUsingSkill && selectedCharacterForAction.equippedSkillList[0].skillName == "Rascaille" && selectedEnemyForAttack != null)
        {
            StunAttack stunSkill = (StunAttack)selectedCharacterForAction.equippedSkillList[0];
            stunSkill.Effect(selectedEnemyForAttack);
            isUsingSkill = false;
            selectedCharacterForAction.hasActionLeft = false;
            selectedCharacterForAction = null;
            selectedEnemyForAttack = null;
            isAttacking = false;
            foreach (var item in inRangeAttackPhaseTiles)
            {
                item.HideTile();
            }
        }

        if (isUsingSkill && selectedCharacterForAction.equippedSkillList[0].skillName == "Bugne" && selectedEnemyForAttack != null)
        {
            selectedCharacterForAction.canCounter = false;
            LongRangeAttack longRangeAttackSkill = (LongRangeAttack)selectedCharacterForAction.equippedSkillList[0];
            longRangeAttackSkill.Effect(selectedCharacterForAction, selectedEnemyForAttack);
            isUsingSkill = false;
            isAttacking = false;
            selectedCharacterForAction.hasActionLeft = false;
            selectedCharacterForAction = null;
            foreach (var item in inRangeAttackPhaseTiles)
            {
                item.HideTile();
            }
        }

        if (isUsingSkill && selectedCharacterForAction.equippedSkillList[0].skillName == "TourneDos" && selectedCharacterForAction != null)
        {
            isUsingSkill = false;
            selectedCharacterForAction.hasActionLeft = false;
            selectedCharacterForAction = null;
            foreach (var item in inRangeAttackPhaseTiles)
            {
                item.HideTile();
            }
        }
    }

    private void MoveAlongPath()
    {
        var step = characterMovementSpeed * Time.deltaTime;

        selectedCharacterForAction.transform.position = new Vector3(Mathf.MoveTowards(selectedCharacterForAction.transform.position.x, path[0].transform.position.x,step),selectedCharacterForAction.transform.position.y, Mathf.MoveTowards(selectedCharacterForAction.transform.position.z, path[0].transform.position.z,step));

        if (Vector2.Distance(new Vector2(selectedCharacterForAction.transform.position.x,selectedCharacterForAction.transform.position.z),new Vector2(path[0].transform.position.x, path[0].transform.position.z)) < 0.001f)
        {
            selectedCharacterForAction.transform.position = new Vector3(path[0].transform.position.x, selectedCharacterForAction.transform.position.y, path[0].transform.position.z);
            path.RemoveAt(0);
        }
        
    }

    private void DeselectCharacter(InputAction.CallbackContext context) //TODO - Need to not be able to deselect a Character while in attack phase (If necessary) (If character is deselected after the attack)
    {
        if (selectedCharacterForAction == null) 
            return;

        selectedCharacterForAction = null;
        foreach (var item in MapManager.instance.map.Values) // Issue : it hides the hovered character range if deselect while we're still on the character. Can be solved by a OnTriggerStay that call the GetInRangeTiles every frames.
        {
            item.HideTile(); //TODO - Maybe change this to Hide only the range and attack range tiles
        }
    }


    private OverlayTile CurrentHoveredTile()
    {
        Physics.Raycast(new Ray(new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z),Vector3.down), out RaycastHit overlayTile, 1.5f, overlayTileLayer);
        if (overlayTile.collider != null)
        {
            return overlayTile.collider.GetComponent<OverlayTile>();
        }
        else
        {
            return null;
        }
    }
}
