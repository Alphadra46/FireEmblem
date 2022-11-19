using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OverlayTile : MonoBehaviour
{

    public int G;
    public int H;
    
    public int F
    {
        get { return G + H; }
    }

    public bool isBlocked;
    
    public OverlayTile previous;
    public Vector3Int gridLocation;

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    
    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
