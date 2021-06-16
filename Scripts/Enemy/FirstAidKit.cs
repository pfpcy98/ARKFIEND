using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidKit : Enemy
{
    [SerializeField]
    private GameObject prefab_gainEffect;
    [SerializeField]
    private int healAmount = 2;

    public override void OnHit(float dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            OnDie();
        }
    }

    public override void OnDie()
    {
        if (prefab_gainEffect != null)
        {
            Instantiate(prefab_gainEffect, transform.position, Quaternion.identity);
        }

        GameManager.GetInstance().TakeHeal(healAmount);

        gameObject.SetActive(false);
    }
}
