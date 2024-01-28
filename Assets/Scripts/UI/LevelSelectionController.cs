using System.Collections.Generic;
using System.Linq;
using Minesweeper.Common;
using Minesweeper.Databases;
using Minesweeper.PlayerPrefs;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Minesweeper.Consts;

namespace Minesweeper.UI
{
    public class LevelSelectionController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Database levelsDatabase;
        [SerializeField] private IntPlayerPref levelPlayerPref;
        [SerializeField] private TMP_Dropdown levelsDropdown;

        private void Awake()
        {
            Level[] levels = levelsDatabase.GetAll().Cast<Level>().OrderBy(l => l.ID).ToArray();
            SetDropdownOptionsByLevels(levels);

            levelsDropdown.SetValueWithoutNotify(levelPlayerPref.Get());
            levelsDropdown.onValueChanged.AddListener(OnSelectLevel);
        }

        private void SetDropdownOptionsByLevels(Level[] levels)
        {
            levelsDropdown.ClearOptions();
            List<string> levelsName = levels.Select(l => l.LevelName).ToList();
            levelsDropdown.AddOptions(levelsName);
        }

        private void OnSelectLevel(int value)
        {
            levelPlayerPref.Set(value);
            SceneManager.LoadScene(GlobalConsts.GAMEPLAY_SCENE_INDEX);
        }
    }
}
