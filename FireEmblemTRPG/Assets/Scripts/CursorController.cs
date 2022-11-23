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
    
    private Vector2 moveDirection;

    private float customDeltaTime = 0.25f;
    private float deltaTimeTracker;

    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();
    private List<OverlayTile> inRangeAttackTiles = new List<OverlayTile>();
    private RangeFinder rangeFinder;

    private List<OverlayTile> path = new List<OverlayTile>();
    private AstarPathfinder astarPathFinder;

    private ArrowTranslator arrowTranslator;
    
    private BaseArchetype selectedCharacterAchetype;
    private BaseArchetype selectedCharacterForAction;

    private bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        InputManagerScript.instance.move.started += MoveCursor;
        InputManagerScript.instance.action.started += SelectCharacter;
        InputManagerScript.instance.cancelAction.started += DeselectCharacter;
        rangeFinder = new RangeFinder();
        astarPathFinder = new AstarPathfinder();
        arrowTranslator = new ArrowTranslator();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (inRangeTiles.Contains(CurrentHoveredTile()) && !isMoving)
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
            
        }
        
        
        if (path.Count > 0 && isMoving)
        {
            MoveAlongPath();
        }

        if (path.Count <= 0 && isMoving)
        {
            isMoving = false;
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayableCharacter") && (selectedCharacterForAction == null || selectedCharacterForAction.gameObject == other.gameObject))
        {
            selectedCharacterAchetype = other.GetComponent<BaseArchetype>();
            GetInRangeTiles(CharacterCurrentStandingTile());
        }
    }

    private OverlayTile CharacterCurrentStandingTile()
    {
        if (selectedCharacterAchetype != null)
        {
            Physics.Raycast(new Ray(new Vector3(selectedCharacterAchetype.transform.position.x,selectedCharacterAchetype.transform.position.y+0.5f,selectedCharacterAchetype.transform.position.z),Vector3.down), out RaycastHit overlayTile, 1.5f, overlayTileLayer);
            return overlayTile.collider.GetComponent<OverlayTile>();
        }
        else
        {
            Physics.Raycast(new Ray(new Vector3(selectedCharacterForAction.transform.position.x,selectedCharacterForAction.transform.position.y+0.5f,selectedCharacterForAction.transform.position.z),Vector3.down), out RaycastHit overlayTile2, 1.5f, overlayTileLayer);
            return overlayTile2.collider.GetComponent<OverlayTile>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayableCharacter") && selectedCharacterForAction == null)
        {
            foreach (var item in MapManager.instance.map.Values)
            {
                item.HideTile();
            }
        }
        selectedCharacterAchetype = null; //Can be a problem for some situations

    }


    private void SelectCharacter(InputAction.CallbackContext context)
    {
        if (selectedCharacterAchetype == null && selectedCharacterForAction == null)
            return;
        
        
        if (selectedCharacterAchetype != null && selectedCharacterForAction == null)
        {
            selectedCharacterForAction = selectedCharacterAchetype;
            Debug.Log("Character Selected");
        }
        else if (selectedCharacterAchetype == null && selectedCharacterForAction != null)
        {
            //path = astarPathFinder.FindPath(CharacterCurrentStandingTile(),CurrentHoveredTile(), inRangeTiles);
            
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

    private void DeselectCharacter(InputAction.CallbackContext context)
    {
        if (selectedCharacterForAction == null) 
            return;

        selectedCharacterForAction = null;
        foreach (var item in MapManager.instance.map.Values) // Problem : it hides the hovered character range if deselect while we're still on the character. Can be solved by a OnTriggerStay that call the GetInRangeTiles every frames.
        {
            item.HideTile();
        }
    }


    private OverlayTile CurrentHoveredTile()
    {
        Physics.Raycast(new Ray(new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z),Vector3.down), out RaycastHit overlayTile, 1.5f, overlayTileLayer);
        return overlayTile.collider.GetComponent<OverlayTile>();
    }
}
