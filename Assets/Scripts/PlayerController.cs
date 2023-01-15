using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float speedRotation;
    [SerializeField] private GameObject prefabCursorPoint;
    [SerializeField] private float timeDetection;

    private float distanceToTarget;
    private float distanceToTargetY;
    private Vector3 target;
    private GameObject currentCursorPoint;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
       ClickToWay();
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
