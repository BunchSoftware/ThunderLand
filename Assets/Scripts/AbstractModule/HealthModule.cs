using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthModule : MonoBehaviour
{

    private float health = 0;
    [HideInInspector] public float Health { get => health; }

    public delegate void MaxHealthRestored();
    public event MaxHealthRestored OnMaxHealthRestored;

    public delegate void Died();
    public event Died OnDied;

    public delegate void HealthChanged(float health);
    public event HealthChanged OnHealthChanged;

    /// <summary>
    /// �� ��������� ����������� 100 ��������.
    /// </summary>
    public float MaxHealth = 100;

    /// <summary>
    /// �� ��������� ���������� 100 ��������.
    /// </summary>
    public float MinHealth = 0;

    private void Start()
    {
        health = MaxHealth;
        OnHealthChanged(health);
    }

    /// <summary>
    /// �������� ��� ��������� ��������.
    /// </summary>
    /// <param name="health">���������� ����������� ��� ����������� ��������</param>
    public void RecountHealth(float health)
    {
        this.health += health;
        OnHealthChanged(this.health);

        if (this.health > MaxHealth)
            this.health = MaxHealth;
        if (health <= MinHealth)
             OnDied?.Invoke();
    }
    /// <summary>
    /// �������� ��� ��������� �������� �� ��������.
    /// *���� ������� 0 ������ �� �� ����� ��������.
    /// </summary>
    /// <param name="health">���������� ����������� ��� ����������� ��������</param>
    /// <param name="second">���������� ������</param>
    //public void RecountHealthForTimes(float health, float second)
    //{
    //     StartCoroutine(RecountHealthForTimesIE(health, second));
    //}
    //private IEnumerator RecountHealthForTimesIE(float health, float second)
    //{
    //    if (second != 0)
    //    {
    //        float tempHealth = this.health;
    //        float differenceHealth = tempHealth - health;
    //        if (health < 0)
    //        {
    //            while (this.health > MinHealth & this.health > (tempHealth + health))
    //            {
    //                this.health += health / second;
    //                if (this.health > MaxHealth)
    //                    this.health = MaxHealth;
    //                print(this.health);
    //                yield return new WaitForSeconds(Mathf.Abs(second / health));
    //            }
    //            if (health <= MinHealth)
    //                OnDied?.Invoke();
    //        }
    //        else
    //        {
    //            while (this.health < MaxHealth & this.health < (tempHealth + health))
    //            {
    //                this.health += health / second;
    //                if (this.health > MaxHealth)
    //                    this.health = MaxHealth;
    //                print(this.health);
    //                yield return new WaitForSeconds(second / health);
    //            }
    //            if (health >= MaxHealth)
    //                OnMaxHealthRestored?.Invoke();
    //        }
    //    }
    //}
}
