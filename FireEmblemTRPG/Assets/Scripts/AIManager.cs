using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager instance;
    
    private List<EnemyAI> aiList = new List<EnemyAI>();
    [HideInInspector] public int aiPlayingOrder = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (var item in TurnManager.instance.nonPlayableCharacterList) //TODO - Maybe Update this later to fit the death list enemy
        {
            aiList.Add(item.gameObject.GetComponent<EnemyAI>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnManager.instance.actualTurnState == TurnManager.TurnStates.PlayerTurn)
            return;

        if (!aiList[aiPlayingOrder].isMoving && aiList[aiPlayingOrder].GetComponent<BaseArchetype>().hasMovementLeft)
        {
            Debug.Log("START MOVE");
            aiList[aiPlayingOrder].Movement(); //TODO limit this to ONE per AI
        }

        Debug.Log("IS MOVING");

        if (aiList[aiPlayingOrder].GetComponent<BaseArchetype>().hasActionLeft)
            return;

        aiPlayingOrder++;
        aiPlayingOrder = Mathf.Clamp(aiPlayingOrder, 0, aiList.Count - 1);
    }
}
