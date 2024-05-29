using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBuffer {
    public TileBuffer()
    {
        tiles = new();
    }

    public Dictionary<Vector2Int, TileDefinition> tiles;

    public bool IsSetTile(Vector2Int cell) => tiles.ContainsKey(cell);
    public bool IsAlive(Vector2Int cell) {
        if (!IsSetTile(cell))
            return false;
        return !tiles[cell].isDead;
    }
    public void SetTile(Vector2Int cell, TileDefinition tile) {
        if(tiles.ContainsKey(cell))
            tiles[cell] = tile;
        else
            tiles.Add(cell, tile);
    }
    public TileDefinition GetTile(Vector2Int pos) {
        if (!tiles.ContainsKey(pos))
            return TileDefinition.NoResult;
        return tiles[pos];
    }

    public int CountAliveNeighbors(Vector2Int cell) {
        int count = 0;

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int neighbor = cell + new Vector2Int(x, y);

                if (x == 0 && y == 0)
                    continue; //skipped to not count self
                else if (IsAlive(neighbor))
                            count++;
            }
        }

        Debug.Log($"{cell} has {count} alive neighbours");
        return count;
    }
    public int CountAnyNeighbors(Vector2Int cell)
    {
        int count = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int neighbor = cell + new Vector2Int(x, y);

                if (x == 0 && y == 0)
                    continue; //skipped to not count self
                else if (IsSetTile(neighbor)) 
                    count++;
            }
        }
        return count;
    }
    public int CountNeighborsOfType(Vector2Int cell, TileDefinition tile)
    {
        int count = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int neighbor = cell + new Vector2Int(x, y);

                if (x == 0 && y == 0) {
                    continue;
                }

                if (IsSetTile(neighbor)) {
                    if (tiles[neighbor] == tile)
                        count++;
                }
            }
        }

        return count;
    }


    public void Clear()
    {
        tiles.Clear();
    }
    public void DrawToTilemap(Tilemap tilemap) {
        tilemap.ClearAllTiles();

        foreach (var tile in tiles)
        {
            tilemap.SetTile((Vector3Int)tile.Key, tile.Value.Tile);
        }
    }
}


