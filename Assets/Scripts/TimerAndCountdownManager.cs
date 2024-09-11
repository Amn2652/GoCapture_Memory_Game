using UnityEngine;
using TMPro;

namespace Assets.Script
{
    public class TimerAndCountdownManager : MonoBehaviour
    {
        public static TimerAndCountdownManager Instance;

        [SerializeField] private TMP_Text _timerText, _countdownText;
        [SerializeField] private float _timerMaxValue, _countdownMaxValue;

        private float _timerValue, _countdownValue;
        private bool _countEnd;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            _timerValue = _timerMaxValue;
            _countdownValue = _countdownMaxValue;
            _timerText.text = _timerValue.ToString("0");
        }

        public void StartTimer()
        {
            _timerValue -= Time.deltaTime;
            _timerText.text = _timerValue.ToString("0");
            if (_timerValue <= 0)
            {
                _timerValue = 0;
                GameManager.instance.IsWon = false;
                GameManager.instance.OnGameEnd?.Invoke();
            }
        }

        public void GameLose()
        {
            _timerValue = 0;
        }

        public void DisableCountDown()
        {
            LeanTween.scale(_countdownText.gameObject, Vector2.zero, 0.1f).setDestroyOnComplete(true);
        }

        public void StartCountdown()
        {
            _countdownValue -= Time.deltaTime;
            _countdownText.text = _countdownValue.ToString("0");
            if (!_countEnd && _countdownValue <= 0)
            {
                GameManager.instance.OnGameStart?.Invoke();
                _countEnd = true;
                DisableCountDown();
            }
        }

        public int CalculateScore()
        {
            // Convert remaining time to seconds
            float remainingTime = _timerValue;

            // Ensure the score is at least 0
            return Mathf.Max(0, Mathf.RoundToInt(remainingTime));
        }

    }
}