using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float speedRotation;
    [SerializeField] private GameObject prefabCursorPoint;

    private float distance;
    private float widthPlayer;
    private Vector3 target;
    private Rigidbody rigidbody;
    private bool isWalk = false;
    private GameObject currentCursorPoint;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        widthPlayer = GetComponent<MeshRenderer>().bounds.size.x;
    }
    private void Update()
    {
       ClickToWay();
    }
    private void FixedUpdate()
    {
        if (isWalk)
        {
            RotatePlayer();
            MovePlayer();
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
                isWalk = true;
                DestroyCurrentCursorPoint();
                CreateCursorPoint(prefabCursorPoint);
            }
        }
    }
    private void MovePlayer()
    {
        distance = Vector3.Distance(transform.position, target);
        if (distance > widthPlayer)
        {
            rigidbody.MovePosition(Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime));
        }
        if (distance < widthPlayer)
        {
            isWalk = false;
            DestroyCurrentCursorPoint();
        }
    }
    private void RotatePlayer()
    {
        Vector3 direction = target - transform.position;
        Quaternion lookQurtinion = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookQurtinion, speedRotation * Time.deltaTime);
    }
    private void CreateCursorPoint(GameObject cursorPoint)
    {
        currentCursorPoint = Instantiate(cursorPoint, target, cursorPoint.transform.rotation);
    }
    private void DestroyCurrentCursorPoint()
    {
        if (currentCursorPoint != null)
        {
            Destroy(currentCursorPoint);
        }
    }
}
