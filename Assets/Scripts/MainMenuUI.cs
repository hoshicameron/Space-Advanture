using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playGameButton=null;
    [SerializeField] private Button shipSelectionButton=null;
    [SerializeField] private Button quitButton=null;
    [SerializeField] private Button returnButton = null;
    [SerializeField] private Button forwardButton = null;
    [SerializeField] private Button backwardButton = null;

    [SerializeField] private ShipButton[] shipButtons = null;

    [SerializeField] private GameObject MainMenuUIGameObject=null;
    [SerializeField] private GameObject ShipSelectionUIGameObject= null;

    private int index=0;

    private void Start()
    {
        playGameButton.onClick.AddListener(() => SceneManager.LoadScene("Scene_Game"));
        shipSelectionButton.onClick.AddListener(ShowShipSelectionScreen);
        quitButton.onClick.AddListener(QuitGame);
        returnButton.onClick.AddListener(ReturnToMainMenu);
        forwardButton.onClick.AddListener(() => {ChangeShip(1);});
        backwardButton.onClick.AddListener(() => {ChangeShip(-1);});
    }

    private void ShowShipSelectionScreen()
    {
        MainMenuUIGameObject.SetActive(false);
        ShipSelectionUIGameObject.SetActive(true);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void ReturnToMainMenu()
    {
        MainMenuUIGameObject.SetActive(true);
        ShipSelectionUIGameObject.SetActive(false);
    }

    private void ChangeShip(int value)
    {
        index += value;
        if (index > shipButtons.Length - 1) index = 0;
        else if (index < 0) index = shipButtons.Length - 1;

        for (int i = 0; i < shipButtons.Length; i++)
        {
            if (i == index)
            {
                shipButtons[index].gameObject.SetActive(true);
            } else
            {
                shipButtons[i].gameObject.SetActive(false);
            }


        }
    }

}
