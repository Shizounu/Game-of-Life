using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rule_Overpopulation", menuName = "Rules/Overpopulation")]
public class Overpopulation : Rule
{
    public override TileDefinition Evaluate(TileBuffer board, Vector2Int positon)
    {
        if(board.IsAlive(positon) && board.CountAliveNeighbors(positon) > 3)
            return TileDefinition.Dead;
        return TileDefinition.NoResult;
    }
}
