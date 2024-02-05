using UnityEngine;
using Minesweeper.Common;

namespace Minesweeper.Values
{
    [CreateAssetMenu(menuName = "Values/Level")]
    public class LevelValue : ScriptableObject
    {
        public Level Level { get; set; }
    }
}
