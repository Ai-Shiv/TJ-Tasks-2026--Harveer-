using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text healthText;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;

    [Header("Player")]
    [SerializeField] private PlayerHealth playerHealth;

    private bool gameOverShown;

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance == null ||
            playerHealth == null)
        {
            return;
        }

        UpdateHUD();

        if (GameManager.Instance.IsGameOver &&
            !gameOverShown)
        {
            ShowGameOver();
        }
    }

    private void UpdateHUD()
    {
        scoreText.text =
            $"SCORE  {GameManager.Instance.Score}";

        healthText.text =
            $"HP  {playerHealth.GetCurrentHealth()}/{playerHealth.GetMaxHealth()}";
    }

    private void ShowGameOver()
    {
        gameOverShown = true;

        gameOverPanel.SetActive(true);

        finalScoreText.text =
            $"SCORE  {GameManager.Instance.Score}";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}