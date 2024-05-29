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

    [SerializeField] private Tilemap RenderMap;

    private HashSet<Vector2Int> activeCells;
    private HashSet<Vector2Int> cellsToCheck;

    private TileBuffer frontBuffer;
    private TileBuffer backBuffer; 

    public int Population { get; private set; }
    public int Iterations { get; private set; }
    public float Time { get; private set; }

    private void Awake()
    {
        activeCells = new HashSet<Vector2Int>();
        cellsToCheck = new HashSet<Vector2Int>();

        frontBuffer = new();
        backBuffer = new(); 
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
            Vector2Int cell = (pattern.cells[i].position - center);
            frontBuffer.SetTile(cell, pattern.cells[i].tile);
            activeCells.Add(cell);
        }

        Population = activeCells.Count;
    }

    private void Reset()
    {
        activeCells.Clear();
        cellsToCheck.Clear();
        frontBuffer.Clear();
        backBuffer.Clear();
        Population = 0;
        Iterations = 0;
        Time = 0f;
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

            Population = activeCells.Count;
            Iterations++;
            Time += updateInterval;

            yield return interval;
        }
    }

    private void UpdateState()
    {
        cellsToCheck.Clear();

        // gather cells to check
        foreach (Vector2Int cell in activeCells) {
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    cellsToCheck.Add(cell + new Vector2Int(x, y));

        }

        // transition cells to the next state
        foreach (Vector2Int cell in cellsToCheck)
        {
            TileDefinition result = TileDefinition.NoResult;
            foreach (var rule in rules) {
                result = rule.Evaluate(frontBuffer, cell);
                if(!result.isNon)
                    break; 
            }
            ApplyResult(result, cell);
        }

        frontBuffer.DrawToTilemap(RenderMap);

        // swap current state with next state
        (backBuffer, frontBuffer) = (frontBuffer, backBuffer);
        backBuffer.Clear();
    }

    public void ApplyResult(TileDefinition result, Vector2Int cell) {
        if(result.isDead) {
            activeCells.Remove(cell);
            backBuffer.SetTile(cell, result);
        } else if(!result.isNon) {
            activeCells.Add(cell);
            backBuffer.SetTile(cell, result);
        } else {
            backBuffer.SetTile(cell, frontBuffer.GetTile(cell));
        }
    }

}
