using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Minesweeper.Databases;
using Minesweeper.Consts;
using Minesweeper.PlayerPrefs;
using UnityEngine.Events;

namespace Minesweeper.Gameplay
{
    public class GridCell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Settings")]
        [SerializeField] private Image blockImg;
        [SerializeField] private Image contentImg;
        [SerializeField] private Image backgroundImg;
        [SerializeField] private Image flagImg;
        [SerializeField] private Database artDatabase;
        [SerializeField] private IntPlayerPref artIDPlayerPref;

        public bool Flagged { get; private set; }
        public bool Opened { get; private set; }
        public Vector2Int GridPos { get; private set; }
        public int Value { get; private set; }
        public bool IsBomb => Value == GameplayConsts.BOMB_CELL_VALUE;
        public bool IsEmpty => Value == GameplayConsts.EMPTY_CELL_VALUE;
        public UnityAction<GridCell, PointerEventData> OnDown { get; set; }
        public UnityAction<GridCell, PointerEventData> OnUp { get; set; }

        public RectTransform RectTransform { get; private set; }

        private Transform dropHolder;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        public void Init(Vector2Int gridPos, Transform dropHolder)
        {
            GridPos = gridPos;
            this.dropHolder = dropHolder;

            SetupArt(gridPos);
        }

        public void SetValue(int value)
        {
            Value = value;

            SetContentSprite(value);
        }

        private void SetupArt(Vector2Int gridPos)
        {
            Theme theme = (Theme)artDatabase.GetDataByID(artIDPlayerPref.Get());
            flagImg.sprite = theme.GetFlagSprite();
            blockImg.color = theme.GetBlockColor(gridPos);
            backgroundImg.color = theme.GetBackgroundColor();
        }

        private void SetContentSprite(int value)
        {
            Theme theme = (Theme)artDatabase.GetDataByID(artIDPlayerPref.Get());
            contentImg.sprite = theme.GetContentSprite(value);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Middle)
                return;

            OnDown?.Invoke(this, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Middle)
                return;

            OnUp?.Invoke(this, eventData);
        }

        public void SwitchFlagState()
        {
            Flagged = !Flagged;
            flagImg.gameObject.SetActive(Flagged);
        }

        public void Open()
        {
            blockImg.gameObject.SetActive(false);
            Opened = true;
        }

        public void Show()
        {
            DropEffect drop = blockImg.gameObject.AddComponent<DropEffect>();
            bool goToRight = Random.Range(0, 2) == 0;
            drop.Init(dropHolder, goToRight);
            if (Flagged)
            {
                drop = flagImg.gameObject.AddComponent<DropEffect>();
                drop.Init(dropHolder, !goToRight);
            }
        }
    }
}
