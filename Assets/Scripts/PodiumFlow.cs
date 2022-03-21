using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumFlow : MonoBehaviour
{
    public Transform _indicatorTrans;
    public Transform _playerTargetPos;
    public float _speedMeter;

    private void OnEnable()
    {
        GameManager.Instance._uiManager._multiplierCanvas.SetActive(true);
        GameManager.Instance._camScript.Target = _indicatorTrans;
        GameManager.Instance._camScript.offset = new Vector3(1, 0, -7);
        GameManager.Instance._camScript.SmoothTime = 0.1f;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GameManager.Instance._uiManager._multiplierCanvas.SetActive(false);
        if (Input.GetMouseButton(0))
        {
            if (_indicatorTrans.localPosition.y <= GameManager.Instance._player._container.GetChild(GameManager.Instance._player._container.childCount - 1).position.y)
                _indicatorTrans.position += transform.up * _speedMeter * Time.deltaTime;
            else
            {
                GameManager.Instance.Win();
                enabled = false;
            }
        }
    }
}
