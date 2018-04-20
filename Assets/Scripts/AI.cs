using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AI {

    static Dictionary<Cell, List<Knowledge>> knownActivity;

    //AI is player 2
    public static void ProcessAI()
    {
        GameManager.controller.player2.scoutingLocation = GameManager.controller.board[6, 0];
        List<Knowledge>[,] baseKnowledge = GameManager.controller.knowledge2;
        knownActivity = new Dictionary<Cell, List<Knowledge>>();

        for (int i = 0; i < GameManager.boardSize; i++)
        {
            for (int k = 0; k < GameManager.boardSize; k++)
            {
                foreach (Knowledge know in baseKnowledge[i, k])
                {
                    Cell current = GameManager.controller.board[i, k];
                    if (know.known) {
                        if (knownActivity.ContainsKey(current)){
                            knownActivity[current].Add(know);
                        } else {
                            knownActivity.Add(current, new List<Knowledge>());
                            knownActivity[current].Add(know);
                        }
                    }
                }
            }
        
        }

        foreach (Unit piece in GameManager.controller.player2.units)
        {
            piece.SetCurrent();

            switch (piece.type)
            {
                case UnitType.soldier:
                    SoldierAi(piece);
                    break;
                case UnitType.saboteur:
                    SaboteurAi(piece);
                    break;
                case UnitType.tank:
                    TankAi(piece);
                    break;
                case UnitType.king:
                    KingAi(piece);
                    break;
                case UnitType.count:
                    piece.targetPosition = RandomMovement(piece);
                    break;
                default:
                    piece.targetPosition = RandomMovement(piece);
                    break;
            }
        }
    }

    static void SoldierAi(Unit piece)
    {
        foreach (KeyValuePair<Cell, List<Knowledge>> activity in knownActivity)
        {
            foreach (Knowledge item in activity.Value)
            {
                switch (item.type)
                {
                    case UnitType.soldier:
                        //Random movement
                        piece.targetPosition = RandomMovement(piece);
                        break;
                    case UnitType.saboteur:
                        //Move towards
                        piece.targetPosition =  piece.MoveInDirection(activity.Key.CheckDirection(piece.currentPosition.coords));
                        
                        break;
                    case UnitType.tank:
                        //Move away
                        piece.targetPosition = piece.MoveInDirection(activity.Key.GetOppositeDireciton(piece.currentPosition.coords));
                        break;
                    case UnitType.king:
                        //Move towards
                        piece.targetPosition = piece.MoveInDirection(activity.Key.CheckDirection(piece.currentPosition.coords));
                        break;
                    case UnitType.count:
                        //Random movement
                        piece.targetPosition = RandomMovement(piece);
                        break;
                    default:
                        piece.targetPosition = RandomMovement(piece);
                        break;
                }
            }
        }

        if (piece.currentPosition == piece.targetPosition)
            piece.targetPosition = RandomMovement(piece);

        int count = 0;
        while (!piece.IsValidMove(piece.targetPosition) && count < 3)
        {
            piece.targetPosition = RandomMovement(piece);
            count++;
        }

        if (count > 3)
            piece.targetPosition = piece.currentPosition;
    }

    static void SaboteurAi(Unit piece)
    {

        foreach (KeyValuePair<Cell, List<Knowledge>> activity in knownActivity)
        {
            foreach (Knowledge item in activity.Value)
            {
                switch (item.type)
                {
                    case UnitType.soldier:
                        //Move away
                        piece.targetPosition = piece.MoveInDirection(activity.Key.GetOppositeDireciton(piece.currentPosition.coords));
                        break;
                    case UnitType.saboteur:
                        //Random movement
                        piece.targetPosition = RandomMovement(piece);
                        break;
                    case UnitType.tank:
                        //Move towards
                        piece.targetPosition = piece.MoveInDirection(activity.Key.CheckDirection(piece.currentPosition.coords));
                        break;
                    case UnitType.king:
                        //Move towards
                        piece.targetPosition = piece.MoveInDirection(activity.Key.CheckDirection(piece.currentPosition.coords));
                        break;
                    case UnitType.count:
                        //Random movement
                        piece.targetPosition = RandomMovement(piece);
                        break;
                    default:
                        break;
                }
            }
        }

        if (piece.currentPosition == piece.targetPosition)
            piece.targetPosition = RandomMovement(piece);

        int count = 0;
        while (!piece.IsValidMove(piece.targetPosition) && count < 3)
        {
            piece.targetPosition = RandomMovement(piece);
            count++;
        }

        if (count > 3)
            piece.targetPosition = piece.currentPosition;
    }

    static void TankAi(Unit piece)
    {

        foreach (KeyValuePair<Cell, List<Knowledge>> activity in knownActivity)
        {
            foreach (Knowledge item in activity.Value)
            {
                switch (item.type)
                {
                    case UnitType.soldier:
                        //Move towards
                        piece.targetPosition = piece.MoveInDirection(activity.Key.CheckDirection(piece.currentPosition.coords));
                        break;
                    case UnitType.saboteur:
                        //Move away
                        piece.targetPosition = piece.MoveInDirection(activity.Key.GetOppositeDireciton(piece.currentPosition.coords));
                        break;
                    case UnitType.tank:
                        //Random movement
                        piece.targetPosition = RandomMovement(piece);
                        break;
                    case UnitType.king:
                        //Move towards
                        piece.targetPosition = piece.MoveInDirection(activity.Key.CheckDirection(piece.currentPosition.coords));
                        break;
                    case UnitType.count:
                        //Random movement
                        piece.targetPosition = RandomMovement(piece);
                        break;
                    default:
                        break;
                }
            }
        }

        if (piece.currentPosition == piece.targetPosition)
            piece.targetPosition = RandomMovement(piece);

        int count = 0;
        while (!piece.IsValidMove(piece.targetPosition) && count < 3)
        {
            piece.targetPosition = RandomMovement(piece);
            count++;
        }

        if (count > 3)
            piece.targetPosition = piece.currentPosition;
    }

    static void KingAi(Unit piece)
    {

        foreach (KeyValuePair<Cell, List<Knowledge>> activity in knownActivity)
        {
            foreach (Knowledge item in activity.Value)
            {
                switch (item.type)
                {
                    case UnitType.soldier:
                        //Move away
                        piece.targetPosition = piece.MoveInDirection(activity.Key.GetOppositeDireciton(piece.currentPosition.coords));
                        break;
                    case UnitType.saboteur:
                        //Move away
                        piece.targetPosition = piece.MoveInDirection(activity.Key.GetOppositeDireciton(piece.currentPosition.coords));
                        break;
                    case UnitType.tank:
                        //Move away
                        piece.targetPosition = piece.MoveInDirection(activity.Key.GetOppositeDireciton(piece.currentPosition.coords));
                        break;
                    case UnitType.king:
                        //Random movement
                        piece.targetPosition = RandomMovement(piece);
                        break;
                    case UnitType.count:
                        //Random movement
                        piece.targetPosition = RandomMovement(piece);
                        break;
                    default:
                        break;
                }
            }
        }

        if (piece.currentPosition == piece.targetPosition)
            piece.targetPosition = RandomMovement(piece);

        int count = 0;
        while (!piece.IsValidMove(piece.targetPosition) && count < 3)
        {
            piece.targetPosition = RandomMovement(piece);
            count++;
        }

        if (count > 3)
            piece.targetPosition = piece.currentPosition;
    }

    static Cell RandomMovement(Unit piece)
    {
        Vector2 currentPos = piece.currentPosition.coords;
        Vector2 targetPos = currentPos;
        Cell target;
        int count = 0;
        do
        {
            float chance = Random.value;
            if (chance >= 0.5)
                //Move Down
                targetPos = new Vector2(currentPos.x, currentPos.y - 1);
            else if (chance >= 0.25)
                //Move down left
                targetPos = new Vector2(currentPos.x - 1, currentPos.y - 1);
            else
                //Move down right
                targetPos = new Vector2(currentPos.x + 1, currentPos.y - 1);

            targetPos = new Vector2(Mathf.Clamp(targetPos.x, 0, GameManager.boardSize), Mathf.Clamp(targetPos.y, 0, GameManager.boardSize));
            target =  GameManager.controller.board[(int)targetPos.x, (int)targetPos.y];
            count++;
        } while (!piece.IsValidMove(target) && count < 3);
        return target;
    }
}
