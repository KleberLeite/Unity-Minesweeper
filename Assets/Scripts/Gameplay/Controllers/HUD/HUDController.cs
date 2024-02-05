using TMPro;
using UnityEngine;

namespace Minesweeper.Gameplay
{
    public class HUDController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Timer timer;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private FlagCounter flagCounter;
        [SerializeField] private TMP_Text flagText;

        private void Awake()
        {
            flagCounter.OnCountChanged += UpdateFlagText;
        }

        private void Update()
        {
            timeText.text = ((int)timer.Current).ToString("000");
        }

        private void UpdateFlagText(int flagsCount) => flagText.text = flagsCount.ToString();
    }
}
