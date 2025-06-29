using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Monster
{
    public override void Init(int nIndex, IMonsterManager iMonsterManager)
    {
        base.Init(nIndex, iMonsterManager);

        dicState = new Dictionary<StateType, State>();

        dicState.Add(StateType.Move, new Move_Monster(this, iMonsterManager));
        dicState.Add(StateType.Die, new Die_Monster(this, true));

        stateType = StateType.Move;
    }
}
