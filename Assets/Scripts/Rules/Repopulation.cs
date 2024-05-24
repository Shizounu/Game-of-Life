using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rule_Repopulation", menuName = "Rules/Repopulation")]
public class Repopulation : Rule
{
    public override RuleResult Evaluate(GameBoard board, Vector3Int positon)
    {
        if(!board.IsAlive(positon) && board.CountNeighbors(positon) == 3) {
            return RuleResult.Tile1;
        }
        return RuleResult.NoResult;
    }
}
