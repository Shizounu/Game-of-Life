using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RuleResult
{
    /// <summary>
    /// The rule does not make a change to this state
    /// </summary>
    NoResult,

    /// <summary>
    /// The rule returns a dead tile
    /// </summary>
    Dead,
    Tile1,
    Tile2,
    Tile3

}
