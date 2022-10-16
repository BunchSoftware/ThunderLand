using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Animator animator;
    [SerializeField] private float speed;
    public float moveInputVertical = 0;
    public float moveInputHorizontal = 0;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        moveInputHorizontal = Input.GetAxis("Horizontal");
        moveInputVertical = Input.GetAxis("Vertical");
        if ((moveInputHorizontal > 0 || moveInputHorizontal < 0) || (moveInputVertical > 0 || moveInputVertical < 0))
        {
            Run();
        }
        else
        {
            Stand();
        }
    }
    private void Stand()
    {
        rigidbody.velocity = new Vector2(0, 0);
    }
    private void Run()
    {
        rigidbody.velocity = new Vector3(speed * moveInputHorizontal, rigidbody.velocity.y, rigidbody.velocity.z);
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, speed * moveInputVertical);
    }
    //private void Walk()
    //{
    //    rigidbody.velocity = new Vector2(_speed * moveInputHorizontal, rigidbody.velocity.y);
    //    rigidbody.velocity = new Vector2(rigidbody.velocity.x, _speed * moveInputVertical);
    //}
}
