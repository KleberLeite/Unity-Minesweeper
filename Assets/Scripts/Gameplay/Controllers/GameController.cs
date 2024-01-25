using System.Collections.Generic;
using Minesweeper.Events;
using Minesweeper.Gameplay.Events;
using UnityEngine;

namespace Minesweeper.Gameplay
{
    public class GameController : MonoBehaviour
    {
        // Warning: temporary while not implement Level system
        [Header("DEV Settings")]
        [SerializeField] private Vector2Int gridDimensions;
        [SerializeField] private int bombsCount;

        [Header("Settings")]
        [SerializeField] private GridSpawner gridSpawner;

        [Header("Events")]
        [SerializeField] private GridCellEvent onClickCell;
        [SerializeField] private VoidEvent startGame;

        private enum GameState
        {
            Preparing,
            Playing,
            End
        }

        private Grid grid;
        private GridCell[,] cells;

        private bool firstClick;

        private void OnEnable()
        {
            onClickCell.OnEvent += OnClickCell;
        }

        private void OnDisable()
        {
            onClickCell.OnEvent -= OnClickCell;
        }

        private void Start()
        {
            ChangeGameState(GameState.Preparing);
        }

        private void ChangeGameState(GameState newState)
        {
            switch (newState)
            {
                case GameState.Preparing:
                    PrepareGame();
                    break;
            }
        }

        private void PrepareGame()
        {
            grid = GridFactory.CreateNewGrid(gridDimensions.x, gridDimensions.y, bombsCount);
            cells = gridSpawner.SpawnGrid(grid);

            firstClick = true;

            ChangeGameState(GameState.Playing);
        }

        private void OnClickCell(GridCell cell)
        {
            startGame.Raise();

            if (cell.IsBomb)
            {
                HandleGameOver(cell);
                return;
            }

            if (firstClick && cell.Value == GameplayConsts.EMPTY_CELL_VALUE)
                OpenAllNeighboursEmpty(cell);
            else
            {
                cell.Open();
                firstClick = false;
            }
        }

        private void OpenAllNeighboursEmpty(GridCell firstCell)
        {
            Queue<GridCell> cellsToAnalyse = new Queue<GridCell>();
            List<GridCell> analysedCells = new List<GridCell>();
            cellsToAnalyse.Enqueue(firstCell);

            List<GridCell> cellsToOpen = new List<GridCell>();
            while (cellsToAnalyse.Count > 0)
            {
                GridCell cell = cellsToAnalyse.Dequeue();
                if (analysedCells.Contains(cell))
                    continue;

                analysedCells.Add(cell);

                if (cell.Value != GameplayConsts.BOMB_CELL_VALUE)
                    cellsToOpen.Add(cell);

                if (cell.Value == GameplayConsts.EMPTY_CELL_VALUE)
                    GetNeighbours(cell.GridPos).ForEach(cellsToAnalyse.Enqueue);
            }

            foreach (GridCell cell in cellsToOpen)
                cell.Open();

            firstClick = false;
        }

        private List<GridCell> GetNeighbours(Vector2Int targetPos)
        {
            List<GridCell> neighbours = new List<GridCell>();
            for (int x = targetPos.x - 1; x < grid.Collumns; x++)
            {
                if (x < 0)
                    continue;

                for (int y = targetPos.y - 1; y < grid.Rows; y++)
                {
                    if (y < 0)
                        continue;

                    // Ignore target pos
                    if (x == targetPos.x && y == targetPos.y)
                        continue;

                    neighbours.Add(cells[x, y]);
                }
            }

            return neighbours;
        }

        private void HandleGameOver(GridCell cell)
        {
            cell.Open();
            for (int i = 0; i < grid.BombsPos.Length; i++)
            {
                if (grid.BombsPos[i] != cell.GridPos)
                    cells[grid.BombsPos[i].x, grid.BombsPos[i].y].Open();
            }
        }
    }
}
