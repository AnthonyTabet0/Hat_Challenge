using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum ObjectType
    {
        PICKABLE,
        TRAP,
        CENTERING_OFFSET
    }

    public ObjectType _type;

    public int _value;

    internal Collider _collider;

    Animation _animationClip;

    private void Awake()
    {
        _collider = GetComponentInChildren<Collider>();
        _animationClip = GetComponent<Animation>();
        Vibration.Init();
    }

    private void Start()
    {
        if (_type == ObjectType.PICKABLE)
            _value = 1;
    }

    internal void Operate()
    {
        switch (_type)
        {
            case ObjectType.PICKABLE: // check if object is a pickable
                for (int i = 0; i < _value; i++)
                {
                    GameManager.Instance._player.Push();
                    _animationClip.Play();
                }
                Vibration.Vibrate(25);
                break;
            case ObjectType.TRAP: // check if object is a trap
                for (int i = 0; i <= GameManager.Instance._player._ladderList.Count - _value; i++)
                {
                    GameManager.Instance._player.Pop();
                }
                Vibration.Vibrate(40);
                break;
        }
    }
}
