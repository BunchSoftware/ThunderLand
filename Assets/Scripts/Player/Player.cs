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

    private void Start()
    {
        healthModule.OnHealthChanged += HealthModule_OnHealthChanged;
        clientHandler = GetComponent<ClientHandler>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void HealthModule_OnHealthChanged(float health)
    {
        healthBar.Handle.fillAmount = health / healthModule.MaxHealth;
        healthBar.Amount.text = healthModule.Health.ToString();
    }

    private void Update()
    {
       //clientHandler.package.position = new Vector3Package(transform.position.x, transform.position.y, transform.position.z);
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ������������ ����� ������� ����

            if (Physics.Raycast(ray, out hit, 100f) && hit.transform.tag == "Ground") // ��������� ��� ��� �����
            {
                target = hit.point;
                agent.SetDestination(target);
                
                DestroyCurrentCursorPoint();
                CreateCursorPoint(prefabCursorPoint);
            }
        }
        CheckPositionToTarget();
    }
    private void CheckPositionToTarget() // �������� �� ���������� target
    {
        distanceToTarget = Vector2.Distance(transform.position, target);
        distanceToTargetY = transform.position.y - target.y;    
        if (distanceToTarget == distanceToTargetY)
            DestroyCurrentCursorPoint();
    }
    private void CreateCursorPoint(GameObject cursorPoint) // �������� ����� �� �����������
    {
        currentCursorPoint = Instantiate(cursorPoint, target, cursorPoint.transform.rotation);
    }
    private void DestroyCurrentCursorPoint() // �������� ����� � �����������
    {
        if (currentCursorPoint != null)
            Destroy(currentCursorPoint);
    }
}