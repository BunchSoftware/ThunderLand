using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : Essense
{

    public Action OnEndPvPPoint;

    public event ParameterWithSlider OnPvPPointChanged;
    public event ParameterWithSlider OnExperienceChanged;
    public event ParameterWithSlider OnMoneyChanged;
    public event ParameterWithSlider OnSpaceOfInventoryChanged;
    public event ParameterWithSlider OnLevelChanged;


    private Clane clane;
    [HideInInspector] public Clane Clane { get => clane; }

    public int maxPvPPoint = 100;

    private int pvpPoint = 0;
    [HideInInspector] public int PvPPoint { get => pvpPoint; }

    public int maxExperience = 1000;

    private int experience = 0;
    [HideInInspector] public int Experience { get => experience; }

    public int maxSpaceInInventory = 160;

    private int spaceInInventory = 0;
    [HideInInspector] public int SpaceInInventory { get => spaceInInventory; }

    private int money = 0;
    [HideInInspector] public int Money { get => money; }


    [Header("Настройка передвижения")]
    [SerializeField] private GameObject prefabCursorPoint;
    [SerializeField] private SkillPanel skillPanel;

    public event Action<Vector3> OnChangePositionPlayer;


    private float distanceToTarget;
    private float distanceToTargetY;
    private Vector3 target;
    private GameObject currentCursorPoint;
    private NavMeshAgent agent;

    private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetFloat("speed", 0);

        RecountHealth(maxHealth);
        RecountMana(maxMana);
        RecountPvPPoint(maxPvPPoint);
        RecountMoney(0);
        RecountExperience(0);
        OnSpaceOfInventoryChanged(0);
        OnLevelChanged(level);

        for (int i = 0; i < skillPanel.cellSkills.Count; i++)
        {
            skillPanel.cellSkills[i].SkillPressed += (skill) =>
            {
                if(skill != null)
                    print(skill.NameSkill);
            };
        }
    }

    public void RecountPvPPoint(int pvpPoint)
    {
        this.pvpPoint += pvpPoint;
        OnPvPPointChanged(this.pvpPoint);

        if (this.pvpPoint > maxPvPPoint)
            this.pvpPoint = maxPvPPoint;
        if (this.pvpPoint <= 0)
            OnEndPvPPoint?.Invoke();
    }
    public void RecountExperience(int experience)
    {
        this.experience += experience;
        OnExperienceChanged(this.experience);

        if (this.experience > maxExperience)
        {
            this.experience = 0;
            if (level <= maxLevel)
            {
                level++;
                OnLevelChanged?.Invoke(level);
            }
        }
        if (this.experience < 0)
            throw new Exception("Опыт не может быть отрицательным.");
    }
    public void RecountMoney(int money)
    {
        this.money += money;
        OnMoneyChanged(this.money);

        if (this.money < 0)
            throw new Exception("Количество денег не может быть отрицательным.");
    }
    public void AddToInventory()
    {
        spaceInInventory++;
        OnSpaceOfInventoryChanged(spaceInInventory);
    }
    

    private void Update()
    {
        ClickToWay();
    }
    public void OnGUI()
    {
        for (int i = 0; i < skillPanel.cellSkills.Count; i++)
        {
            if (Event.current.type == EventType.KeyDown 
                && Event.current.modifiers == skillPanel.cellSkills[i].KeyCodeCombination.keyCombination 
                && Event.current.keyCode == skillPanel.cellSkills[i].KeyCodeCombination.keyCode)
            {
                skillPanel.cellSkills[i].SkillPress();           
            }
        }
    }
    private void ClickToWay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Рассчитываем точку касания мыши

            if (Physics.Raycast(ray, out hit, 100f) && hit.collider.tag == "Ground") // Проверяем что это земля
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                target = hit.point;
                agent.SetDestination(target);
                animator.SetFloat("speed", 1);
                DestroyCurrentCursorPoint();
                CreateCursorPoint(prefabCursorPoint);
            }
        }
        if(currentCursorPoint != null)
            CheckPositionToTarget();
    }
    private void CheckPositionToTarget() // Проверка на достижения target
    {
        OnChangePositionPlayer?.Invoke(transform.position);
        if (Math.Round(transform.position.x) == Math.Round(target.x) && Math.Round(transform.position.z) == Math.Round(target.z))
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
