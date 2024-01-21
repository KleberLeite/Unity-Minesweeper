using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper.Gameplay
{
    public class GridFactory
    {
        private static readonly System.Random rng;

        static GridFactory()
        {
            rng = new System.Random();
        }

        public static Grid CreateNewGrid(int rows, int collumns, int bombsCount)
        {
            int[,] cellsValue = new int[collumns, rows];
            List<Vector2Int> bombsPos = InsertBombsInRandomPositions(ref cellsValue, rows, collumns, bombsCount);
            IncrementValueInNeighboursBombCells(ref cellsValue, rows, collumns, bombsPos);

            return new Grid(rows, collumns, cellsValue, bombsPos.ToArray());
        }

        private static List<Vector2Int> InsertBombsInRandomPositions(
            ref int[,] cellsValue,
            int rows,
            int collumns,
            int bombsCount)
        {
            List<Vector2Int> bombsPos = new List<Vector2Int>();

            for (int i = 0; i < bombsCount; i++)
            {
                int rngX = rng.Next(0, collumns);
                int rngY = rng.Next(0, rows);

                Vector2Int rngPos = new Vector2Int(rngX, rngY);
                if (bombsPos.Contains(rngPos))
                    continue;

                cellsValue[rngX, rngY] = GameplayConsts.BOMB_CELL_VALUE;
                bombsPos.Add(rngPos);
            }

            return bombsPos;
        }

        private static void IncrementValueInNeighboursBombCells(
            ref int[,] cellsValue,
            int rows,
            int collumns,
            List<Vector2Int> bombsPos)
        {
            foreach (Vector2Int pos in bombsPos)
            {
                for (int x = pos.x - 1; x <= pos.x + 1 && x < collumns; x++)
                {
                    if (x < 0)
                        continue;

                    for (int y = pos.y - 1; y <= pos.y + 1 && y < rows; y++)
                    {
                        if (y < 0)
                            continue;

                        // Ignore bomb cells.
                        Vector2Int targetPos = new Vector2Int(x, y);
                        if (bombsPos.Contains(targetPos))
                            continue;

                        cellsValue[x, y]++;
                    }
                }
            }
        }
    }
}
