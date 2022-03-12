using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [Header("InGameUI")]
    [SerializeField] private TextMeshProUGUI scoreText=null;
    [SerializeField] private TextMeshProUGUI waveText=null;
    [SerializeField] private Slider healthSlider=null;
    [SerializeField] private Button pauseButton = null;
    [Header("PauseMenu")]
    [SerializeField] private GameObject pauseMenu=null;
    [SerializeField] private Button pauseMenu_MainMenuButton=null;
    [SerializeField] private Button pauseMenu_restartButton=null;
    [SerializeField] private Button pauseMenu_resumeButton=null;
    [Header("GameOverMenu")]
    [SerializeField] private GameObject gameOverMenu=null;
    [SerializeField] private Button gameOver_MainMenuButton=null;
    [SerializeField] private Button gameOver_RestartButton=null;
    [SerializeField] private TextMeshProUGUI currentWaveText=null;
    [SerializeField] private TextMeshProUGUI BestWaveText=null;
    [SerializeField] private TextMeshProUGUI currentScoreText=null;
    [SerializeField] private TextMeshProUGUI BestScoreText=null;

    protected override void Awake()
    {
        base.Awake();

        pauseButton.onClick.AddListener(ShowPauseUI);

        pauseMenu_MainMenuButton.onClick.AddListener(LoadMainMenu);
        pauseMenu_restartButton.onClick.AddListener(RestartLevel);
        pauseMenu_resumeButton.onClick.AddListener(HidePauseUI);

        gameOver_RestartButton.onClick.AddListener(RestartLevel);
        gameOver_MainMenuButton.onClick.AddListener(LoadMainMenu);


    }

    private void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("Scene_MainMenu");
    }

    public void UpdateScoreText(string scoreText)
    {
        this.scoreText.SetText($"Score: {scoreText}");
    }

    public void UpdateWaveText(string waveText)
    {
        this.waveText.SetText($"Waves: {waveText}");
    }
    public void ShowPauseUI()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void HidePauseUI()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void ShowGameOverUI(string currentWave,string bestWave,string currentScore, string bestScore)
    {
        gameOverMenu.SetActive(true);

        currentWaveText.SetText(currentWave);
        BestWaveText.SetText(bestWave);

        currentScoreText.SetText(currentScore);
        BestScoreText.SetText(bestScore);
    }

    public void UpdateHealthSlider(int value)
    {
        healthSlider.value = value;
    }

    public void SetHealthSliderMaxValue(int Value)
    {
        healthSlider.maxValue = Value;
    }
}
