using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Tile", menuName = "Game of Life/Tile")]
public class TileDefinition : ScriptableObject {
    public TileDefinition Init(Tile Tile, bool isDead = false, bool isNon = false) {
        this.Tile = Tile;
        this.isDead = isDead;
        this.isNon = isNon;

        return this;
    }
    public Tile Tile;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isNon;

    public static TileDefinition Dead => CreateInstance<TileDefinition>().Init(null, true);
    public static TileDefinition NoResult => CreateInstance<TileDefinition>().Init(null, false, true);
}