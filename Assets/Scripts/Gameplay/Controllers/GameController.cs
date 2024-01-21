using System.Collections.Generic;
using System.Linq;
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

            startGame.Raise();
            ChangeGameState(GameState.Playing);
        }

        private void OnClickCell(GridCell cell)
        {
            if (cell.IsBomb)
            {
                HandleGameOver();
                return;
            }

            if (firstClick && cell.Value == GameplayConsts.EMPTY_CELL_VALUE)
                OpenAllNeighboursEmpty(cell);
            else
                cell.Open();
        }

        private void OpenAllNeighboursEmpty(GridCell firstCell)
        {
            Queue<GridCell> cellsToAnalyse = new Queue<GridCell>();
            List<GridCell> analysedCells = new List<GridCell>();
            cellsToAnalyse.Enqueue(firstCell);

            List<GridCell> cellsToOpen = new List<GridCell>();
            int i = 0;
            while (cellsToAnalyse.Count > 0)
            {
                GridCell cell = cellsToAnalyse.Dequeue();
                if (analysedCells.Contains(cell))
                    continue;

                analysedCells.Add(cell);
                if (cell.Value == GameplayConsts.EMPTY_CELL_VALUE)
                {
                    cellsToOpen.Add(cell);
                    GetNeighbours(cell.GridPos).ForEach(cellsToAnalyse.Enqueue);
                }

                i++;
                if (i > 900)
                {
                    Debug.Log("GameController: ForceToBreak");
                    break;
                }
            }

            foreach (GridCell cell in cellsToOpen)
                cell.Open();

            firstClick = false;
        }

        private List<GridCell> GetNeighbours(Vector2Int targetPos)
        {
            List<GridCell> neighbours = new List<GridCell>();
            if (targetPos.x + 1 < grid.Collumns)
                neighbours.Add(cells[targetPos.x + 1, targetPos.y]);

            if (targetPos.y + 1 < grid.Rows)
                neighbours.Add(cells[targetPos.x, targetPos.y + 1]);

            if (targetPos.x - 1 >= 0)
                neighbours.Add(cells[targetPos.x - 1, targetPos.y]);

            if (targetPos.y - 1 >= 0)
                neighbours.Add(cells[targetPos.x, targetPos.y - 1]);

            return neighbours;
        }

        private void HandleGameOver()
        {
            Debug.Log("GameController: GameOver");

        }
    }
}
