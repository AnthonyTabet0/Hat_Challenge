using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    private Text _textTimer;

    private float _timer;

    public bool _startTimer;

    private void Awake()
    {
        _textTimer = GetComponent<Text>();
        _startTimer = true;
    }

    private void Update()
    {
        if (_startTimer)
            _timer += Time.deltaTime;
        _textTimer.text = _timer.ToString("#0.00");
        if (Input.GetKeyDown(KeyCode.T)) // Lap
        {
            _timer = 0;
            _startTimer = true;
        }
        if (Input.GetKeyDown(KeyCode.R))// Reset
        {
            _timer = 0;
            _startTimer = false;
        }
    }
}
