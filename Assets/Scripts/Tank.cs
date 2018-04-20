using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Unit {
    public override BattleResult Battle(UnitType enemy)
    {
        switch (enemy)
        {
            case UnitType.soldier:
                base.Move();
                return BattleResult.draw;
            case UnitType.saboteur:
                base.Die();
                return BattleResult.win;
            case UnitType.tank:
                base.Draw();
                return BattleResult.lose;
            default:
                return BattleResult.count;
        }
    }
}
