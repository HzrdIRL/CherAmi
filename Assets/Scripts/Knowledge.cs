using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Knowledge {
    public Direction direction;
    public UnitType type;
    public int age { get; set; }
    public bool known;

    public Knowledge(Direction direction, UnitType type, bool known)
    {
        this.type = type;
        this.direction = direction;
        this.known = known;
        age = 0;
    }

    public static Knowledge isHere(UnitType type)
    {
        return new Knowledge(direction: Direction.Count, type: type, known: true);
    }

    public static Knowledge MovementOccurred(Vector2 origin, Vector2 destination)
    {
        Direction direction = Direction.Count;

        Vector2 directionVector = new Vector2(destination.x - origin.x, destination.y - origin.y);

        if (directionVector.y == 0)
        {
            if (directionVector.x == 1)
            {
                direction = Direction.EAST;
            }
            else if (directionVector.x == -1)
            {
                direction = Direction.WEST;
            }
        }

        if (directionVector.y == 1)
        {
            if (directionVector.x == 1)
            {
                direction = Direction.NORTHEAST;
            }
            else if (directionVector.x == -1)
            {
                direction = Direction.NORTHWEST;
            }
            else if (directionVector.x == 0)
            {
                direction = Direction.NORTH;
            }
        }

        if (directionVector.y == -1)
        {
            if (directionVector.x == 1)
            {
                direction = Direction.SOUTHEAST;
            }
            else if (directionVector.x == -1)
            {
                direction = Direction.SOUTHWEST;
            }
            else if (directionVector.x == 0)
            {
                direction = Direction.SOUTH;
            }
        }

        return new Knowledge(direction: direction, type: UnitType.count, known: false);
    }

    internal void AgeUp()
    {
        age++;
    }
}
