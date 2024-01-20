
namespace Minesweeper.Gameplay
{
    [System.Serializable]
    public class Grid
    {
        public int Rows { get; private set; }
        public int Collumns { get; private set; }

        private int[,] cellsValue;

        public Grid(int rows, int collumns, int[,] cellsValue)
        {
            Rows = rows;
            Collumns = collumns;
            this.cellsValue = cellsValue;
        }

        public int GetCellValue(int x, int y) => cellsValue[x, y];
    }
}
