using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // camera will follow this object
    public Transform Target;
    //camera transform
    public Transform camTransform;
    // change this value to get desired smoothness
    public float SmoothTime = 0.3f;
    // This value will change at the runtime depending on target movement. Initialize with zero vector.
    private Vector3 velocity = Vector3.zero;
    // offset between camera and target
    public Vector3 offset;

    public float _minX, _maxX;

    private void Update()
    {
            if (Target != null)
                transform.position = Vector3.SmoothDamp(transform.position, Target.position + offset, ref velocity, SmoothTime);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, _minX, _maxX), transform.position.y, transform.position.z);
    }

    float timer = 0;

    internal IEnumerator RotationDelay()
    {
        offset = new Vector3(.17f, 4, -5);
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            Camera.main.transform.eulerAngles = new Vector3(Mathf.Lerp(Camera.main.transform.eulerAngles.x, 0, timer / 10f), 0, 0);
            yield return null;
        }
        GameManager.Instance._podiumFlow.enabled = true;
    }
}
