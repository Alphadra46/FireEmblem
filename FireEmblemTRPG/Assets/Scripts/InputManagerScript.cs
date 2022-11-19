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
    [HideInInspector] public InputAction cancelAction;

    private void Awake()
    {
        instance = this;
        playerInputAction = new PlayerInputAction();
        InitControls();
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

    private void InitControls()
    {
        move = playerInputAction.Player.Move;
        action = playerInputAction.Player.Action;
        cancelAction = playerInputAction.Player.CancelAction;
    }
    
    public void OnEnablePlayerControls()
    {
        move.Enable();
        action.Enable();
        cancelAction.Enable();
    }

    public void OnDisablePlayerControls()
    {
        move.Disable();
        action.Disable();
        cancelAction.Disable();
    }
    
}
