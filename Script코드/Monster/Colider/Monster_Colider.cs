using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster_Colider : MonoBehaviour
{
    protected Player m_Player;
    protected MonsterBase m_Monster;
    protected virtual void Start()
    {
        if (m_Monster == null)
        {
            m_Monster = GetComponentInParent<MonsterBase>();
        }
        if (m_Player == null)
        {
            m_Player = Object.FindAnyObjectByType<Player>();
        }
    }
    protected abstract void OnTriggerEnter(Collider other);
    protected abstract void OnTriggerStay(Collider other);

    protected abstract void OnTriggerExit(Collider other);
    



}
