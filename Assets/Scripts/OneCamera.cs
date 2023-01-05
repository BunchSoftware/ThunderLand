using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCamera : MonoBehaviour
{
    private float mouseScrollDeltaY = 0f; // Текущее положение колесико мыши

    private float currentPositionZ; // Текущее положение по оси Z
    private float currentVelocityZ; // Текущее скорость Z

    private float rotY; // Поворот Y
    private float rotX; // Поворот X

    private float currentRotX; // Текущий поворот X
    private float currentRotY; // Текущий поворот Y

    private float currentVellocityX; // Текущая скорость X
    private float currentVellocityY; // Текущая скорость Y

    private float smoothTime = 0.1f; // Время сглаживания

    [SerializeField] private Vector3 offset;

    [SerializeField] private Transform player;
    [Range(0.1f, 10)]
    [SerializeField] private float sensivity = 5f;  // Чуствительность 

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.position += offset;
    }

    private void MouseMove()
    {
        // Рассчет позиции мыши
        rotY += Input.GetAxis("Mouse Y") * sensivity;
        rotX += Input.GetAxis("Mouse X") * sensivity;

        // Огранечение на поврот по оси Y
        rotY = Mathf.Clamp(rotY, -90, 90);

        currentRotX = Mathf.SmoothDamp(currentRotX, rotX, ref currentVellocityX, smoothTime); // Сглаживание поворота камеры
        currentRotY = Mathf.SmoothDamp(currentRotY, rotY, ref currentVellocityY, smoothTime);
        this.transform.rotation = Quaternion.Euler(-currentRotY, currentRotX, 0); // Поворот камеры
        player.rotation = Quaternion.Euler(0, rotX, 0);
    }
    private void MouseScrollWheel()
    {
        // Рассчет положения колесика
        mouseScrollDeltaY += Input.GetAxisRaw("Mouse ScrollWheel");

        currentPositionZ = Mathf.SmoothDamp(currentPositionZ, mouseScrollDeltaY, ref currentVelocityZ, smoothTime);

        if (mouseScrollDeltaY > 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, currentPositionZ);
        }
        if (mouseScrollDeltaY < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -currentPositionZ);
        }
    }
    private void Update()
    {
        MouseMove();
        MouseScrollWheel();
    }
}
