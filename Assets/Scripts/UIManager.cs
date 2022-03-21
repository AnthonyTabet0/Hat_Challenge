using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject _winPanel;
    public GameObject _startPanel;
    public GameObject _multiplierCanvas;

    public Button _buttonPlay;
    public Button _buttonNextLevel;

    public TextMeshProUGUI _textLevel;
    public TextMeshProUGUI _textScore;

    private void Awake()
    {
        _buttonPlay.onClick.AddListener(ButtonPlayAction); // Listener for play button
        _buttonNextLevel.onClick.AddListener(ButtonNextLevel);
    }

    private void ButtonPlayAction() // To be called when pressed on screen to play
    {
        GameManager.Instance._gameRunning = true;
        _buttonPlay.gameObject.SetActive(false);
    }

    private void ButtonNextLevel()
    {
        GameManager.Instance.LoadScene();
    }

}
