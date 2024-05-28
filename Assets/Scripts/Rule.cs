using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Rule : ScriptableObject
{
    public abstract TileDefinition Evaluate(GameBoard board, Vector3Int positon);
}
