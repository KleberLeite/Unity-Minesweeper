
using UnityEngine;

namespace Minesweeper.Gameplay
{
    [System.Serializable]
    public class Grid
    {
        public int Rows { get; private set; }
        public int Collumns { get; private set; }

        public Vector2Int[] BombsPos { get; private set; }

        private int[,] cellsValue;

        public Grid(int rows, int collumns, int[,] cellsValue, Vector2Int[] bombsPos)
        {
            Rows = rows;
            Collumns = collumns;
            this.cellsValue = cellsValue;
            BombsPos = bombsPos;
        }

        public int GetCellValue(int x, int y) => cellsValue[x, y];
    }
}
