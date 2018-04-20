using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventLogger : MonoBehaviour {

    public Text log;

    public static EventLogger controller;

    string player1Log = "";
    string player2Log = "";

    void Awake()
    {
        controller = this;
    }
    
    public void SwitchLog(PlayerID player)
    {
        switch (player)
        {
            case PlayerID.player1:
                log.text = player1Log;
                break;
            case PlayerID.player2:
                log.text = player2Log;
                break;
            default:
                break;
        }
    }

    public void ResetSpeech()
    {
        player1Log = "";
        player2Log = "";
    }

    public void addBattle(PlayerID player, UnitType yourUnit, UnitType theirUnit, Vector2 locationTheirs, BattleResult result)
    {
        switch (result)
        {
            case BattleResult.win:
                switch (player)
                {
                    case PlayerID.player1:
                        player1Log += "\nYour " + yourUnit + "destroyed " + theirUnit + " at " + locationTheirs.x + ", " + locationTheirs.y + "!\n";
                        break;
                    case PlayerID.player2:
                        player2Log += "\nYour " + yourUnit + "destroyed " + theirUnit + " at " + locationTheirs.x + ", " + locationTheirs.y + "!\n";
                        break;
                    default:
                        break;
                }
                break;
            case BattleResult.lose:
                switch (player)
                {
                    case PlayerID.player1:
                        player1Log += "\nYour " + yourUnit + " was destroyed by " + theirUnit + " at " + locationTheirs.x + ", " + locationTheirs.y + "!\n";
                        break;
                    case PlayerID.player2:
                        player2Log += "\nYour " + yourUnit + " was destroyed by " + theirUnit + " at " + locationTheirs.x + ", " + locationTheirs.y + "!\n";
                        break;
                    default:
                        break;
                }
                break;
            case BattleResult.draw:
                player1Log += "\nA unit " + theirUnit + " was discovered at " + locationTheirs.x + ", " + locationTheirs.y + "!\n";
                player2Log += "\nA unit " + theirUnit + " was discovered at " + locationTheirs.x + ", " + locationTheirs.y + "!\n";
                return;
            case BattleResult.count:
                break;
            default:
                break;
        }
    }

    public void ScoutInfo(PlayerID scouter, Knowledge knowledge, Vector2 location)
    {
        switch (scouter)
        {
            case PlayerID.player1:
                switch (knowledge.direction)
                {
                    case Direction.Count:
                        player1Log += "\nFound " + knowledge.type + " at " + location.x + ", " + location.y + "\n";
                        break;
                    default:
                        player1Log += "\nSomething was at " + location.x + ", " + location.y + " recently\n";
                        break;
                }
                player1Log += "";
                break;
            case PlayerID.player2:
                switch (knowledge.direction)
                {
                    case Direction.Count:
                        player2Log += "\nFound " + knowledge.type + " at " + location.x + ", " + location.y + "\n";
                        break;
                    default:
                        player2Log += "\nSomething was at " + location.x + ", " + location.y + " recently\n";
                        break;
                }
                break;
        }
    }
}
