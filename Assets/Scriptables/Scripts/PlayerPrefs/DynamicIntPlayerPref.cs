using System.Text;
using UnityEngine;

namespace Minesweeper.PlayerPrefs
{
    [CreateAssetMenu(menuName = "PlayerPrefs/dynamic int")]
    public class DynamicIntPlayerPref : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private string formatedKey;
        [SerializeField] private int defaultValue;

        public int Get(params int[] ids)
        {
            string key = GetKey(ids);
            return UnityEngine.PlayerPrefs.GetInt(key, defaultValue);
        }

        public void Set(int value, params int[] ids)
        {
            string key = GetKey(ids);
            UnityEngine.PlayerPrefs.SetInt(key, value);
        }

        private string GetKey(params int[] ids)
        {
            StringBuilder key = new StringBuilder();
            int paramIndex = 0;
            for (int i = 0; i < formatedKey.Length; i++)
            {
                if (formatedKey[i] == '{')
                {
                    key.Append(ids[paramIndex]);
                    paramIndex++;
                    i++; // skip }
                }
                else
                    key.Append(formatedKey[i]);
            }

            return key.ToString();
        }
    }
}
