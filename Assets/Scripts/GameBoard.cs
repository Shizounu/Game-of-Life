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

    [Header("Tile References")]
    [SerializeField] private Tile aliveTile;
    [SerializeField] private Tile deadTile;


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

    [ContextMenu("Restart")]
    private void SetBasePattern() => SetPattern(startPattern);

    private void SetPattern(Pattern pattern)
    {
        Reset();

        Vector2Int center = pattern.GetCenter();

        for (int i = 0; i < pattern.cells.Length; i++)
        {
            Vector3Int cell = (Vector3Int)(pattern.cells[i] - center);
            frontBuffer.SetTile(cell, aliveTile);
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
            RuleResult result = RuleResult.NoResult;
            foreach (var rule in rules) {
                result = rule.Evaluate(this, cell);

                if(result != RuleResult.NoResult)
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
                    continue;
                } else if (IsAlive(neighbor)) {
                    count++;
                }
            }
        }

        return count;
    }

    public bool IsAlive(Vector3Int cell)
    {
        return frontBuffer.GetTile(cell) == aliveTile;
    }

    public void ApplyResult(RuleResult result, Vector3Int cell) {
        switch (result)
        {
            case RuleResult.Dead:
                backBuffer.SetTile(cell, deadTile);
                aliveCells.Remove(cell);
                break;

            case RuleResult.Tile1:
            case RuleResult.Tile2:
            case RuleResult.Tile3:
                backBuffer.SetTile(cell, aliveTile);
                aliveCells.Add(cell);
                break; 

            case RuleResult.NoResult:
                backBuffer.SetTile(cell, frontBuffer.GetTile(cell));
                break;

        }
    }

}
