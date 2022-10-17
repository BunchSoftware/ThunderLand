using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCamera : MonoBehaviour
{
    private float rotY; // ������� Y
    private float rotX; // ������� X

    private float currentRotX; // ������� ������� X
    private float currentRotY; // ������� ������� Y

    private float currentVellocityX; // ������� �������� X
    private float currentVellocityY; // ������� �������� Y

    private float smoothTime = 0.1f; // ����� �����������
    [SerializeField] private Transform player;
    [Range(0.1f, 10)]
    [SerializeField] private float sensivity = 5f;  // ��������������� 

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void MouseMove()
    {
        // ������� ������� ����
        rotY += Input.GetAxis("Mouse Y") * sensivity;
        rotX += Input.GetAxis("Mouse X") * sensivity;
        // ����������� �� ������ �� ��� Y
        rotY = Mathf.Clamp(rotY, -90, 90);

        currentRotX = Mathf.SmoothDamp(currentRotX, rotX, ref currentVellocityX, smoothTime); // ����������� �������� ������
        currentRotY = Mathf.SmoothDamp(currentRotY, rotY, ref currentVellocityY, smoothTime);
        this.transform.rotation = Quaternion.Euler(-currentRotY, currentRotX, 0); // ������� ������
        player.rotation = Quaternion.Euler(0, rotX, 0);
    }
    private void Update()
    {
        MouseMove();
    }
}
