using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private bool canPlay=false;
    private int score=0;
    private int waveSurvived=0;

    protected override void Awake()
    {
        base.Awake();

        canPlay = true;

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

        // todo add player record to PlayerPreference

        UIManager.Instance.ShowGameOverUI();
    }

    public bool GetCanPlay()
    {
        return canPlay;
    }
}
