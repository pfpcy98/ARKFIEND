using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float hp;

    public virtual void OnHit(float dmg) { }
    public virtual void OnDie() { }
}
