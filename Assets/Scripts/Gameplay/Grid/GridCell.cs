using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Minesweeper.Databases;
using Minesweeper.Gameplay.Events;
using Minesweeper.Consts;
using Minesweeper.PlayerPrefs;

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

        [Header("Events")]
        [SerializeField] private GridCellEvent onClick;
        [SerializeField] private GridCellEvent onRequestSwitchFlagState;

        public bool Flagged { get; private set; }
        public bool Opened { get; private set; }
        public Vector2Int GridPos { get; private set; }
        public int Value { get; private set; }
        public bool IsBomb => Value == GameplayConsts.BOMB_CELL_VALUE;
        public bool IsEmpty => Value == GameplayConsts.EMPTY_CELL_VALUE;

        private bool pressing;
        private float pressedTime;
        private bool ignoreLeftUp;

        public void Init(Vector2Int gridPos)
        {
            GridPos = gridPos;

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

        private void Update()
        {
            if (!pressing || ignoreLeftUp)
                return;

            pressedTime += Time.deltaTime;
            if (pressedTime >= GameplayConsts.TIME_PRESSING_TO_ADD_FLAG)
            {
                ignoreLeftUp = true;
                onRequestSwitchFlagState.Raise(this);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Opened || pressing)
                return;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                onRequestSwitchFlagState.Raise(this);
                return;
            }

            pressedTime = 0;
            pressing = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            pressing = false;
            if (ignoreLeftUp)
            {
                ignoreLeftUp = false;
                return;
            }

            onClick.Raise(this);
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
    }
}
