using UnityEngine;
using Minesweeper.Databases;

namespace Minesweeper.Gameplay
{
    [CreateAssetMenu(menuName = "Databases/Themes/Theme")]
    public class Theme : Data
    {
        [Header("Settings")]
        [SerializeField] private Color[] cellColors;
        [SerializeField] private Color backgroundColor;
        [SerializeField] private Sprite flagSprite;
        [SerializeField] private Sprite bombContentSprite;
        [SerializeField] private Sprite[] othersContentSprites;

        public Color GetBlockColor(Vector2Int gridPos)
        {
            int sum = gridPos.x + gridPos.y;
            int colorIndex = sum % 2 == 0 ? 0 : 1;

            return cellColors[colorIndex];
        }

        public Color GetBackgroundColor() => backgroundColor;

        public Sprite GetFlagSprite() => flagSprite;

        public Sprite GetContentSprite(int value)
        {
            if (value == GameplayConsts.BOMB_CELL_VALUE)
                return bombContentSprite;

            return othersContentSprites[value];
        }
    }
}
