using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rule_Repopulation", menuName = "Rules/Repopulation")]
public class Repopulation : Rule
{
    public TileDefinition Tile;

    public override TileDefinition Evaluate(TileBuffer board, Vector2Int positon)
    {
        /*
        Debug.Log($"Tile at {positon} is {board.IsAlive(positon)}");
        Debug.Log($"Tile at {positon} has {board.CountNeighbors(positon)} neighbours");
        */

        if (!board.IsAlive(positon) && board.CountAliveNeighbors(positon) == 3)
            return Tile;
        return TileDefinition.NoResult;
    }
}
