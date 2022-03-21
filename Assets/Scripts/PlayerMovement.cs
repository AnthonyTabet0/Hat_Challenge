using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("STACKING PARAMETERS")]

    public Transform _container;
    public GameObject[] _ladderPrefab;
    public Vector3 _stackTopPos;
    public float _stackTopPosY;
    public List<Transform> _ladderList;

    [Header("MOVEMENT PARAMETERS")]

    public float _moveSpeed;
    public float _clampingValue;
    float dist;
    float _speedInit;
    bool dragging = false;
    Vector3 v3;
    Vector3 pos;
    Vector3 offset;

    bool _canTurn;
    internal Animator _anim;

    internal AudioSource _source;

    Transform _hatParent;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _anim = transform.GetChild(0).GetComponent<Animator>();
        _hatParent = _container.parent.parent;
    }
    private void Start()
    {
        _speedInit = _moveSpeed;
        _moveSpeed = 0;
        _canTurn = true;
        _ladderList = new List<Transform>();
        Vibration.Init();
    }

    void Update()
    {
        // check if game is runnning
        if (!GameManager.Instance._gameRunning)
            return;

        // move forward
        transform.position += transform.forward * _moveSpeed * Time.deltaTime;

        #region TOUCH INPUT HANDLER
        // check if user tapped on screen
        if (_canTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _anim.SetBool("Walk", true);
                _moveSpeed = _speedInit;
                pos = Input.mousePosition;
                _moveSpeed = _speedInit;
                dist = transform.position.z - Camera.main.transform.position.z;
                v3 = new Vector3(pos.x, 0, dist);
                v3 = Camera.main.ScreenToWorldPoint(v3);
                offset = new Vector3(transform.position.x, transform.position.y, 0) - new Vector3(v3.x, 0, 0);
                dragging = true;
            }
            // check if user holding on screen
            if (dragging && Input.GetMouseButton(0))
            {
                dist = transform.position.z - Camera.main.transform.position.z;
                v3 = new Vector3(Input.mousePosition.x, 0, dist);
                v3 = Camera.main.ScreenToWorldPoint(v3);
                transform.position = (new Vector3(v3.x, 0, transform.position.z) + offset);
            }
            // check if user released finger
            if (dragging && Input.GetMouseButtonUp(0))
            {
                _moveSpeed = 0;
                _anim.SetBool("Walk", false);
                dragging = false;
            }
        }
        #endregion

        // clamp player horizontal movement
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -_clampingValue, _clampingValue), transform.position.y, transform.position.z);

        #region TESTING 
        if (Input.GetKeyDown(KeyCode.E))
            Push();
        if (Input.GetKeyDown(KeyCode.G))
            Pop();
        #endregion
    }

    internal void Push() // Increase hat length
    {
        GameObject toInst = Instantiate(_ladderPrefab[Random.Range(0, _ladderPrefab.Length)], _container); // Add hat part as child of container
        toInst.transform.localPosition = _stackTopPos; // Set position of hat part
        _ladderList.Add(toInst.transform); // Add hat part to list
        _stackTopPosY = _ladderPrefab[0].transform.localScale.y * 2; // Updating hat's new top T pos
        _stackTopPos += new Vector3(0, _stackTopPosY, 0); // Updating value in vector3
        GameManager.Instance._containerValue++; // Increasing hat container value
        GameManager.Instance.ZoomOut();
    }

    internal void Pop() // Decrease hat length
    {
        if (_ladderList.Count <= 0) // check if minimum reached DO NOTHING
            return;
        _stackTopPosY = _ladderList[_ladderList.Count - 1].localPosition.y; // Storing hat's top Y pos
        _stackTopPos = new Vector3(0, _stackTopPosY, 0); // Storing value in vector3
        Destroy(_ladderList[_ladderList.Count - 1].gameObject); // Destroying hat
        _ladderList.RemoveAt(_ladderList.Count - 1);// Removing hat from list
        GameManager.Instance._containerValue--; // Decreasing hat container value
        GameManager.Instance.ZoomIn();
    }

    Coroutine _routineCenter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out var interactable))
        {
            if (interactable._type == Interactable.ObjectType.PICKABLE) // check if interacted with pickable objects
            {
                interactable.Operate();
                _anim.SetTrigger("Hat"); // animating picked up hat
            }
            if (interactable._type == Interactable.ObjectType.CENTERING_OFFSET) // check if reached finish line
            {
                GameManager.Instance.AddCoins(100); // Regular earnings
                GameManager.Instance._uiManager._textScore.text = GameManager.Instance._coins.ToString();
                _canTurn = false; // prevent from swiping
                if (_routineCenter == null)
                    _routineCenter = StartCoroutine(LerpToMultiplier()); // lerping to center
            }
        }
    }

    void MesurmentSection() // Modification due to animation head position
    {
        StartCoroutine(GameManager.Instance._camScript.RotationDelay()); // camera rotation reset
        _hatParent.parent = null; // detaching hat from body in hierarchy
        _hatParent.eulerAngles = new Vector3(0, 0, 0); // resetting rotation
    }

    IEnumerator LerpToMultiplier() // lerp player to standing point of multiplier
    {
        _anim.SetBool("Walk", true);
        GameManager.Instance._gameRunning = false;
        float timer = 0;
        Vector3 pos = transform.position;
        Vector3 direction = new Vector3(GameManager.Instance._podiumFlow._playerTargetPos.position.x, transform.position.y, GameManager.Instance._podiumFlow._playerTargetPos.position.z);
        while (Mathf.Abs(Vector3.Distance(transform.position, direction)) > float.Epsilon)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(pos, direction, timer / 2);
            yield return null;
        }
        _anim.SetBool("Walk", false);
        _routineCenter = null;
        yield return new WaitForSecondsRealtime(.25f);
        MesurmentSection();
    }
}
