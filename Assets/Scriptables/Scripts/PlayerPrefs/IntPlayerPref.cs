using UnityEngine;

namespace Minesweeper.PlayerPrefs
{
    [CreateAssetMenu(menuName = "PlayerPrefs/int", fileName = "New Int PlayerPref")]
    public class IntPlayerPref : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private string key;
        [SerializeField] private int defaultValue;

        public int Get() => UnityEngine.PlayerPrefs.GetInt(key, defaultValue);

        public void Set(int value) => UnityEngine.PlayerPrefs.SetInt(key, value);
    }
}
