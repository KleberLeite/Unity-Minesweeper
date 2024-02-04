using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Minesweeper.Gameplay
{
    public abstract class PlayerController : MonoBehaviour, IPlayerController
    {
        public UnityAction<GridCell> RequestOpen { get; set; }
        public UnityAction<GridCell> RequestSwitchFlagState { get; set; }

        public abstract void OnDown(GridCell cell, PointerEventData eventData);

        public abstract void OnUp(GridCell cell, PointerEventData eventData);
    }
}
