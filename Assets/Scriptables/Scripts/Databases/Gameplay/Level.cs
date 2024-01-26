using Minesweeper.Databases;
using UnityEngine;

namespace Minesweeper.Gameplay
{
    [CreateAssetMenu(menuName = "Databases/Levels/Level")]
    public class Level : Data
    {
        [Header("Settings")]
        [SerializeField] private string _name;
        [SerializeField] private Vector2Int gridDimensions;
        [SerializeField] private int bombsCount;

        public string LevelName => _name;
        public int Collumns => gridDimensions.x;
        public int Rows => gridDimensions.y;
        public int BombsCount => bombsCount;
    }
}
