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
    // Start is called before the first frame update
    void Start()
    {
        InputManagerScript.instance.move.started += MoveCursor;
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

    
    
}
