 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [Header("������������ ������")]
    [SerializeField] private Transform target; // ���� �� ������� ������� ������
    [SerializeField] private float sensition; // ��������������� ����
    [SerializeField] private float smoothMouseMoveTime;
    private Vector3 euler; // ���� ��������
    private float mouseY = 0f; // ��������� ���� �� Y
    private float mouseX = 0f; // ��������� ���� �� X
    private Vector3 velocityEuler;

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
        MouseRotate();
    }
    private void MouseScrollWheel()
    {
        mouseScrollDeltaY -= Input.mouseScrollDelta.y;
        mouseScrollDeltaY = Mathf.Clamp(mouseScrollDeltaY, 0, maxZoom);

        zoom = Mathf.SmoothDamp(zoom, mouseScrollDeltaY, ref zoomVelocity, smoothZoomTime);
        zoom = Mathf.Clamp(zoom, 0, maxZoom);

        transform.position = target.position;

        transform.position = Vector3.Lerp(transform.position, target.position, Time.fixedDeltaTime);

        transform.Translate(new Vector3(0, 0, -zoom));
    }
    private void MouseRotate()
    {
        if (Input.GetMouseButton(1))
        {
            mouseX = Input.GetAxis("Mouse X") * sensition;
            mouseY = Input.GetAxis("Mouse Y") * sensition;

        euler.x -= mouseY;

        euler.x = Mathf.Clamp(euler.x, 0f, 90.0f);

        euler.y += mouseX;

        euler.z = 0;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(euler), Time.fixedDeltaTime * sensition);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        }
    }
}


