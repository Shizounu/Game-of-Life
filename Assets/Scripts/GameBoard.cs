using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    [Header("Logic")]
    [SerializeField] private Pattern startPattern;
    [SerializeField] private List<Rule> rules;

    [SerializeField] private float updateInterval = 0.05f;
    [SerializeField] private bool isActive = true;

    [Header("Buffers")]
    [SerializeField] private Tilemap frontBuffer;
    [SerializeField] private Tilemap backBuffer;

    private HashSet<Vector3Int> aliveCells;
    private HashSet<Vector3Int> cellsToCheck;

    public int population { get; private set; }
    public int iterations { get; private set; }
    public float time { get; private set; }

    private void Awake()
    {
        aliveCells = new HashSet<Vector3Int>();
        cellsToCheck = new HashSet<Vector3Int>();
    }

    private void Start()
    {
        SetPattern(startPattern);
    }

    private void SetPattern(Pattern pattern)
    {
        Reset();

        Vector2Int center = pattern.GetCenter();

        for (int i = 0; i < pattern.cells.Length; i++)
        {
            Vector3Int cell = (Vector3Int)(pattern.cells[i].position - center);
            frontBuffer.SetTile(cell, pattern.cells[i].tile.Tile);
            aliveCells.Add(cell);
        }

        population = aliveCells.Count;
    }

    private void Reset()
    {
        aliveCells.Clear();
        cellsToCheck.Clear();
        frontBuffer.ClearAllTiles();
        backBuffer.ClearAllTiles();
        population = 0;
        iterations = 0;
        time = 0f;
    }

    private void OnEnable()
    {
        StartCoroutine(Simulate());
    }

    private IEnumerator Simulate()
    {
        var interval = new WaitForSeconds(updateInterval);
        float tickTime = updateInterval;

        while (isActive) {
            if(updateInterval != tickTime) {
                interval = new WaitForSeconds(updateInterval);
                tickTime = updateInterval;
            }

            UpdateState();

            population = aliveCells.Count;
            iterations++;
            time += updateInterval;

            yield return interval;
        }
    }

    private void UpdateState()
    {
        cellsToCheck.Clear();

        // gather cells to check
        foreach (Vector3Int cell in aliveCells) {
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    cellsToCheck.Add(cell + new Vector3Int(x, y));

        }

        // transition cells to the next state
        foreach (Vector3Int cell in cellsToCheck)
        {
            TileDefinition result = TileDefinition.NoResult;
            foreach (var rule in rules) {
                result = rule.Evaluate(this, cell);
                if(!result.isNon)
                    break; 
            }
            ApplyResult(result, cell);
        }

        // swap current state with next state
        Tilemap temp = frontBuffer;
        frontBuffer = backBuffer;
        backBuffer = temp;
        backBuffer.ClearAllTiles();
    }

    public int CountNeighbors(Vector3Int cell)
    {
        int count = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighbor = cell + new Vector3Int(x, y);

                if (x == 0 && y == 0) {
                    continue; //skipped to not count self
                } else if (IsAlive(neighbor)) {
                    count++;
                }
            }
        }

        return count;
    }
    public int CountNeighborsOfType(Vector3Int cell, TileDefinition tile)
    {
        int count = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighbor = cell + new Vector3Int(x, y);

                if (x == 0 && y == 0)
                {
                    continue;
                }
                else if (IsAlive(neighbor) && frontBuffer.GetTile(neighbor) == tile.Tile)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public bool IsAlive(Vector3Int cell)
    {
        return frontBuffer.GetTile(cell) != TileDefinition.Dead.Tile;
    }


    public void ApplyResult(TileDefinition result, Vector3Int cell) {
        if(result.isDead) {
            aliveCells.Remove(cell);
            backBuffer.SetTile(cell, result.Tile);
        } else if(!result.isNon) {
            aliveCells.Add(cell);
            backBuffer.SetTile(cell, result.Tile);
        } else {
            backBuffer.SetTile(cell, frontBuffer.GetTile(cell));
        }
    }

}
