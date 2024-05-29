using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rule_Underpopulation", menuName = "Rules/Underpopulation")]
public class Underpopulation : Rule
{
    public override TileDefinition Evaluate(TileBuffer board, Vector2Int positon)
    {
        if(board.IsAlive(positon) && board.CountAliveNeighbors(positon) < 2)
            return TileDefinition.Dead;
        return TileDefinition.NoResult;
    }
}
