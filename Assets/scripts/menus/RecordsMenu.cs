using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordsMenu : MonoBehaviour
{
    [SerializeField] public GameObject template;
    [SerializeField] public int defaultMax=8;
    private float _recordShift;
    private int _max;
    void Start()
    {
        _recordShift=0;
        template.SetActive(false);
        var scores = HighScoreList.Load().GetHighScores();
        _max = Math.Min(scores.Count, defaultMax);
        for (int i = 0; i < _max; i++)
        {
            var pos = template.GetComponent<RectTransform>().anchoredPosition;
            var record = Instantiate(template, gameObject.transform);
            record.SetActive(true);
            record.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x, pos.y-_recordShift);
            _recordShift += template.GetComponent<RectTransform>().rect.height;
            var labels = record.GetComponentsInChildren<TMP_Text>();
            labels[0].text = i.ToString();
            labels[1].text = scores[i].name;
            labels[2].text = scores[i].score.ToString();
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
