using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    internal UIManager _uiManager;

    internal PlayerMovement _player; // Player reference

    internal PodiumFlow _podiumFlow; // game flow after crossing finish line

    internal int _containerValue; // Hat container value
    internal int _coins;
    internal int _levelIndex;
    internal bool _gameRunning; // To check if in game

    public CameraFollow _camScript;

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        #region Initializing
        _uiManager = GetComponent<UIManager>();
        _player = FindObjectOfType<PlayerMovement>();
        _podiumFlow = FindObjectOfType<PodiumFlow>();
        #endregion
        #region Fetching data
        _levelIndex = PlayerPrefs.GetInt("level");
        _coins = PlayerPrefs.GetInt("coins");
        #endregion
        if (PlayerPrefs.GetInt("level")==0)
        {
            _levelIndex = 1;
        }
        #region Assigning UI values
        _uiManager._textLevel.text = "Level " + _levelIndex;
        _uiManager._textScore.text = _coins.ToString();
        #endregion
    }

    internal void ZoomOut() // zoomm out when picking up hats off screen
    {
        if (_containerValue < 10)
            return;
        else if (_containerValue == 10)
            _camScript.offset += new Vector3(0, 1, -.5f);
        else
            _camScript.offset += new Vector3(0, .3f, -.3f);
    }
    internal void ZoomIn() // zooming in when losing hats
    {
        if (_containerValue >= 10)
            _camScript.offset -= new Vector3(0, .3f, -.3f);
        else if (_containerValue == 9)
            _camScript.offset -= new Vector3(0, 1, -.5f);
        else
            return;
    }

    internal void AddCoins(int amount) // adding coins
    {
        _coins = PlayerPrefs.GetInt("coins");
        _coins += amount;
        PlayerPrefs.SetInt("coins",_coins);
        _uiManager._textScore.text = _coins.ToString();
    }

    internal void Win()
    {
        _uiManager._winPanel.SetActive(true);
        _levelIndex++;
        PlayerPrefs.SetInt("level",_levelIndex);
        AddCoins(_containerValue); // Bonus earnings
    }

    internal void LoadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
