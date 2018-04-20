using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType {soldier, saboteur, tank, king, count}
public enum BattleResult {win, lose, draw, count}

public class Unit : MonoBehaviour {

    public bool canMove = true;
    public PlayerID player;
    public bool isDead;
    public float moveSpeed = 1f, moveDistance = 1f;
    public UnitType type;
    public Cell currentPosition, targetPosition;
    public GameObject[] directions;

    void Start()
    {
        SetCurrent();
        canMove = true;
    }

    public void SetCurrent()
    {
        currentPosition = GameManager.controller.board[(int)transform.position.x, (int)transform.position.y];
        targetPosition = currentPosition;
    }

    public void Move()
    {
        StartCoroutine(MoveUnit(transform.position, targetPosition.coords));
        currentPosition = targetPosition;
        canMove = false;
    }

    IEnumerator MoveUnit(Vector2 startPos, Vector2 endPos)
    {
        while(transform.position.x != endPos.x || transform.position.y != endPos.y){
            transform.position = Vector2.Lerp(startPos, endPos, moveSpeed);
            transform.position += new Vector3(0, 0, -2);
            yield return null;
        }
    }

    public void Draw()
    {
        StartCoroutine(Stalemate());
        targetPosition = currentPosition;
        canMove = false;
    }

    IEnumerator Stalemate()
    {
        yield return StartCoroutine(MoveUnit(transform.position, targetPosition.coords));
        yield return StartCoroutine(MoveUnit(targetPosition.coords, transform.position));
    }

    public void Die()
    {
        StartCoroutine(DestroyUnit());
    }

    IEnumerator DestroyUnit()
    {
        isDead = true;
        gameObject.SetActive(false);
        yield return null;
    }
    
    public virtual BattleResult Battle(UnitType enemy)
    {
        return BattleResult.count;
    }

    public bool IsValidMove(Cell targetPos)
    {
        switch (GameManager.controller.turn)
        {
            case PlayerID.player1:
                foreach (Unit unit in GameManager.controller.player1.units)
                {
                    if(targetPos == unit.targetPosition)
                    {
                        return false;
                    }
                }
                    break;
            case PlayerID.player2:
                foreach (Unit unit in GameManager.controller.player2.units)
                {
                    if (targetPos == unit.targetPosition)
                    {
                        return false;
                    }
                }
                break;
            default:
                break;
        }
        
        if (Mathf.Abs(targetPos.coords.x - currentPosition.coords.x) <= moveDistance && Mathf.Abs(targetPos.coords.y - currentPosition.coords.y) <= moveDistance)
        {
            return true;
        }
        return false;
    }

    void OnMouseDown()
    {
        if (GameManager.controller.isScouting)
        {
            GameManager.controller.isScouting = false;
        }
        
        ClearMove();
        GameManager.controller.selectedUnit = this;
    }

    public void ClearMove()
    {
        //Resets thigns incase we had a previous move set.
        targetPosition = currentPosition;
        foreach (GameObject dir in directions)
        {
            dir.SetActive(false);
        }
    }

    public Cell MoveInDirection(Direction dir)
    {
        Vector2 target;
        switch (dir)
        {
            case Direction.NORTH:
                target = new Vector2(currentPosition.coords.x, currentPosition.coords.y + 1);
                break;
            case Direction.NORTHEAST:
                target = new Vector2(currentPosition.coords.x + 1, currentPosition.coords.y + 1);
                break;
            case Direction.EAST:
                target = new Vector2(currentPosition.coords.x + 1, currentPosition.coords.y);
                break;
            case Direction.SOUTHEAST:
                target = new Vector2(currentPosition.coords.x + 1, currentPosition.coords.y - 1);
                break;
            case Direction.SOUTH:
                target = new Vector2(currentPosition.coords.x, currentPosition.coords.y - 1);
                break;
            case Direction.SOUTHWEST:
                target = new Vector2(currentPosition.coords.x - 1, currentPosition.coords.y - 1);
                break;
            case Direction.WEST:
                target = new Vector2(currentPosition.coords.x - 1, currentPosition.coords.y);
                break;
            case Direction.NORTHWEST:
                target = new Vector2(currentPosition.coords.x - 1, currentPosition.coords.y + 1);
                break;
            case Direction.Count:
                target = new Vector2(currentPosition.coords.x, currentPosition.coords.y);
                break;
            default:
                target = new Vector2(currentPosition.coords.x, currentPosition.coords.y);
                break;
        }

        Cell targetPos = GameManager.controller.board[(int)target.x, (int)target.y];

        return targetPos;
    }
}
