using UnityEngine;
using UnityEngine.Events;

namespace Minesweeper.Gameplay.Events
{
    public class GridCellEvent : MonoBehaviour
    {
        public UnityAction<GridCell> OnEvent;
        public void Raise(GridCell value) => OnEvent?.Invoke(value);
    }
}
