using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private GameObject[] playerShips = null;

    private bool canPlay=false;
    private int score=0;
    private int waveSurvived=0;


    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60;

        canPlay = true;

        int index = PlayerPrefs.GetInt(PlayerPrefsKey.SelectedShip.ToString());

        for (int i = 0; i < playerShips.Length; i++)
        {
            if (i == index)
            {
                Instantiate(playerShips[i], new Vector3(0, -4, 0), Quaternion.identity);
            }
        }

    }

    public void UpdateScore(int value)
    {
        score += value;

        UIManager.Instance.UpdateScoreText(score.ToString());
    }

    public void UpdateSurvivedWave(int value)
    {
        waveSurvived += value;

        UIManager.Instance.UpdateWaveText(waveSurvived.ToString());
    }

    public void GameOver()
    {
        canPlay = false;

        // Save player data
        if (PlayerPrefs.GetInt(PlayerPrefsKey.BestRecord_WaveSurvived.ToString(), 0) < waveSurvived)
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.BestRecord_WaveSurvived.ToString(),waveSurvived);
        }

        if (PlayerPrefs.GetInt(PlayerPrefsKey.BestRecord_ScoreCollected.ToString(), 0) < score)
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.BestRecord_ScoreCollected.ToString(),score);
        }

        UIManager.Instance.ShowGameOverUI(waveSurvived.ToString(),
            PlayerPrefs.GetInt(PlayerPrefsKey.BestRecord_WaveSurvived.ToString()).ToString(), score.ToString(),
            PlayerPrefs.GetInt(PlayerPrefsKey.BestRecord_ScoreCollected.ToString()).ToString());
    }

    public bool GetCanPlay()
    {
        return canPlay;
    }
}


public enum ConditionType
{
    Wave,
    Score,
    None
}

public enum ShipType
{
    Arrow,
    Blade,
    Reaper,
    Hummingbird,
    None
}

public enum PlayerPrefsKey
{
    BestRecord_WaveSurvived,
    BestRecord_ScoreCollected,
    SelectedShip
}