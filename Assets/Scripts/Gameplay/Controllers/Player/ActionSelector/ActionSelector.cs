using UnityEngine;
using UnityEngine.Events;

namespace Minesweeper.Gameplay
{
    public class ActionSelector
    {
        private ActionSelectorView view;

        public UnityAction OnPressOpen;
        public UnityAction OnPressFlag;
        public UnityAction OnPressCancel;

        public bool Opened { get; private set; }

        private GridCell target;

        public ActionSelector(ActionSelectorView view)
        {
            this.view = view;

            view.OnPressClose += () => OnPressCancel?.Invoke();
            view.OnPressFlag += () => OnPressFlag?.Invoke();
            view.OnPressOpen += () => OnPressOpen?.Invoke();

            view.gameObject.SetActive(false);
        }

        public void Open(GridCell target)
        {
            if (target.Opened)
                return;

            view.Open(useOpenBtn: !target.Flagged, target.RectTransform);

            this.target = target;
            Opened = true;
        }

        public void Close()
        {
            view.Close();

            target = null;
            Opened = false;
        }
    }
}
