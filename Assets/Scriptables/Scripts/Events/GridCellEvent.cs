using Minesweeper.Gameplay;
using UnityEngine;
using UnityEngine.Events;

namespace Minesweeper.Events
{
    public class GridCellEvent : MonoBehaviour
    {
        public UnityAction<GridCell> OnEvent;
        public void Raise(GridCell value) => OnEvent?.Invoke(value);
    }
}
