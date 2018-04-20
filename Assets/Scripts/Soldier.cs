using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    public override BattleResult Battle(UnitType enemy)
    {
        switch (enemy)
        {
            case UnitType.soldier:
                base.Draw();
                return BattleResult.draw;
            case UnitType.saboteur:
                base.Move();
                return BattleResult.win;
            case UnitType.tank:
                base.Die();
                return BattleResult.lose;
            default:
                return BattleResult.count;
        }
    }
}
