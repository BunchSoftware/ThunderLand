using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCamera : MonoBehaviour
{
    private float rotY; // Поворот Y
    private float rotX; // Поворот X

    private float currentRotX; // Текущий поворот X
    private float currentRotY; // Текущий поворот Y

    private float currentVellocityX; // Текущая скорость X
    private float currentVellocityY; // Текущая скорость Y

    private float smoothTime = 0.1f; // Время сглаживания
    [SerializeField] private Transform player;
    [Range(0.1f, 10)]
    [SerializeField] private float sensivity = 5f;  // Чуствительность 

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
    private void Update()
    {
        MouseMove();
    }
}
