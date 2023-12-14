using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private List<string> sceneNames = new List<string>();

    public void QUIT()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneNames[0]);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(sceneNames[1]);
    }
}
