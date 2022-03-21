using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCondition : MonoBehaviour
{
    public enum ActionType
    {
        STILL,
        MOVING
    }

    public enum MoveType
    {
        HORIZONTAL,
        VERTICAL,
        BOTH
    }

    public ActionType _typeAction;
    public MoveType _typeMove;

    public LayerMask _layer;

    public Transform _transProjector;

    Interactable _interactable;

    public float _speed;
    public float _clampRange;

    float _speedX;
    float _speedZ;

    RaycastHit _hit;


    private void Start()
    {
        //if (Random.Range(0, 2) == 1)
        //{
        //    _speed = -_speed;
        //}
        _interactable = GetComponent<Interactable>();
        _speedX = _speedZ = _speed;
    }

    private void Update()
    {
        if (GameManager.Instance._player.transform.position.z - transform.position.z > 3)
            gameObject.SetActive(false);
        switch (_typeAction)
        {
            case ActionType.MOVING:
                switch (_typeMove)
                {
                    case MoveType.HORIZONTAL:
                        transform.position += transform.right * _speed * Time.deltaTime;
                        if (Mathf.Abs(transform.position.x) >= _clampRange)
                            _speed = -_speed;
                        break;
                    case MoveType.VERTICAL:
                        transform.position += transform.up * _speed * Time.deltaTime;
                        if (transform.position.y >= _clampRange || transform.position.y <= 0)
                            _speed = -_speed;
                        break;
                    case MoveType.BOTH:
                        transform.position += transform.right * _speedX * Time.deltaTime;
                        if (Mathf.Abs(transform.position.x) >= _clampRange)
                            _speedX = -_speedX;
                        transform.position += transform.up * _speedZ * Time.deltaTime;
                        if (transform.position.y >= _clampRange || transform.position.y <= 0)
                            _speedZ = -_speedZ;
                        break;
                }
                break;
        }

        if (Physics.Raycast(_interactable._collider.transform.position, -transform.up, out _hit, 100, _layer))
        {
            if (_hit.transform == null)
                return;
            _transProjector.position = _hit.point;
        }
    }
}
