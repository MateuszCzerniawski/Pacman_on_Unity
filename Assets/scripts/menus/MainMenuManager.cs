using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("PLAY");
        SceneManager.LoadScene("BoardMenu");
    }

    public void OpenRecords()
    {
        Debug.Log("RECORDS");
        SceneManager.LoadScene("Records");
    }

    public void Exit()
    {
        Debug.Log("Quiting...");
        Application.Quit();
    }
}
