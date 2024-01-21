using UnityEngine;
using UnityEngine.Events;

namespace Minesweeper.Gameplay.Events
{
    [CreateAssetMenu(menuName = "Events/Gameplay/GridCell")]
    public class GridCellEvent : ScriptableObject
    {
        public UnityAction<GridCell> OnEvent;
        public void Raise(GridCell value) => OnEvent?.Invoke(value);
    }
}
