using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Rule : ScriptableObject
{
    public abstract TileDefinition Evaluate(TileBuffer board, Vector2Int positon);
}
