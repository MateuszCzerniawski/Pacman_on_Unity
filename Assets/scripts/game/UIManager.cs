using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UIManager : MonoBehaviour
{
    [SerializeField] public GameObject healthTemplate;
    [SerializeField] public GameObject healthNode;
    [SerializeField] public GameObject buffTemplate;
    [SerializeField] public GameObject buffNode;
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public float healthSpacing = 10f;
    private float _healthTemplateWidth;
    private float _buffTemplateWidth;
    private void Start()
    {
        healthTemplate.SetActive(false);
        buffTemplate.SetActive(false);
    }
    

    public void UpdateHealth(int hp)
    {
        foreach (Transform heart in healthNode.transform)
        {
            Destroy(heart.gameObject);
        }
        float shift=0;
        _healthTemplateWidth = healthTemplate.GetComponent<RectTransform>().rect.width;
        for (int i = 0; i < hp; i++)
        {
            var heart = Instantiate(healthTemplate, healthNode.transform);
            heart.SetActive(true);
            heart.GetComponent<RectTransform>().anchoredPosition = new Vector2(shift, 0);
            shift += _healthTemplateWidth+healthSpacing;
        }
    }

    public void UpdateBuffs(Dictionary<Sprite,char> buffs)
    {
        foreach (Transform buff in buffNode.transform)
        {
            Destroy(buff.gameObject);
        }
        float shift = 0;
        _buffTemplateWidth = buffTemplate.GetComponent<RectTransform>().rect.width;
        foreach (var pair in buffs)
        {
            Sprite sprite = pair.Key;
            char key = pair.Value;
            var buff = Instantiate(buffTemplate, buffNode.transform);
            buff.SetActive(true);
            buff.GetComponent<RectTransform>().anchoredPosition = new Vector2(shift, 0);
            buffTemplate.GetComponentInChildren<Image>().sprite = sprite;
            buff.GetComponentInChildren<TMP_Text>().text = key.ToString();
            shift += _buffTemplateWidth;
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "score: "+score;
    }
    
}
