using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Direction
{
    NORTH = 0,
    NORTHEAST,
    EAST,
    SOUTHEAST,
    SOUTH,
    SOUTHWEST,
    WEST,
    NORTHWEST,

    Count
}

public class Cell : MonoBehaviour {

    public Vector2 coords;

    public List<Knowledge> PlayerKnowledge;

    public GameObject[] directions;

    public Image cellColour;

    public Sprite enemy;
    public Image scoutingSprite;

    void Awake()
    {
        coords = new Vector2((int)transform.position.x, (int)transform.position.y);
    }

    void OnMouseDown()
    {
        Unit unit = GameManager.controller.selectedUnit;
        if (GameManager.controller.isScouting)
        {
            if(GameManager.controller.turn == GameManager.controller.player1.id)
            {
                GameManager.controller.player1.scoutingLocation = this;
                GameManager.controller._isScouting = false;
                scoutingSprite.sprite = GameManager.controller.sprites[5];
                scoutingSprite.gameObject.SetActive(true);
                //cellColour.color = Color.green;
            }
            else
            {
                GameManager.controller.player2.scoutingLocation = this;
                GameManager.controller._isScouting = false;
                scoutingSprite.sprite = GameManager.controller.sprites[5];
                scoutingSprite.gameObject.SetActive(true);
                //cellColour.color = Color.green;
            }
        }
        else if (GameManager.controller.selectedUnit != null)
        {
            if (unit == null)
                return;
            if (unit.IsValidMove(this))
            {
                //transform.Find("Canvas/Background").GetComponent<Image>().color = Color.green;
                unit.ClearMove();
                unit.targetPosition = this;
                unit.directions[(int)CheckDirection(unit.currentPosition.coords)].SetActive(true);
            }
        }
    }

    public Direction CheckDirection(Vector2 origin)
    {
        Direction direction = Direction.Count;

        Vector2 directionVector = new Vector2(transform.position.x - origin.x, transform.position.y - origin.y);

        if (directionVector.y == 0)
        {
            if (directionVector.x >= 1)
            {
                direction = Direction.EAST;
            }
            else if (directionVector.x <= -1)
            {
                direction = Direction.WEST;
            }
        }

        if (directionVector.y >= 1)
        {
            if (directionVector.x >= 1)
            {
                direction = Direction.NORTHEAST;
            }
            else if (directionVector.x <= -1)
            {
                direction = Direction.NORTHWEST;
            }
            else if (directionVector.x == 0)
            {
                direction = Direction.NORTH;
            }
        }

        if (directionVector.y <= -1)
        {
            if (directionVector.x >= 1)
            {
                direction = Direction.SOUTHEAST;
            }
            else if (directionVector.x <= -1)
            {
                direction = Direction.SOUTHWEST;
            }
            else if (directionVector.x == 0)
            {
                direction = Direction.SOUTH;
            }
        }

        return direction;
    }

    public Direction GetOppositeDireciton(Vector2 origin)
    {
        Direction direction = Direction.Count;

        Vector2 directionVector = new Vector2(transform.position.x - origin.x, transform.position.y - origin.y);

        if (directionVector.y == 0)
        {
            if (directionVector.x >= 1)
            {
                direction = Direction.WEST;
            }
            else if (directionVector.x <= -1)
            {
                direction = Direction.EAST;
            }
        }

        if (directionVector.y >= 1)
        {
            if (directionVector.x >= 1)
            {
                direction = Direction.SOUTHWEST;
            }
            else if (directionVector.x <= -1)
            {
                direction = Direction.SOUTHEAST;
            }
            else if (directionVector.x == 0)
            {
                direction = Direction.SOUTH;
            }
        }

        if (directionVector.y <= -1)
        {
            if (directionVector.x >= 1)
            {
                direction = Direction.NORTHWEST;
            }
            else if (directionVector.x <= -1)
            {
                direction = Direction.NORTHEAST;
            }
            else if (directionVector.x == 0)
            {
                direction = Direction.NORTH;
            }
        }

        return direction;
    }
}

