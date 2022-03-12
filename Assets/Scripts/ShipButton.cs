using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipButton : MonoBehaviour
{
    [SerializeField] private Button shipBtn=null;

    [SerializeField] public ConditionType conditionType;
    [SerializeField] public int ConditionValue;
    [SerializeField] public ShipType shipType;
    private void OnEnable()
    {
        shipBtn.interactable = false;
        switch (conditionType)
        {
            case ConditionType.Wave:
                if (PlayerPrefs.GetInt(PlayerPrefsKey.BestRecord_WaveSurvived.ToString()) >= ConditionValue)
                    shipBtn.interactable = true;
                break;
            case ConditionType.Score:
                if (PlayerPrefs.GetInt(PlayerPrefsKey.BestRecord_ScoreCollected.ToString()) >= ConditionValue)
                    shipBtn.interactable = true;
                break;
            case ConditionType.None:
                shipBtn.interactable = true;
                break;
        }

        shipBtn.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.SelectedShip.ToString(),(int)shipType);
        });
    }
}


