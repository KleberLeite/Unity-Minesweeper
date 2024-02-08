using System.Collections.Generic;
using System.Linq;
using Minesweeper.Consts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Minesweeper.Gameplay
{
    public class MobilePlayerController : PlayerController
    {
        [Header("Settings")]
        [SerializeField] private ActionSelectorView selectorView;

        private ActionSelector selector;
        private GridCell selectedCell;
        private List<PressData> clicks = new List<PressData>();

        private bool firstClick = true;

        private class PressData
        {
            public int PointerID { get; }
            public float TimeSpan { get; }
            public bool Pressing { get; set; }

            public PressData(int id, float timeSpan)
            {
                PointerID = id;
                TimeSpan = timeSpan;
                Pressing = true;
            }
        }

        protected override void StopReadInputs()
        {
            clicks.Clear();
            CloseSelector();
            base.StopReadInputs();
        }

        private void Awake()
        {
            ConfigureSelector();
        }

        private void ConfigureSelector()
        {
            selector = new ActionSelector(selectorView);
            selector.OnPressCancel += CancelSelection;
            selector.OnPressFlag += OnSelectorPressFlag;
            selector.OnPressOpen += OnSelectorPressOpen;
        }

        private void Update()
        {
            if (!readingInputs)
                return;

            for (int i = 0; i < clicks.Count; i++)
            {
                PressData data = clicks[i];
                if (Time.time - data.TimeSpan >= GameplayConsts.TIME_PRESSING_TO_ADD_FLAG)
                {
                    if (data.Pressing)
                        HandleRequestSwithcFlagState();
                    else
                        HandleJustOpenSelector();

                    return;
                }
            }
        }

        private void HandleRequestSwithcFlagState()
        {
            RequestSwitchFlagState?.Invoke(selectedCell);

            clicks.Clear();
            selectedCell = null;

            CloseSelector();
        }

        private void HandleJustOpenSelector()
        {
            clicks.Clear();
            OpenSelector();
        }

        public override void OnDown(GridCell cell, PointerEventData eventData)
        {
            if (!readingInputs)
                return;

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (firstClick)
            {
                firstClick = false;
                RequestOpen?.Invoke(cell);
                return;
            }

            if (selectedCell == null || cell != selectedCell)
            {
                HandleNewCellSelected(cell, eventData);
                return;
            }

            clicks.Add(new PressData(eventData.pointerId, Time.time));
            if (clicks.Count >= 2)
            {
                HandleDoubleClick();
                return;
            }
        }

        private void HandleNewCellSelected(GridCell cell, PointerEventData eventData)
        {
            CloseSelector();

            // Cells already open aren't considered
            if (cell.Opened)
            {
                clicks.Clear();
                selectedCell = null;
                return;
            }

            clicks = new List<PressData> { new PressData(eventData.pointerId, Time.time) };
            selectedCell = cell;
        }

        private void HandleDoubleClick()
        {
            CloseSelector();

            RequestOpen?.Invoke(selectedCell);

            clicks.Clear();
            selectedCell = null;
        }

        public override void OnUp(GridCell cell, PointerEventData eventData)
        {
            if (!readingInputs)
                return;

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            // Look for the PressData of the released button and disable PressData.Pressing
            PressData target = clicks.FirstOrDefault(pd => pd.PointerID == eventData.pointerId);
            if (target != null)
                target.Pressing = false;
        }

        private void OpenSelector()
        {
            if (!selector.Opened)
                selector.Open(selectedCell);
        }

        private void CloseSelector()
        {
            if (selector.Opened)
                selector.Close();
        }

        private void OnSelectorPressFlag()
        {
            RequestSwitchFlagState?.Invoke(selectedCell);
            CancelSelection();
        }

        private void OnSelectorPressOpen()
        {
            RequestOpen?.Invoke(selectedCell);
            CancelSelection();
        }

        private void CancelSelection()
        {
            clicks.Clear();
            selectedCell = null;

            CloseSelector();
        }
    }
}
