using Minesweeper.Events;
using UnityEngine;

namespace Minesweeper.Gameplay
{
    public class Timer : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private VoidEvent onGameStart;
        [SerializeField] private VoidEvent onGameEnd;

        public float Current { get; private set; }

        private bool started;
        private bool stop;

        private void OnEnable()
        {
            onGameStart.OnEvent += OnGameStart;
            onGameEnd.OnEvent += OnGameEnd;
        }

        private void OnDisable()
        {
            onGameStart.OnEvent -= OnGameStart;
            onGameEnd.OnEvent -= OnGameEnd;
        }

        private void Update()
        {
            if (started && !stop)
                Current += Time.deltaTime;
        }

        private void OnGameStart()
        {
            started = true;
        }

        private void OnGameEnd()
        {
            stop = true;
        }
    }
}
