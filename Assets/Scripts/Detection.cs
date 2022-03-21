using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public LayerMask _layer; // layering to control collisions

    Collider _collider; // object's collider

    RaycastHit _hit;

    int indexSibling; // index of the object in hierarchy

    Vector3 _rayOriginPos; // position of the raycast start pos

    #region Collider Extents
    // to ease up redudent calculation
    float _colliderExt_X;
    float _colliderExt_Z;
    #endregion
    private void Start()
    {
        _collider = GetComponent<Collider>();
        #region Assigning Values
        indexSibling = transform.GetSiblingIndex(); // Getting index in hierarchy

        _colliderExt_X = _collider.bounds.extents.x;
        _colliderExt_Z = _collider.bounds.extents.z;
        #endregion
    }

    private void Update()
    {
        _rayOriginPos = new Vector3(transform.position.x, transform.position.y, GameManager.Instance._player.transform.position.z); // Adjusting Z position of sub-hats to allign on Z-axis

        if (Physics.SphereCast(_rayOriginPos, _colliderExt_X, Vector3.forward, out _hit, _colliderExt_Z, _layer)) // Racyasting to detect collision with traps
        {
            if (_hit.transform == null) // if no TRAP layered object detected, DO NOTHING
                return;
            if (_hit.transform.parent.TryGetComponent<Interactable>(out var interactable)) // Check if TRAP layered object detected and assign script
            {
                interactable._value = indexSibling; // Assign sibling index tp collided object's script
                interactable.Operate(); // Call function in collided object's script
                if (!interactable._particleCloud.isPlaying) // check if particle is playing
                {
                    interactable._particleCloud.transform.position = _hit.point; // set position to collision point
                    interactable._particleCloud.Play(); // play effect
                }
            }
        }
    }

    private void OnDrawGizmos()// For testing
    {
        Gizmos.DrawSphere(_rayOriginPos, _colliderExt_X);
    }
}
