using Minesweeper.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Minesweeper.Gameplay
{
    public abstract class PlayerController : MonoBehaviour, IPlayerController
    {
        [Header("Events")]
        [SerializeField] private VoidEvent onEndGame;

        public UnityAction<GridCell> RequestOpen { get; set; }
        public UnityAction<GridCell> RequestSwitchFlagState { get; set; }

        protected bool readingInputs = true;

        protected virtual void OnEnable()
        {
            onEndGame.OnEvent += StopReadInputs;
        }

        protected virtual void OnDisable()
        {
            onEndGame.OnEvent -= StopReadInputs;
        }

        protected virtual void StopReadInputs() => readingInputs = false;

        public abstract void OnDown(GridCell cell, PointerEventData eventData);

        public abstract void OnUp(GridCell cell, PointerEventData eventData);
    }
}
