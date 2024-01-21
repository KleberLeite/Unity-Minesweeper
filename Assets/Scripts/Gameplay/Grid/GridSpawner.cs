using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper.Gameplay
{
    public class GridSpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private GridCell cellPrefab;

        public GridCell[,] SpawnGrid(Grid grid)
        {
            RectTransform gridTransform = gridLayoutGroup.GetComponent<RectTransform>();

            SetupGridLayoutGroup(grid, gridTransform);
            return SpawnCells(grid, gridTransform);
        }

        private void SetupGridLayoutGroup(Grid grid, RectTransform gridTransform)
        {
            float cellSize = GetCellSizeByGridDimensions(grid.Rows, grid.Collumns, gridTransform.rect.width, gridTransform.rect.height);
            gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
            gridLayoutGroup.constraintCount = grid.Collumns;
        }

        private float GetCellSizeByGridDimensions(int rows, int collumns, float gridWidth, float gridHeight)
        {
            float cellSizeByWidth = gridWidth / collumns;
            float cellSizeByHeight = gridHeight / rows;

            return Mathf.Min(cellSizeByWidth, cellSizeByHeight);
        }

        private GridCell[,] SpawnCells(Grid grid, RectTransform gridTransform)
        {
            GridCell[,] cells = new GridCell[grid.Collumns, grid.Rows];

            for (int x = 0; x < grid.Collumns; x++)
            {
                for (int y = 0; y < grid.Rows; y++)
                {
                    GridCell newCell = Instantiate(cellPrefab, gridTransform);
                    newCell.gameObject.name = $"--- GridCell ({x}, {y}) ---";

                    Vector2Int gridPos = new Vector2Int(x, y);
                    int value = grid.GetCellValue(x, y);
                    newCell.Init(gridPos, value);

                    cells[x, y] = newCell;
                }
            }

            return cells;
        }
    }
}
