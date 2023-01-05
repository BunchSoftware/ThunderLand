using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigibody;
    public float w_speed, wb_speed, olw_speed, rn_speed, ro_speed;
    public bool walking;

    private void Start()
    {
        rigibody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigibody.velocity = transform.forward * w_speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigibody.velocity = -transform.forward * wb_speed;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            walking = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            walking = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -ro_speed, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, ro_speed, 0);
        }
        if (walking == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                w_speed = w_speed + rn_speed;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                w_speed = olw_speed;
            }
        }
    }
}
