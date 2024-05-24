using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rule_Underpopulation", menuName = "Rules/Underpopulation")]
public class Underpopulation : Rule
{
    public override RuleResult Evaluate(GameBoard board, Vector3Int positon)
    {
        if(board.IsAlive(positon) && board.CountNeighbors(positon) < 2){
            return RuleResult.Dead;
        }
        return RuleResult.NoResult;
    }
}
