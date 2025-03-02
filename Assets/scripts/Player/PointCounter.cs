using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointCounter : MonoBehaviour
{
    [SerializeField] public TMP_Text text;
    [SerializeField] public GameObject resultMenu;
    public int gathered;
    private List<GameObject> _onBoard;
    void Awake()
    {
        gathered = 0;
        _onBoard = new List<GameObject>();
    }

    private void Update()
    {
        text.text = "score: "+gathered;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_onBoard.Contains(other.gameObject))
        {
            _onBoard.Remove(other.gameObject);
            Destroy(other.gameObject);
            gathered ++;
            if (_onBoard.Count <= 0)
            {
                resultMenu.GetComponent<ResultMenu>().Win(gathered);
            }
        }
    }

    public void Add(GameObject point)
    {
        if (point is not null)
        {
            _onBoard.Add(point);
        }
    }
}
