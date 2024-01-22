using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject prefabCursorPoint;
    [SerializeField] private HealthModule healthModule;
    [SerializeField] private string Name;
    private ClientHandler clientHandler;

    private float distanceToTarget;
    private float distanceToTargetY;
    private Vector3 target;
    private GameObject currentCursorPoint;
    private NavMeshAgent agent;
    [SerializeField] private ImageBar healthBar;

    private Animator animator;

    private void Start()
    {
        healthModule.OnHealthChanged += HealthModule_OnHealthChanged;
        clientHandler = GetComponent<ClientHandler>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetFloat("speed", 0);
    }

    private void HealthModule_OnHealthChanged(float health)
    {
        healthBar.Handle.fillAmount = health / healthModule.MaxHealth;
        healthBar.Amount.text = healthModule.Health.ToString();
    }

    private void Update()
    {
       ClickToWay();
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            target = transform.position;
            target = new Vector3(transform.position.x, transform.position.y, target.z + 1);
            agent.SetDestination(target);
        }
    }
    private void ClickToWay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Рассчитываем точку касания мыши

            if (Physics.Raycast(ray, out hit, 100f) && hit.transform.tag == "Ground") // Проверяем что это земля
            {
                target = hit.point;
                agent.SetDestination(target);
                animator.SetFloat("speed", 1);
                DestroyCurrentCursorPoint();
                CreateCursorPoint(prefabCursorPoint);
            }
        }
        CheckPositionToTarget();
    }
    private void CheckPositionToTarget() // Проверка на достижения target
    {
        distanceToTarget = Vector2.Distance(transform.position, target);
        distanceToTargetY = transform.position.y - target.y;    
       
        if (Math.Round(distanceToTarget, 2) == Math.Round(distanceToTargetY, 2))
        {
            DestroyCurrentCursorPoint();
            animator.SetFloat("speed", 0);
        }
    }
    private void CreateCursorPoint(GameObject cursorPoint) // Создание точки на поверхности
    {
        currentCursorPoint = Instantiate(cursorPoint, target, cursorPoint.transform.rotation);
    }
    private void DestroyCurrentCursorPoint() // Удаление точки с поверхности
    {
        if (currentCursorPoint != null)
            Destroy(currentCursorPoint);
    }
}
