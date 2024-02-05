using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Minesweeper.Gameplay
{
    public class ActionSelectorView : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject openGO;
        [SerializeField] private GameObject blankOpenGO;
        [SerializeField] private GridLayoutGroup gridGroup;
        [SerializeField] private Vector3 offset;

        [Header("Scene Settings")]
        [SerializeField] private RectTransform botRect;
        [SerializeField] private RectTransform cellHighlight;

        [Header("Buttons")]
        [SerializeField] private Button backgroundBtn;
        [SerializeField] private Button openBtn;
        [SerializeField] private Button closeBtn;
        [SerializeField] private Button flagBtn;

        public UnityAction OnPressOpen { get; set; }
        public UnityAction OnPressFlag { get; set; }
        public UnityAction OnPressClose { get; set; }

        private bool alreadyInitialized = false;
        private RectTransform rectTransform;

        private void SetupButtons()
        {
            backgroundBtn.onClick.AddListener(OnCloseBtn);
            closeBtn.onClick.AddListener(OnCloseBtn);
            openBtn.onClick.AddListener(OnOpenBtn);
            flagBtn.onClick.AddListener(OnFlagBtn);
        }

        private void SetupGridGroup()
        {
            gridGroup.cellSize = Vector2.one * (rectTransform.rect.width / 2);
            offset = Vector2.one * (rectTransform.rect.width / 4);
        }

        public void Open(bool useOpenBtn, RectTransform cellTarget)
        {
            if (!alreadyInitialized)
                Initialize(cellTarget);

            GoToTarget(cellTarget.localPosition);

            cellHighlight.gameObject.SetActive(true);
            cellHighlight.localPosition = cellTarget.localPosition;

            openGO.SetActive(useOpenBtn);
            blankOpenGO.SetActive(!useOpenBtn);
            gameObject.SetActive(true);
        }

        private void Initialize(RectTransform cellTarget)
        {
            rectTransform = GetComponent<RectTransform>();
            rectTransform.SetAsLastSibling();

            cellHighlight.sizeDelta = new Vector2(cellTarget.rect.width, cellTarget.rect.height);
            cellHighlight.SetSiblingIndex(rectTransform.GetSiblingIndex() - 1);

            SetupButtons();
            SetupGridGroup();

            alreadyInitialized = true;
        }

        private void GoToTarget(Vector3 targetLocalPos)
        {
            bool outVertical = targetLocalPos.y + this.offset.y > (botRect.rect.height - rectTransform.rect.height) / 2;
            bool outHorizontal = (targetLocalPos.x + this.offset.x) > (botRect.rect.width - rectTransform.rect.width) / 2;

            Vector3 offset = new Vector3(
                this.offset.x * (outHorizontal ? -1 : 1),
                this.offset.y * (outVertical ? -1 : 1)
            );
            transform.localPosition = targetLocalPos + offset;

            int corner = 0;
            if (outVertical)
                corner += 2;
            if (outHorizontal)
                corner += 1;

            gridGroup.startCorner = (GridLayoutGroup.Corner)corner;
        }

        public void Close()
        {
            gameObject.SetActive(false);
            cellHighlight.gameObject.SetActive(false);
        }

        private void OnCloseBtn()
        {
            OnPressClose?.Invoke();
        }

        private void OnOpenBtn()
        {
            OnPressOpen?.Invoke();
        }

        private void OnFlagBtn()
        {
            OnPressFlag?.Invoke();
        }
    }
}
