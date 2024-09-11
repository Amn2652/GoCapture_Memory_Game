using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Script;

public class GameS : MonoBehaviour
{
    public TextMeshProUGUI highScoreText; // Assign this in the Inspector
    public TMP_InputField playerNameInput; // Input field for player name

    private void Start()
    {
        // Check if ScoreManager exists and update the high score text
        if (ScoreManager.Instance != null)
        {
            highScoreText.text = "High Score: " + ScoreManager.Instance.HighScore.ToString() +
                                 " by " + ScoreManager.Instance.HighScorePlayer;
        }
    }

    public void onPlayButtonClick()
    {
        if (!string.IsNullOrEmpty(playerNameInput.text))
        {
            // Pass the player name to the GameManager and load the next scene
            GameManager.playerName = playerNameInput.text;
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.LogError("Player name cannot be empty!");
        }
    }
}
