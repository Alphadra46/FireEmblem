using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerScript : MonoBehaviour
{

    public static InputManagerScript instance;

    public PlayerInputAction playerInputAction;
    [HideInInspector] public InputAction move;
    [HideInInspector] public InputAction action;

    private void Awake()
    {
        instance = this;
        playerInputAction = new PlayerInputAction();
        OnEnablePlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnablePlayerControls()
    {
        move = playerInputAction.Player.Move;
        action = playerInputAction.Player.Action;
        
        move.Enable();
        action.Enable();
    }

    public void OnDisablePlayerControls()
    {
        move.Disable();
        action.Disable();
    }
    
}
