using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string m_ItemName; // �������� �̸� (Ű��)
    public string[] m_Part; // ����
    public int[] m_Num; // ��ġ
}
public class ItemEffctDB : MonoBehaviour
{
    private ItemEffect[] m_itemEffects;
   
   
    public void Use_Item(Item _item)
    {
        if (_item.m_ItemType == Item.ItemType.ActiveItem)
        {
            for (int x = 0; x < m_itemEffects.Length; x++)
            {
                if (m_itemEffects[x].m_ItemName == _item.m_ItmeName)
                {

                }
            }
        }
    }
}
