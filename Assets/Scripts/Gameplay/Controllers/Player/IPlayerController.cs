using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Minesweeper.Gameplay
{
    public interface IPlayerController
    {
        UnityAction<GridCell> RequestOpen { get; set; }
        UnityAction<GridCell> RequestSwitchFlagState { get; set; }

        void OnDown(GridCell cell, PointerEventData eventData);
        void OnUp(GridCell cell, PointerEventData eventData);
    }
}
