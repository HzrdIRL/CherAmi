using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager controller;

    public const int boardSize = 10;

    public List<Cell> cellList;
    public Cell[,] board = new Cell[boardSize, boardSize];
    public List<Knowledge>[,] knowledge1 = new List<Knowledge>[boardSize, boardSize], knowledge2 = new List<Knowledge>[boardSize, boardSize];
    public ViewportBlocker viewportBlocker;
    public GameObject jason;
    public Player player1, player2;
    public Unit _selectedUnit;
    public Unit selectedUnit { set
        {
            if (value != null)
                jason.SetActive(true);
            else
                jason.SetActive(false);
            _selectedUnit = value;
        }
        get
        {
            return _selectedUnit;
        }
    }
    public Button scoutingButton, aiButton;
    public Sprite[] sprites;
    public bool singlePlayer;

    public bool _isScouting;
    public bool isScouting
    {
        set
        {
            _isScouting = value;
            selectedUnit = null;
            if (value == true)
            {
                if (turn == player1.id)
                {
                    if (player1.scoutingLocation != null)
                    {
                        board[(int)player1.scoutingLocation.coords.x, (int)player1.scoutingLocation.coords.y].cellColour.color = Color.white;
                        board[(int)player1.scoutingLocation.coords.x, (int)player1.scoutingLocation.coords.y].scoutingSprite.sprite = null;
                        player1.scoutingLocation = null;
                    }
                }
                else
                {
                    if (player2.scoutingLocation != null)
                    {
                        board[(int)player2.scoutingLocation.coords.x, (int)player2.scoutingLocation.coords.y].cellColour.color = Color.white;
                        board[(int)player2.scoutingLocation.coords.x, (int)player2.scoutingLocation.coords.y].scoutingSprite.sprite = null;
                        player2.scoutingLocation = null;
                    }
                }
            }
        }

        get
        {
            return _isScouting;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            selectedUnit = null;
        }
            
    }

    public void switchAI()
    {
        singlePlayer = !singlePlayer;
        aiButton.GetComponentInChildren<Text>().text = (singlePlayer == true ? "AI: On" : "AI: Off");
    }

    public PlayerID turn = PlayerID.player1;

    // Use this for initialization
    void Awake() {
        controller = this;
        foreach (Cell cell in cellList)
        {
            board[(int)cell.coords.x, (int)cell.coords.y] = cell;
        }

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                knowledge1[i, j] = new List<Knowledge>();
                knowledge2[i, j] = new List<Knowledge>();
            }
        }
        viewportBlocker.Rules();
    }
    public void Start()
    {
        aiButton.GetComponentInChildren<Text>().text = (singlePlayer == true ? "AI: On" : "AI: Off");
    }

    public void EndTurnOnClick()
    {
        StartCoroutine(EndTurn());
    }
    
    public IEnumerator EndTurn()
    {
        switch (turn)
        {
            case PlayerID.player1:
                if (singlePlayer)
                    viewportBlocker.ReadyUp("Player 1, are you ready?", false);
                else
                    viewportBlocker.ReadyUp("Player 2, are you ready?", false);
                while(viewportBlocker.viewportBlocker.alpha < 1)
                {
                    yield return null;
                }
                removeScoutingVisual(player1);
                switchTurn(player2.id);
                selectedUnit = null;
                

                if (singlePlayer)
                {
                    AI.ProcessAI();
                    EndTurnOnClick();
                } else
                {
                    viewportBlocker.ReadyUp("Player 2, are you ready?", true);
                }
                yield break;
            case PlayerID.player2:
                viewportBlocker.ReadyUp("Player 1, are you ready?", false);

                while (viewportBlocker.viewportBlocker.alpha < 1)
                {
                    yield return null;
                }
                removeScoutingVisual(player2);

                EnableUnits();

                ResolveBattles();

                MovePlayerUnits(player1, knowledge2);
                MovePlayerUnits(player2, knowledge1);
                
                Scout(player1, knowledge1);
                Scout(player2, knowledge2);

                switchTurn(player1.id);

                ResetPieces();
                AgeKnowledge();
                selectedUnit = null;
                killUnits(player1);
                killUnits(player2);

                viewportBlocker.ReadyUp("Player 1, are you ready?", true);
                WinCheck(player1);
                WinCheck(player2);
                yield break;
            default:
                break;
        }
    }

    private void EnableUnits()
    {
        player1.EnableUnits();
        player2.EnableUnits();
    }

    private void WinCheck(Player player)
    {
        foreach(Unit unit in player.units)
        {
            if (unit.type == UnitType.king && unit.currentPosition.coords.y == player.winRow)
            {
                Debug.Log("Winner");
                win(player.id);
            }
        }
        
    }

    private void AgeKnowledge()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                for (int k = 0; k < knowledge1[i, j].Count; k++)
                {
                    knowledge1[i, j][k].AgeUp();
                }

                for (int k = 0; k < knowledge2[i, j].Count; k++)
                {
                    knowledge2[i, j][k].AgeUp();
                }
            }
        }
    }

    private void MovePlayerUnits(Player player, List<Knowledge>[,] knowledge)
    {
        foreach (Unit unit in player.units)
        {
            if (unit.canMove && !unit.isDead)
            {
                Vector2 origin = unit.currentPosition.coords;
                Vector2 destination = unit.targetPosition.coords;
                unit.Move();
                if (origin != destination)
                    knowledge[(int)origin.x, (int)origin.y].Add(Knowledge.MovementOccurred(origin, destination));
            }

            unit.ClearMove();
        }
    }

    private void killUnits(Player player) {
        List<Unit> markedForDeletion = new List<Unit>();

        foreach (Unit unit in player.units)
        {
            if (unit.isDead)
            {
                markedForDeletion.Add(unit);
            }
        }

        for (int i = 0; i < markedForDeletion.Count; i++)
        {
            player.units.Remove(markedForDeletion[i]);
            markedForDeletion[i].gameObject.SetActive(false);
        }
    }

    public void Scout(Player player, List<Knowledge>[,] knowledge)
    {
        List<Knowledge> markedForDeletion = new List<Knowledge>(); 

        if (player.scoutingLocation != null)
        {
            switch (player.id)
            {
                case PlayerID.player1:
                    foreach(Unit unit in player2.units)
                    {
                        if (unit.targetPosition == player.scoutingLocation) {
                            removeOldKnowledge(player, knowledge, unit.type);
                            Knowledge newKnowledge = Knowledge.isHere(unit.type);
                            knowledge[(int)player.scoutingLocation.coords.x, (int)player.scoutingLocation.coords.y].Add(newKnowledge);
                            EventLogger.controller.ScoutInfo(PlayerID.player1, newKnowledge, unit.currentPosition.coords);
                        }
                    }
                    break;
                case PlayerID.player2:
                    foreach (Unit unit in player1.units)
                    {
                        if (unit.targetPosition == player.scoutingLocation)
                        {
                            removeOldKnowledge(player, knowledge, unit.type);
                            Knowledge newKnowledge = Knowledge.isHere(unit.type);
                            knowledge[(int)player.scoutingLocation.coords.x, (int)player.scoutingLocation.coords.y].Add(newKnowledge);
                            EventLogger.controller.ScoutInfo(PlayerID.player2, newKnowledge, unit.currentPosition.coords);
                        }
                    }
                    break;
                default:
                    break;
            }
            for (int i = 0; i < knowledge[(int)player.scoutingLocation.coords.x, (int)player.scoutingLocation.coords.y].Count; i++)
            {
                if (knowledge[(int)player.scoutingLocation.coords.x, (int)player.scoutingLocation.coords.y][i].known && knowledge[(int)player.scoutingLocation.coords.x, (int)player.scoutingLocation.coords.y][i].age > 2)
                {
                    markedForDeletion.Add(knowledge[(int)player.scoutingLocation.coords.x, (int)player.scoutingLocation.coords.y][i]);
                }
                else if (knowledge[(int)player.scoutingLocation.coords.x, (int)player.scoutingLocation.coords.y][i].age <= 2 && !knowledge[(int)player.scoutingLocation.coords.x, (int)player.scoutingLocation.coords.y][i].known)
                {
                    knowledge[(int)player.scoutingLocation.coords.x, (int)player.scoutingLocation.coords.y][i].known = true;
                    EventLogger.controller.ScoutInfo(PlayerID.player1, knowledge[(int)player.scoutingLocation.coords.x, (int)player.scoutingLocation.coords.y][i], player.scoutingLocation.coords);
                }
            }

            for (int i = 0; i < markedForDeletion.Count; i++)
            {
                knowledge[(int)player.scoutingLocation.coords.x, (int)player.scoutingLocation.coords.y].Remove(markedForDeletion[i]);
            }
            player.scoutingLocation = null;
        }
    }

    private void win(PlayerID player)
    {
        viewportBlocker.ReadyUp("Player " + ((int)player).ToString() + " Wins!", false);
    }

    private void ResetPieces()
    {
        player1.ResetPieces();
        player2.ResetPieces();
    }

    private void ResolveBattles()
    {
        foreach (Unit unit1 in player1.units)
        {
            List<Knowledge> knowledgeCell;
            foreach (Unit unit2 in player2.units)
            {
                if (unit1.targetPosition == unit2.targetPosition
                    || (unit1.targetPosition == unit2.currentPosition && unit1.currentPosition == unit2.targetPosition)
                    || unit2.targetPosition == null && unit1.targetPosition == unit2.currentPosition)
                {
                    BattleResult result = unit1.Battle(unit2.type);
                    knowledgeCell = knowledge2[(int)unit1.currentPosition.coords.x, (int)unit1.currentPosition.coords.y];
                    Knowledge knowledge = Knowledge.isHere(unit1.type);

                    removeOldKnowledge(player2, knowledge2, unit1.type);

                    knowledgeCell.Add(knowledge);
                    EventLogger.controller.addBattle(PlayerID.player1, unit1.type, unit2.type, unit2.currentPosition.coords, result);

                    result = unit2.Battle(unit1.type);
                    knowledgeCell = knowledge1[(int)unit2.currentPosition.coords.x, (int)unit2.currentPosition.coords.y];
                    knowledge = Knowledge.isHere(unit2.type);
                    removeOldKnowledge(player1, knowledge1, unit2.type);
                    knowledgeCell.Add(knowledge);
                    EventLogger.controller.addBattle(PlayerID.player2, unit2.type, unit1.type, unit1.currentPosition.coords, result);
                    break;
                }
            }
        }
    }

    void removeOldKnowledge(Player player, List<Knowledge>[,] knowledge, UnitType unitType)
    {
        Knowledge oldKnowledge = null;
        foreach (List<Knowledge> currentKnowledge in knowledge)
        {
            foreach (Knowledge knowledgeNugget in currentKnowledge)
            {
                if (knowledgeNugget.type == unitType)
                {
                    oldKnowledge = knowledgeNugget;
                    break;
                }
            }

            if (oldKnowledge != null)
            {
                currentKnowledge.Remove(oldKnowledge);
                break;
            }
        }
    }

    void removeScoutingVisual(Player player)
    {
        if (player.scoutingLocation != null)
        {
            board[(int)player.scoutingLocation.coords.x, (int)player.scoutingLocation.coords.y].cellColour.color = Color.white;
        }
    }

    void switchTurn(PlayerID player)
    {
        EventLogger.controller.SwitchLog(player);
        for (int row = 0; row < boardSize; row++) {
            for (int column = 0; column < boardSize; column++)
            {
                foreach (GameObject direction in board[row, column].directions)
                {
                    direction.SetActive(false);
                    board[row, column].scoutingSprite.gameObject.SetActive(false);
                    board[row, column].enemy = null;
                }

                switch (player)
                {
                    case PlayerID.player1:
                        board[row, column].PlayerKnowledge = knowledge1[row, column];
                        break;
                    case PlayerID.player2:
                        board[row, column].PlayerKnowledge = knowledge2[row, column];
                        break;
                    default:
                        break;
                }

                foreach(Knowledge knowledge in board[row, column].PlayerKnowledge)
                {
                    if (knowledge.known && knowledge.direction != Direction.Count)
                    {
                        board[row, column].directions[(int)knowledge.direction].SetActive(true);
                    }
                    else if (knowledge.known) {
                        board[row, column].scoutingSprite.sprite = sprites[(int)knowledge.type];
                        board[row, column].scoutingSprite.gameObject.SetActive(true);
                    }
                }
            }
        }

        switch (player)
        {
            case PlayerID.player1:
                player1.EnableUnits();
                player2.DisableUnits();
                break;
            case PlayerID.player2:
                player2.EnableUnits();
                player1.DisableUnits();
                break;
            default:
                break;
        }
        turn = player;
    }
}

public enum PlayerID : int {
    player1 = 0,
    player2
}