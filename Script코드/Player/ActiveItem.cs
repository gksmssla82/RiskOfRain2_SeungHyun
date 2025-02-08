using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
public class ActiveItem : MonoBehaviour
{
    private Player m_Player;
    void Start()
    {
        m_Player = GetComponent<Player>();
    }

    public void Apply_ItemEffect(Item _Item, GameObject[] _ActiveItemObject)
    {
        if (_Item.m_Add_Hp > 0)
        {
            m_Player.m_MaxHp += _Item.m_Add_Hp;
            m_Player.m_CurrentHp = m_Player.m_MaxHp;
            m_Player.Update_HpUI();
            Debug.Log(m_Player.m_MaxHp);
        }

        if (_Item.m_Add_Defence > 0)
        {
            m_Player.m_Defence += _Item.m_Add_Defence;
        }

        if (_Item.m_Add_CriticalProbability > 0)
        {
            m_Player.m_CriticalProbability += _Item.m_Add_CriticalProbability;
            Debug.Log(m_Player.m_CriticalProbability);
        }

        if (_Item.m_Add_CriticalDamage > 0)
        {
            m_Player.m_CriticalDamage += _Item.m_Add_CriticalDamage;
        }

        if (_Item.m_Add_Speed > 0)
        {

            m_Player.GetComponent<ThirdPersonController>().m_MoveSpeed += _Item.m_Add_Speed;
        }

        if (_Item.m_Add_SprintSpeed > 0)
        {
            m_Player.GetComponent<ThirdPersonController>().m_SprintSpeed += _Item.m_Add_Speed;
        }

        if (_Item.m_Add_RecoveryRate > 0)
        {
            m_Player.m_RecoveryRate += _Item.m_Add_RecoveryRate;
        }



        // 아이템 이름에 맞는 GameObject를 찾아서 활성화
        foreach (var obj in _ActiveItemObject)
        {
            if (obj != null && obj.name == _Item.m_ItmeName && !obj.activeSelf)
            {
                obj.SetActive(true);
            }
        }
    }

   
}
