using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper.Gameplay
{
    public class GridSpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private GridCell cellPrefab;

        public GridCell[,] SpawnGrid(int rows, int collumns)
        {
            RectTransform gridTransform = gridLayoutGroup.GetComponent<RectTransform>();

            SetupGridLayoutGroup(rows, collumns, gridTransform);
            return SpawnCells(rows, collumns, gridTransform);
        }

        private void SetupGridLayoutGroup(int rows, int collumns, RectTransform gridTransform)
        {
            float cellSize = GetCellSizeByGridDimensions(rows, collumns, gridTransform.rect.width, gridTransform.rect.height);
            gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
            gridLayoutGroup.constraintCount = collumns;
        }

        private float GetCellSizeByGridDimensions(int rows, int collumns, float gridWidth, float gridHeight)
        {
            float cellSizeByWidth = gridWidth / collumns;
            float cellSizeByHeight = gridHeight / rows;

            return Mathf.Min(cellSizeByWidth, cellSizeByHeight);
        }

        private GridCell[,] SpawnCells(int rows, int collumns, RectTransform gridTransform)
        {
            GridCell[,] cells = new GridCell[collumns, rows];

            for (int x = 0; x < collumns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    GridCell newCell = Instantiate(cellPrefab, gridTransform);
                    newCell.gameObject.name = $"--- GridCell ({x}, {y}) ---";

                    Vector2Int gridPos = new Vector2Int(x, y);
                    newCell.Init(gridPos);

                    cells[x, y] = newCell;
                }
            }

            return cells;
        }
    }
}
