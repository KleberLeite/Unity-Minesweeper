using Minesweeper.Events;
using UnityEngine;

namespace Minesweeper.Gameplay
{
    public class Timer : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private VoidEvent onGameStart;

        public float Current { get; private set; }

        private bool started;

        private void OnEnable()
        {
            onGameStart.OnEvent += OnGameStart;
        }

        private void OnDisable()
        {
            onGameStart.OnEvent -= OnGameStart;
        }

        private void Update()
        {
            if (started)
                Current += Time.deltaTime;
        }

        private void OnGameStart()
        {
            started = true;
        }
    }
}
