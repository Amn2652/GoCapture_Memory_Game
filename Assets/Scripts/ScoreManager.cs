using Assets.Script;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int Score { get; set; }
    public int HighScore { get; private set; }
    public string HighScorePlayer { get; private set; } // Store the name of the player with high score

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
            LoadHighScore(); // Load high score and player name
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateScore(int newScore)
    {
        Score = newScore;
        if (Score > HighScore)
        {
            HighScore = Score;
            HighScorePlayer = GameManager.playerName; // Save the player's name
            SaveHighScore(); // Save the new high score and name
        }
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", HighScore);
        PlayerPrefs.SetString("HighScorePlayer", HighScorePlayer); // Save the player's name
        PlayerPrefs.Save();
    }

    private void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        HighScorePlayer = PlayerPrefs.GetString("HighScorePlayer", "No Player");
    }
}
