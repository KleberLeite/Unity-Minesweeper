using Minesweeper.Gameplay.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Minesweeper.Gameplay
{
    public class FlagCounter : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private GridCellEvent onRequestSwitchFlagState;

        public UnityAction<int> OnCountChanged;

        private int current;

        private void OnEnable()
        {
            onRequestSwitchFlagState.OnEvent += OnRequestSwitchFlagState;
        }

        private void OnDisable()
        {
            onRequestSwitchFlagState.OnEvent -= OnRequestSwitchFlagState;
        }

        private void OnRequestSwitchFlagState(GridCell cell)
        {
            current += cell.Flagged ? 1 : -1;
            OnCountChanged?.Invoke(current);
        }
    }
}
