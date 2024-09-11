using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

namespace Assets.Script
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public static string playerName; // Store player's name

        private bool _isGameStarted;
        public Action OnGameEnd { get; set; }
        public Action OnGameStart { get; set; }

        public bool IsWon { get; set; }

        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private TMP_Text _gameOverText;
        [SerializeField] private TMP_Text _scoreText;
        //[SerializeField] private TMP_Text _highScoreText; // Text for high score display

        private void Awake()
        {
            if (instance == null)
                instance = this;

            OnGameStart += () => _isGameStarted = true;
            OnGameEnd += ShowGameEndScreen;
            OnGameEnd += () => _isGameStarted = false;
        }

        private void ShowGameEndScreen()
        {
            if (IsWon)
            {
                _winPanel.SetActive(true);
                _gameOverScreen.SetActive(false);
                int score = TimerAndCountdownManager.Instance.CalculateScore();
                ScoreManager.Instance.UpdateScore(score);
                _scoreText.text = score.ToString();

                // Update the high score text with player name and score
                //_highScoreText.text = "High Score: " + ScoreManager.Instance.HighScore + " by " + playerName;
            }
            else
            {
                _gameOverScreen.SetActive(true);
                _winPanel.SetActive(false);
                _gameOverText.text = "Better Luck\nNext Time!!";
                _scoreText.text = "";
            }

            LeanTween.scale(_gameOverScreen, Vector2.zero, 1f).setDelay(5).setOnComplete(() =>
            {
                SceneLoad(0);
            });
        }

        private void Update()
        {
            if (_isGameStarted)
            {
                TimerAndCountdownManager.Instance.StartTimer();
            }
            else
            {
                TimerAndCountdownManager.Instance.StartCountdown();
            }
        }

        public void SceneLoad(int buildindex)
        {
            SceneManager.LoadScene(buildindex);
        }
    }
}
