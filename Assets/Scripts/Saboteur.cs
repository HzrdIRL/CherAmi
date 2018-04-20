using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saboteur : Unit
{
    public override BattleResult Battle(UnitType enemy)
    {
        switch (enemy)
        {
            case UnitType.soldier:
                base.Die();
                return BattleResult.lose;
            case UnitType.saboteur:
                base.Draw();
                return BattleResult.draw;
            case UnitType.tank:
                base.Move();
                return BattleResult.win;
            default:
                return BattleResult.count;
        }
    }
}
