using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("������������ ������")]
    [SerializeField] private Transform target; // ���� �� ������� ������� ������
    [SerializeField] private float sensition; // ��������������� ����
    [SerializeField] private float smoothMouseMoveTime;
    private Vector3 euler; // ���� ��������
    private float eulerVelocityY = 0f; // ��� �������� ������ Y
    private float eulerVelocityX = 0f; // ��� �������� ������ X
    private float mouseY = 0f; // ��������� ���� �� Y
    private float mouseX = 0f; // ��������� ���� �� X

    [Header("��������� ����������� ������")]
    [SerializeField] private float maxZoom; // ������������ ����������
    [SerializeField] private float smoothZoomTime = 0.35f;
    private float zoom; // ����������
    private float zoomVelocity = 0f; // ��� ����������
    private float mouseScrollDeltaY = 0f;


    private void Start()
    {
        euler = transform.localEulerAngles;
    }
    private void LateUpdate()
    {
        MouseScrollWheel();
        MouseMove();
    }
    private void MouseScrollWheel()
    {
        mouseScrollDeltaY += Input.mouseScrollDelta.y;
        mouseScrollDeltaY = Mathf.Clamp(mouseScrollDeltaY, 0, maxZoom);

        transform.position = target.position;

        zoom = Mathf.SmoothDamp(zoom, mouseScrollDeltaY, ref zoomVelocity, smoothZoomTime);

        transform.Translate(new Vector3(0, 0, -zoom));
    }
    private void MouseMove()
    {
        if (Input.GetMouseButton(1))
        {
            mouseX = Input.GetAxis("Mouse X") * sensition;
            mouseY = Input.GetAxis("Mouse Y") * sensition;

                   euler.x -= mouseY;
        euler.x = Mathf.Clamp(euler.x, 0f, 90.0f);

        euler.y += mouseX;

        euler.z = 0;

        transform.eulerAngles = euler;
        }
    }
}


