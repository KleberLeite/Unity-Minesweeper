using System.Collections;
using Minesweeper.Gameplay;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Gameplay = Minesweeper.Gameplay;
using Minesweeper.Consts;
using Minesweeper.Common;

public class GridFactoryTests
{
    private class TestInput
    {
        public readonly int Rows;
        public readonly int Collumns;
        public readonly int BombsCount;

        public TestInput(int rows, int collumns, int bombsCount)
        {
            Rows = rows;
            Collumns = collumns;
            BombsCount = bombsCount;
        }
    }

    [TestCase(2, 2, 2, 2, false)]
    [TestCase(3, 3, 2, 2, false)]
    [TestCase(1, 1, 2, 2, false)]
    [TestCase(1, 2, 2, 2, false)]
    [TestCase(2, 3, 2, 2, false)]
    [TestCase(3, 1, 2, 2, false)]
    [TestCase(1, 2, 2, 2, false)]
    [TestCase(4, 4, 2, 2, true)]
    [TestCase(3, 5, 2, 2, true)]
    [TestCase(2, 4, 2, 2, true)]
    [TestCase(2, 0, 2, 2, true)]
    [TestCase(3, 0, 2, 2, true)]
    [TestCase(1, 0, 2, 2, true)]
    [TestCase(1, 4, 2, 2, true)]
    [TestCase(3, 4, 2, 2, true)]
    public void CanSetBomb(int bombPosX, int bombPosY, int clickedPosX, int clickedPosY, bool expected)
    {
        Vector2Int bombPos = new Vector2Int(bombPosX, bombPosY);
        Vector2Int clickedPos = new Vector2Int(clickedPosX, clickedPosY);
        bool inHorizontalSpace = bombPos.x >= clickedPos.x - 1 && bombPos.x <= clickedPos.x + 1;
        bool inVerticalSpace = bombPos.y >= clickedPos.y - 1 && bombPos.y <= clickedPos.y + 1;

        bool result = !(inHorizontalSpace && inVerticalSpace);
        Assert.AreEqual(expected, result);
    }

    // ERRO AQ
    // Easy
    [TestCase(8, 16, 12, 4, 6)]
    [TestCase(8, 16, 12, 3, 4)]
    [TestCase(8, 16, 12, 7, 7)]
    [TestCase(8, 16, 12, 8, 3)]
    [TestCase(8, 16, 12, 1, 8)]
    [TestCase(8, 16, 12, 0, 0)]
    [TestCase(8, 16, 12, 8, 16)]
    // Medium
    [TestCase(10, 18, 16, 4, 6)]
    [TestCase(10, 18, 16, 3, 4)]
    [TestCase(10, 18, 16, 7, 7)]
    [TestCase(10, 18, 16, 8, 3)]
    [TestCase(10, 18, 16, 1, 8)]
    [TestCase(10, 18, 16, 8, 4)]
    [TestCase(10, 18, 16, 2, 9)]
    [TestCase(10, 18, 16, 9, 13)]
    [TestCase(10, 18, 16, 8, 16)]
    // Hard
    [TestCase(12, 20, 20, 4, 6)]
    [TestCase(12, 20, 20, 3, 4)]
    [TestCase(12, 20, 20, 7, 7)]
    [TestCase(12, 20, 20, 8, 3)]
    [TestCase(12, 20, 20, 1, 8)]
    [TestCase(12, 20, 20, 0, 3)]
    [TestCase(12, 20, 20, 10, 4)]
    [TestCase(12, 20, 20, 7, 6)]
    [TestCase(12, 20, 20, 0, 0)]
    [TestCase(12, 20, 20, 12, 20)]
    public void GenerateNewGrid(int rows, int collumns, int bombsCountLevel, int clickedX, int clickedY)
    {
        Vector2Int clickedPos = new Vector2Int(clickedX, clickedY);
        Gameplay.Grid result = GridFactory.CreateNewGrid(rows, collumns, bombsCountLevel, clickedPos);
        int bombsCount = 0;
        for (int x = 0; x < collumns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                int targetCellValue = result.GetCellValue(x, y);
                if (targetCellValue == GameplayConsts.BOMB_CELL_VALUE)
                {
                    bombsCount++;
                    continue;
                }

                if (targetCellValue != 0)
                {
                    int bombsInNeighbour = CountBombsInNeighbour(result, new Vector2Int(x, y));
                    if (bombsInNeighbour != targetCellValue)
                        Assert.AreNotEqual(targetCellValue, bombsInNeighbour, "Cell value is not equal to bombs in neighbour");
                }
            }
        }

        for (int x = clickedX - 1; x <= clickedX + 1 && x < collumns; x++)
        {
            if (x < 0)
                continue;

            for (int y = clickedY - 1; y <= clickedY + 1 && y < rows; y++)
            {
                if (y < 0)
                    continue;

                if (result.GetCellValue(x, y) == GameplayConsts.BOMB_CELL_VALUE)
                    Assert.Fail($"Cell ({x}, {y}) = {result.GetCellValue(x, y)}");
            }
        }

        if (bombsCount != bombsCountLevel)
            Assert.AreNotEqual(bombsCountLevel, bombsCount, "Bombs count are not the same");
    }

    private int CountBombsInNeighbour(Gameplay.Grid grid, Vector2Int pos)
    {
        int bombsCount = 0;
        for (int x = pos.x - 1; x <= pos.x + 1 && x < grid.Collumns; x++)
        {
            if (x < 0)
                continue;

            for (int y = pos.y - 1; y <= pos.y + 1 && y < grid.Rows; y++)
            {
                if (y < 0)
                    continue;

                if (x == pos.x && y == pos.y)
                    continue;

                if (grid.GetCellValue(x, y) == GameplayConsts.BOMB_CELL_VALUE)
                    bombsCount++;
            }
        }

        return bombsCount;
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GridFactoryTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
