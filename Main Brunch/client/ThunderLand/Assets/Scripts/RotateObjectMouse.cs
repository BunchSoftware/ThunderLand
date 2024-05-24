using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectMouse : MonoBehaviour
{
    [SerializeField] private float speedRotation = 5f;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            transform.Rotate(new Vector3(0, -Input.GetAxis("Mouse X") * speedRotation, 0), Space.World);
        }
    }
}
