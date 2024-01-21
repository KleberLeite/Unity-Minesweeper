using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Minesweeper.Gameplay.Databases;
using Minesweeper.Gameplay.Events;

namespace Minesweeper.Gameplay
{
    public class GridCell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Settings")]
        [SerializeField] private Image blockImg;
        [SerializeField] private Image contentImg;
        [SerializeField] private Image backgroundImg;
        [SerializeField] private Image flagImg;
        [SerializeField] private GridCellArtDatabase artDatabase;

        [Header("Events")]
        [SerializeField] private GridCellEvent onClick;

        public bool Flagged { get; private set; }
        public bool Opened { get; private set; }
        public Vector2Int GridPos { get; private set; }
        public int Value { get; private set; }
        public bool IsBomb => Value == GameplayConsts.BOMB_CELL_VALUE;

        private bool pressing;
        private float pressedTime;

        public void Init(Vector2Int gridPos, int value)
        {
            GridPos = gridPos;
            Value = value;

            SetupArt(gridPos, value);
        }

        private void SetupArt(Vector2Int gridPos, int value)
        {
            flagImg.sprite = artDatabase.GetFlagSprite();
            blockImg.color = artDatabase.GetBlockColor(gridPos);
            backgroundImg.color = artDatabase.GetBackgroundColor();
            contentImg.sprite = artDatabase.GetContentSprite(value);
        }

        private void Update()
        {
            if (pressing)
                pressedTime += Time.deltaTime;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (pressing)
                return;

            if (eventData.button == PointerEventData.InputButton.Right && !Opened)
            {
                SwitchFlagState();
                return;
            }

            pressedTime = 0;
            pressing = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !pressing)
                return;

            pressing = false;

            if (pressedTime >= GameplayConsts.TIME_PRESSING_TO_ADD_FLAG && !Opened)
                SwitchFlagState();
            else if (!Flagged && !Opened)
                onClick.Raise(this);
        }

        private void SwitchFlagState()
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
