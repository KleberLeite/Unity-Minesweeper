using System.Collections.Generic;
using UnityEngine;
using Minesweeper.Consts;

namespace Minesweeper.Gameplay
{
    public class GridFactory
    {
        private static readonly System.Random rng;

        static GridFactory()
        {
            rng = new System.Random();
        }

        public static Grid CreateNewGrid(int rows, int collumns, int bombsCount, Vector2Int clickedPos)
        {
            int[,] cellsValue = new int[collumns, rows];
            List<Vector2Int> bombsPos = InsertBombs(ref cellsValue, rows, collumns, bombsCount, clickedPos);
            IncrementValueInNeighboursBombCells(ref cellsValue, rows, collumns, bombsPos);

            return new Grid(rows, collumns, cellsValue, bombsPos.ToArray());
        }

        /// <summary>
        /// Insere bombas em posições aleatórias exceto no 3x3 com centro em "clickedPos".
        /// </summary>
        private static List<Vector2Int> InsertBombs(
            ref int[,] cellsValue,
            int rows,
            int collumns,
            int bombsCount,
            Vector2Int clickedPos)
        {
            List<Vector2Int> bombsPos = new List<Vector2Int>();

            for (int i = 0; i < bombsCount; i++)
            {
                int rngX = rng.Next(0, collumns);
                int rngY = rng.Next(0, rows);

                Vector2Int rngPos = new Vector2Int(rngX, rngY);
                if (bombsPos.Contains(rngPos) || !CanSetBomb(rngPos, clickedPos))
                    continue;

                cellsValue[rngX, rngY] = GameplayConsts.BOMB_CELL_VALUE;
                bombsPos.Add(rngPos);
            }

            return bombsPos;
        }

        /// <summary>
        /// Retorna se pode colocar uma bomba na posição, retornando falso caso esteja
        /// no 3x3 com centro em "clickedPos", caso contrário, retorna positivo.
        /// </summary>
        private static bool CanSetBomb(Vector2Int bombPos, Vector2Int clickedPos)
        {
            bool inHorizontalSpace = bombPos.x >= clickedPos.x - 1 && bombPos.x <= clickedPos.x + 1;
            bool inVerticalSpace = bombPos.y >= clickedPos.y - 1 && bombPos.y <= clickedPos.y + 1;

            return !(inHorizontalSpace && inVerticalSpace);
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
