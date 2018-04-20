using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defector : Unit {

    public override BattleResult Battle(UnitType enemy)
    {
        if(enemy == UnitType.king)
        {
            base.Draw();
            return BattleResult.win;
        }else
        {
            base.Die();
            return BattleResult.lose;
        }
        
    }
}
