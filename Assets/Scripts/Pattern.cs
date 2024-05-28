using UnityEngine;

[CreateAssetMenu(menuName = "Game of Life/Pattern")]
public class Pattern : ScriptableObject
{
    public Cell[] cells;

    public Vector2Int GetCenter()
    {
        if (cells == null || cells.Length == 0) {
            return Vector2Int.zero;
        }

        Vector2Int min = Vector2Int.zero;
        Vector2Int max = Vector2Int.zero;

        for (int i = 0; i < cells.Length; i++)
        {
            Vector2Int cell = cells[i].position;
            min.x = Mathf.Min(min.x, cell.x);
            min.y = Mathf.Min(min.y, cell.y);
            max.x = Mathf.Max(max.x, cell.x);
            max.y = Mathf.Max(max.y, cell.y);
        }

        return (min + max) / 2;
    }

    [System.Serializable]
    public class Cell {
        public Vector2Int position; 
        public TileDefinition tile;
    }

}
