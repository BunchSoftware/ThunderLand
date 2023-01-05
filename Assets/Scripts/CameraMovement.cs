using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Приследуемый обьект")]
    [SerializeField] private Transform target; // Цель за которой следует камера
    [SerializeField] private float sensition; // Чуствительность мыши
    [SerializeField] private float smoothMouseMoveTime;
    private Vector3 euler; // Углы поворота
    private float eulerVelocityY = 0f; // Ось поворота камеры Y
    private float eulerVelocityX = 0f; // Ось поворота камеры X
    private float mouseY = 0f; // Положение мыши по Y
    private float mouseX = 0f; // Положение мыши по X

    [Header("Настройка приближения камеры")]
    [SerializeField] private float maxZoom; // Максимальное увелечение
    [SerializeField] private float smoothZoomTime = 0.35f;
    private float zoom; // Увелечение
    private float zoomVelocity = 0f; // Ось Увелечения
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


