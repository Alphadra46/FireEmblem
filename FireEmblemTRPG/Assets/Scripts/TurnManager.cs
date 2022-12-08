using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<BaseArchetype> playableCharacterList = new List<BaseArchetype>();
    private List<BaseArchetype> nonPlayableCharacterList = new List<BaseArchetype>();

    [SerializeField] private TurnStates actualTurnState;
    public enum TurnStates
    {
        PlayerTurn,
        EnemyTurn
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeLists();
        actualTurnState = TurnStates.PlayerTurn;
    }

    // Update is called once per frame
    void Update()
    {
        switch (actualTurnState)
        {
            case TurnStates.PlayerTurn:
                foreach (var item in playableCharacterList)
                {
                    if (item.hasActionLeft)
                        return;
                }

                ChangeTurn(TurnStates.EnemyTurn);
                break;
            case TurnStates.EnemyTurn:
                foreach (var item in nonPlayableCharacterList)
                {
                    if (item.hasActionLeft)
                        return;
                }

                ChangeTurn(TurnStates.PlayerTurn);
                break;
            default:
                break;
        }
        
        
    }

    private void ChangeTurn(TurnStates nextTurn)
    {
        EndTurn(actualTurnState);
        StartTurn(nextTurn);
        actualTurnState = nextTurn;
    }

    private void StartTurn(TurnStates turnToStart)
    {
        switch (turnToStart)
        {
            case TurnStates.PlayerTurn:
                StartPlayerTurn();
                break;
            case TurnStates.EnemyTurn:
                StartEnemyTurn();
                break;
            default:
                break;
        }
    }

    private void EndTurn(TurnStates turnToEnd)
    {
        switch (turnToEnd)
        {
            case TurnStates.PlayerTurn:
                EndPlayerTurn();
                break;
            case TurnStates.EnemyTurn:
                EndEnemyTurn();
                break;
            default:
                break;
        }
    }
    
    
    private void StartPlayerTurn()
    {
        foreach (var item in playableCharacterList)
        {
            if (!item.isStun)
            {
                item.hasMovementLeft = true;
                item.hasActionLeft = true;
            }
        }

        foreach (var item in nonPlayableCharacterList)
        {
            item.isStun = false;
        }
    }

    private void StartEnemyTurn()
    {
        foreach (var item in nonPlayableCharacterList)
        {
            if (!item.isStun)
            {
                item.hasMovementLeft = true;
                item.hasActionLeft = true;
            }
        }
        
        foreach (var item in playableCharacterList)
        {
            item.isStun = false;
        }
    }


    private void EndPlayerTurn()
    {
        foreach (var item in playableCharacterList)
        {
            item.canCounter = true;
        }
    }

    private void EndEnemyTurn()
    {
        foreach (var item in nonPlayableCharacterList)
        {
            item.canCounter = true;
        }
    }
    private void InitializeLists()
    {
        var temp = FindObjectsOfType<BaseArchetype>();

        foreach (var item in temp)
        {
            if (item.CompareTag("PlayableCharacter"))
            {
                playableCharacterList.Add(item);
            }
            else if (item.CompareTag("NonPlayableCharacter"))
            {
                nonPlayableCharacterList.Add(item);   
            }
        }
    }
    
}
