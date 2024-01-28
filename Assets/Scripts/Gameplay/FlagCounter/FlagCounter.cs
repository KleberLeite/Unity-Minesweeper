using Minesweeper.Common;
using Minesweeper.Databases;
using Minesweeper.Gameplay.Events;
using Minesweeper.PlayerPrefs;
using UnityEngine;
using UnityEngine.Events;

namespace Minesweeper.Gameplay
{
    public class FlagCounter : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private GridCellEvent onRequestSwitchFlagState;
        [SerializeField] private IntPlayerPref levelPlayerPref;
        [SerializeField] private Database levelsDatabase;

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

        private void Awake()
        {
            Level level = (Level)levelsDatabase.GetDataByID(levelPlayerPref.Get());
            current = level.BombsCount;
            OnCountChanged.Invoke(current);
        }

        private void OnRequestSwitchFlagState(GridCell cell)
        {
            current += cell.Flagged ? 1 : -1;
            OnCountChanged?.Invoke(current);
        }
    }
}
