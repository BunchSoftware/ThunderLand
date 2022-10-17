using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private bool isGrounded = true;
    //public float moveInputVertical = 0;
    //public float moveInputHorizontal = 0;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //moveInputHorizontal = Input.GetAxis("Horizontal");
        //moveInputVertical = Input.GetAxis("Vertical");
        Run();
        Jump();
    }
    private void Stand()
    {
        rigidbody.velocity = new Vector2(0, 0);
    }
    private void Run()
    {
            if (Input.GetKey(KeyCode.W))
            {
                transform.localPosition += transform.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.localPosition += -transform.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.localPosition += -transform.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.localPosition += transform.right * speed * Time.deltaTime;
            }
    }
    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) & isGrounded)
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = false;
    }
    //private void Walk()
    //{
    //    rigidbody.velocity = new Vector2(_speed * moveInputHorizontal, rigidbody.velocity.y);
    //    rigidbody.velocity = new Vector2(rigidbody.velocity.x, _speed * moveInputVertical);
    //}
}
