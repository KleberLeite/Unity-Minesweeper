using UnityEngine;
using TMPro;

namespace Minesweeper.Gameplay
{
    // Warning:
    // Use TMP_Text temporary while not use images to it.
    public class GridCell : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private TMP_Text valueText;

        public void Init(int value)
        {
            valueText.text = value.ToString();
        }
    }
}
