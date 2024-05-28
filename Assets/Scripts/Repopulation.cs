using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rule_Repopulation", menuName = "Rules/Repopulation")]
public class Repopulation : Rule
{
    public TileDefinition Tile;

    public override TileDefinition Evaluate(GameBoard board, Vector3Int positon)
    {
        /*
        Debug.Log($"Tile at {positon} is {board.IsAlive(positon)}");
        Debug.Log($"Tile at {positon} has {board.CountNeighbors(positon)} neighbours");
        */

        if (!board.IsAlive(positon) && board.CountNeighbors(positon) == 3)
        {
            Debug.Log($"repopulating at {positon}");
            return Tile;
        }
        return TileDefinition.NoResult;
    }
}
