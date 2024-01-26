using Minesweeper.Databases;
using UnityEngine;

namespace Minesweeper.Common
{
    [CreateAssetMenu(menuName = "Databases/Levels/Level")]
    public class Level : Data
    {
        [Header("Settings")]
        [SerializeField] private string _name;
        [SerializeField] private Vector2Int gridDimensions;
        [SerializeField] private int bombsCount;
        [SerializeField] private int orderIndex;

        public int OrderIndex => orderIndex;

        public string LevelName => _name;
        public int Collumns => gridDimensions.x;
        public int Rows => gridDimensions.y;
        public int BombsCount => bombsCount;
    }
}
