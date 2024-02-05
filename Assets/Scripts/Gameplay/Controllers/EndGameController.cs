using Minesweeper.Consts;
using Minesweeper.Events;
using Minesweeper.PlayerPrefs;
using Minesweeper.Values;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Minesweeper.Gameplay
{
    public class EndGameController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject pageGO;
        [SerializeField] private TMP_Text currentText;
        [SerializeField] private TMP_Text bestText;
        [SerializeField] private LevelValue currentLevel;
        [SerializeField] private DynamicIntPlayerPref bestTimePP;
        [SerializeField] private BoolEvent onEnd;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private Button playAgainBtn;

        [Header("Dependencies")]
        [SerializeField] private Timer timer;

        private void OnEnable()
        {
            onEnd.OnEvent += OnEnd;
        }

        private void OnDisable()
        {
            onEnd.OnEvent -= OnEnd;
        }

        private void Awake()
        {
            pageGO.SetActive(false);
        }

        private void OnEnd(bool win)
        {
            resultText.text = win ? "Victory" : "Defeat";

            currentText.text = ((int)timer.Current).ToString("000");
            int bestTime = bestTimePP.Get(currentLevel.Level.ID);
            bestText.text = bestTime == GameplayConsts.WITHOUT_RECORD ? "_ _ _" : bestTime.ToString("000");

            playAgainBtn.onClick.AddListener(RestartLevel);

            pageGO.SetActive(true);
        }

        private void RestartLevel()
        {
            SceneManager.LoadScene(GameplayConsts.GAME_SCENE);
        }
    }
}
