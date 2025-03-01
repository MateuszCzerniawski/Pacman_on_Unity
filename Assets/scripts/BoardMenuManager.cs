using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoardMenuManager : MonoBehaviour
{
    [SerializeField] public TMP_InputField widthField;
    [SerializeField] public TMP_InputField heightField;
    [SerializeField] public TMP_Text warningText;
    [SerializeField] public int minHeight=10;
    [SerializeField] public int minWidth=10;
    [SerializeField] public int maxHeight=100;
    [SerializeField] public int maxWidth=100;
    [SerializeField] public float smallScale = 0.25f;
    [SerializeField] public float mediumScale = 0.5f;
    [SerializeField] public float largeScale = 0.75f;

    public void Start()
    {
        warningText.text = "";
    }

    public void Launch()
    {
        int width = GetWidth();
        int height = GetHeight();
        if (CheckInput(width, height))
        {
            SetBoardSize(width, height);
            SceneManager.LoadScene("Game");
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private int GetWidth()
    {
        return int.TryParse(widthField.text, out int width) ? width : -1;
    }
    private int GetHeight()
    {
        return int.TryParse(heightField.text, out int height) ? height : -1;
    }
    

    private void SetBoardSize(int width, int height)
    {
        widthField.text = width.ToString();
        heightField.text = height.ToString();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("width",width);
        PlayerPrefs.SetInt("height",height);
    }

    private bool CheckInput(int width, int height)
    {
        if (width < 0 && height < 0)
        {
            warningText.text = "width and height must be numbers";
            return false;
        }
        if (width < minWidth || height < minHeight || width > maxWidth || height > maxHeight)
        {
            warningText.text = "width must be number between " + minWidth + " and " + maxWidth +
                               "\nheight must number between " + minHeight + " and " + maxHeight;
            widthField.text = width < minWidth? minWidth.ToString():width > maxWidth ? maxWidth.ToString(): width.ToString();
            heightField.text = height < minHeight? minHeight.ToString():height > maxHeight ? maxHeight.ToString(): height.ToString();
            return false;
        }

        return true;
    }
    
    public void SetMinimalSize()
    {
        SetBoardSize(minWidth,minHeight);
    }

    public void SetSmallSize()
    {
        SetBoardSize((int)(smallScale * maxWidth), (int)(smallScale * maxHeight));
    }
    public void SetMediumSize()
    {
        SetBoardSize((int)(mediumScale * maxWidth), (int)(mediumScale * maxHeight));
    }
    public void SetLargeSize()
    {
        SetBoardSize((int)(largeScale * maxWidth), (int)(largeScale * maxHeight));
    }
    public void SetMaximalSize()
    {
        SetBoardSize(maxWidth,maxHeight);
    }
}
