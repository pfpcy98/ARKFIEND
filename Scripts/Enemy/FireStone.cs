using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStone : Enemy
{
    [SerializeField]
    private GameObject prefab_Break;

    private const string playerTag = "MainCamera";

    public float moveSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GetInstance().is_GameStart)
        {
            transform.LookAt(GameManager.GetInstance().GetPlayerTransform().position);
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        }
    }

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
        if(prefab_Break != null)
        {
            Instantiate(prefab_Break, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (prefab_Break != null)
            {
                Instantiate(prefab_Break, transform.position, Quaternion.identity);
            }
            GameManager.GetInstance().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
