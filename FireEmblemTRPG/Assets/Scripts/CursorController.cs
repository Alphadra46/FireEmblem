using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [SerializeField] private bool activeDiagonals;
    
    private Vector2 moveDirection;

    private float customDeltaTime = 0.25f;
    private float deltaTimeTracker;

    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();
    private RangeFinder rangeFinder;

    private List<OverlayTile> path = new List<OverlayTile>();
    private AstarPathfinder astarPathFinder;

    private BaseArchetype selectedCharacterAchetype;
    private BaseArchetype selectedCharacterForAction;
    // Start is called before the first frame update
    void Start()
    {
        InputManagerScript.instance.move.started += MoveCursor;
        InputManagerScript.instance.action.started += SelectCharacter;
        InputManagerScript.instance.cancelAction.started += DeselectCharacter;
        rangeFinder = new RangeFinder();
        astarPathFinder = new AstarPathfinder();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        
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

    private void GetInRangeTiles(OverlayTile characterStandingTile)
    {
        foreach (var item in inRangeTiles)
        {
            item.HideTile();
        }
        
        inRangeTiles = rangeFinder.GetTilesInRange(characterStandingTile, selectedCharacterAchetype.movement);

        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayableCharacter"))
        {
            selectedCharacterAchetype = other.GetComponent<BaseArchetype>();
            GetInRangeTiles(MapManager.instance.map[new Vector2Int(Mathf.RoundToInt(other.transform.position.x),Mathf.RoundToInt(other.transform.position.z))]);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayableCharacter") && selectedCharacterForAction != other.GetComponent<BaseArchetype>())
        {
            foreach (var item in MapManager.instance.map.Values)
            {
                item.HideTile();
            }
        }
    }


    private void SelectCharacter(InputAction.CallbackContext context)
    {
        if (selectedCharacterAchetype == null)
            return;
        
        selectedCharacterForAction = selectedCharacterAchetype;
    }

    private void DeselectCharacter(InputAction.CallbackContext context)
    {
        if (selectedCharacterAchetype == null)
            return;

        selectedCharacterForAction = null;
        foreach (var item in MapManager.instance.map.Values) // Problem : it hides the hovered character range if deselect while we're still on the character. Can be solved by a OnTriggerStay that call the GetInRangeTiles every frames.
        {
            item.HideTile();
        }
    }
}
