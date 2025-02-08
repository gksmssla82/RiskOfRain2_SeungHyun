using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player_Colider : MonoBehaviour
{
    protected Player m_Player;
    protected MonsterBase m_Monster;
    protected virtual void Start()
    {
        if (m_Player == null)
        {
            m_Player = GetComponentInParent<Player>();
        }
        if (m_Monster == null)
        {
            m_Monster = Object.FindAnyObjectByType<MonsterBase>();
        }
    }
    protected abstract void OnTriggerEnter(Collider other);
    protected abstract void OnTriggerStay(Collider other);

    protected abstract void OnTriggerExit(Collider other);
    



}
