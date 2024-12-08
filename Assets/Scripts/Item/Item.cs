using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]

public class Item  : ScriptableObject // 게임오브젝트에 붙힐필요가없음
{
    public enum ItemType
    {
        ActiveItem, // 장비 
        StatusItem, // 소모품
        Etc // ~등
    }

    public ItemType m_ItemType;
    public string m_ItmeName;
    public string m_ItemInfo;
    public Sprite m_ItemImg;
    public GameObject m_ItemPrefab;
    


    public float m_Add_Hp;
    public int m_Add_Defence;
    public float m_Add_Speed;
    public float m_Add_SprintSpeed;
    public float m_Add_CriticalProbability;
    public float m_Add_CriticalDamage;
    public float m_Add_RecoveryRate;

}
