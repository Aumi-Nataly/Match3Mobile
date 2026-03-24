using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _panelHowPlay;

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void HowPlay()
    {
        _panelHowPlay.SetActive(true);
    }

    public void ReturnHowPlay()
    {
        _panelHowPlay.SetActive(false);
    }
}
