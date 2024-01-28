using System.Collections.Generic;
using Minesweeper.Common;
using Minesweeper.Databases;
using Minesweeper.Events;
using Minesweeper.Gameplay.Events;
using Minesweeper.PlayerPrefs;
using Minesweeper.Consts;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Minesweeper.Gameplay
{
    public class GameController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GridSpawner gridSpawner;
        [SerializeField] private Database levelDatabase;
        [SerializeField] private IntPlayerPref levelPlayerPref;
        [SerializeField] private float timeToReplayAfterWin;

        [Header("Events")]
        [SerializeField] private GridCellEvent onClickCell;
        [SerializeField] private GridCellEvent onRequestSwitchFlagState;
        [SerializeField] private VoidEvent startGame;
        [SerializeField] private VoidEvent endGame;

        private enum GameState
        {
            Preparing,
            Playing,
            End
        }

        private Grid grid;
        private GridCell[,] cells;

        private bool started;

        private int remainingCellsToOpenCount;

        private void OnEnable()
        {
            onClickCell.OnEvent += OnClickCell;
            onRequestSwitchFlagState.OnEvent += OnRequestSwitchFlagState;
        }

        private void OnDisable()
        {
            onClickCell.OnEvent -= OnClickCell;
            onRequestSwitchFlagState.OnEvent -= OnRequestSwitchFlagState;
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
            Level level = (Level)levelDatabase.GetDataByID(levelPlayerPref.Get());
            grid = GridFactory.CreateNewGrid(level.Rows, level.Collumns, level.BombsCount);
            cells = gridSpawner.SpawnGrid(grid);
            remainingCellsToOpenCount = level.Rows * level.Collumns - level.BombsCount;

            started = false;

            ChangeGameState(GameState.Playing);
        }

        private void OnClickCell(GridCell cell)
        {
            if (!started)
            {
                started = true;
                startGame.Raise();
            }

            if (cell.IsBomb)
            {
                HandleGameOver(cell);
                return;
            }

            if (cell.Value == GameplayConsts.EMPTY_CELL_VALUE)
                OpenAllNeighboursEmpty(cell);
            else
            {
                remainingCellsToOpenCount--;
                cell.Open();
            }

            if (remainingCellsToOpenCount == 0)
                StartCoroutine(HandleGameWinCoroutine());
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
            {
                remainingCellsToOpenCount--;
                cell.Open();
            }
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

        private void OnRequestSwitchFlagState(GridCell cell)
        {
            if (!started)
            {
                started = true;
                startGame.Raise();
            }

            cell.SwitchFlagState();
        }

        private void HandleGameOver(GridCell cell)
        {
            cell.Open();
            for (int i = 0; i < grid.BombsPos.Length; i++)
            {
                if (grid.BombsPos[i] != cell.GridPos)
                    cells[grid.BombsPos[i].x, grid.BombsPos[i].y].Open();
            }

            endGame.Raise();
        }

        private IEnumerator HandleGameWinCoroutine()
        {
            Debug.Log("GameController: Player wins");
            yield return new WaitForSeconds(timeToReplayAfterWin);
            SceneManager.LoadScene(GlobalConsts.GAMEPLAY_SCENE_INDEX);
        }
    }
}
