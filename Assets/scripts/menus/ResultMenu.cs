using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ResultMenu : MonoBehaviour
{
    [SerializeField] public TMP_Text resultText;
    [SerializeField] public TMP_Text saveText;
    [SerializeField] public Button saveButton;
    [SerializeField] public TMP_InputField nameField;
    private int _score;
    void Start()
    {
        gameObject.SetActive(false);
        saveText.enabled = false;
        saveButton.enabled = false;
        resultText.SetText("");
    }

    public void Win(int score)
    {
        _score = score;
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        saveText.enabled = true;
        saveButton.enabled = true;
        resultText.SetText("YOU WON");
    }

    public void Lose()
    {
        gameObject.SetActive(true);
        resultText.SetText("GAME OVER");
        Time.timeScale = 0f;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Save()
    {
        var highScores = HighScoreList.Load();
        highScores.AddRecord(nameField.text,_score);
        highScores.SaveHighScores();
        saveText.text = "Saved!";
    }
}
