using System.Collections;
using Minesweeper.Gameplay;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Gameplay = Minesweeper.Gameplay;

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

    // A Test behaves as an ordinary method
    [Test]
    public void GenerateNewGrid()
    {
        TestInput[] inputs = new TestInput[]
        {
            new TestInput(10, 10, 3),
            new TestInput(15, 15, 5),
            new TestInput(15, 15, 7),
            new TestInput(7, 7, 5),
            new TestInput(10, 10, 10),
        };
        foreach (TestInput input in inputs)
        {
            Gameplay.Grid result = GridFactory.CreateNewGrid(input.Rows, input.Collumns, input.BombsCount);

            int bombsCount = 0;
            for (int x = 0; x < input.Collumns; x++)
            {
                for (int y = 0; y < input.Rows; y++)
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

            if (bombsCount == input.BombsCount)
                Assert.AreEqual(input.BombsCount, bombsCount, "Bombs count are the same");
            else
                Assert.AreNotEqual(input.BombsCount, bombsCount, "Bombs count are not the same");
        }
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
