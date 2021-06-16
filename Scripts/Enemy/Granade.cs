using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : Enemy
{
    [SerializeField]
    private GameObject prefab_Explosion;

    [SerializeField]
    private float damage = 50;

    private const string enemyTag = "Enemy";

    public override void OnHit(float dmg)
    {
        hp -= dmg;

        if(hp <= 0)
        {
            OnDie();
        }
    }

    public override void OnDie()
    {
        if(prefab_Explosion != null)
        {
            Instantiate(prefab_Explosion, transform.position, Quaternion.identity);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        foreach(GameObject gameObject in enemies)
        {
            if (gameObject != this.gameObject)
            {
                Enemy enemy = gameObject.GetComponent<Enemy>();
                if (enemy != null &&
                    gameObject.GetComponent<FirstAidKit>() == null)
                {
                    enemy.OnHit(damage);
                }
            }
        }

        gameObject.SetActive(false);
    }
}
