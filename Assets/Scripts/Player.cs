using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player {
    public PlayerID id;
    public int winRow;
    public List<Unit> units = new List<Unit>();
    public Cell scoutingLocation;

    public Player(PlayerID id)
    {
        this.id = id;
        switch (id)
        {
            case PlayerID.player1:
                winRow = GameManager.boardSize;
                break;
            case PlayerID.player2:
                winRow = 0;
                break;
            default:
                break;
        }
    }

    public  void ResetPieces()
    {
        foreach(Unit unit in units)
        {
            unit.canMove = true;
        }
    }

    public void EnableUnits()
    {
        foreach (Unit unit in units)
        {
            unit.gameObject.SetActive(true);
        }
    }
    public  void DisableUnits()
    {
        foreach (Unit unit in units)
        {
            unit.gameObject.SetActive(false);
        }
    }
}
