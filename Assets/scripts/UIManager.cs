using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] public GameObject healthTemplate;
    [SerializeField] public GameObject healthNode;
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public float healthSpacing = 10f;
    private float _templateWidth;
    private void Start()
    {
        healthTemplate.SetActive(false);
    }
    

    public void UpdateHealth(int hp)
    {
        foreach (Transform heart in healthNode.transform)
        {
            Destroy(heart.gameObject);
        }
        float shift=0;
        _templateWidth = healthTemplate.GetComponent<RectTransform>().rect.width;
        for (int i = 0; i < hp; i++)
        {
            var heart = Instantiate(healthTemplate, healthNode.transform);
            heart.SetActive(true);
            heart.GetComponent<RectTransform>().anchoredPosition = new Vector2(shift, 0);
            shift += _templateWidth+healthSpacing;
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "score: "+score;
    }
    
}
