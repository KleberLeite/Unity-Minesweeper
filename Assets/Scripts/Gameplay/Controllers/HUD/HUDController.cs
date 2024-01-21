using TMPro;
using UnityEngine;

namespace Minesweeper.Gameplay
{
    public class HUDController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Timer timer;
        [SerializeField] private TMP_Text timeText;

        private void Update()
        {
            timeText.text = timer.Current.ToString("000");
        }
    }
}
