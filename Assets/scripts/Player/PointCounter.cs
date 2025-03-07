using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointCounter : MonoBehaviour
{
    [SerializeField] public GameObject gameUI;
    [SerializeField] public GameObject resultMenu;
    [SerializeField] public float defaultMultiplier = 1f;
    private int _gathered;
    private List<GameObject> _onBoard;
    private float _multiplier = 1f;
    void Awake()
    {
        _multiplier = defaultMultiplier;
        _gathered = 0;
        _onBoard = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollisionWithPoint(other);
    }

    public void HandleCollisionWithPoint(Collider2D other)
    {
        if (_onBoard.Contains(other.gameObject))
        {
            _onBoard.Remove(other.gameObject);
            Destroy(other.gameObject);
            AddToScore(1);
            if (_onBoard.Count <= 0)
            {
                resultMenu.GetComponent<ResultMenu>().Win(_gathered);
            }
        }
    }

    public void AddToTrack(GameObject point)
    {
        if (point is not null)
        {
            _onBoard.Add(point);
        }
    }

    public void AddToScore(int points)
    {
        _gathered += (int)(points * _multiplier);
        gameUI.GetComponent<UIManager>().UpdateScore(_gathered);
    }

    public void SetMultiplier(float multiplier)
    {
        _multiplier = multiplier;
    }
}
