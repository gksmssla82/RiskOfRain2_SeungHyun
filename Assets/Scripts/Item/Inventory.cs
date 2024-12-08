using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject m_SlotsParent;
    [SerializeField] private Player m_Player;
    private Slot[] m_Slots;
    public GameObject[] m_ActiveItemObject;
    void Start()
    {
        if (m_Player == null)
        {
            m_Player = Object.FindAnyObjectByType<Player>();
        }

        if (m_Slots == null)
        { 
            m_Slots = m_SlotsParent.GetComponentsInChildren<Slot>();
        }
    }

   

    public void Acquire_Item(Item _item, int _Count = 1)
    {
        bool ItemAdd = false;

        // ��Ƽ��������� �ƴҰ�쿡�� ���� ī��Ʈ����
        if (Item.ItemType.ActiveItem != _item.m_ItemType)
        {
            for (int i = 0; i < m_Slots.Length; i++)
            {
                if (m_Slots[i].m_Item != null)
                {
                    if (m_Slots[i].m_Item.m_ItmeName == _item.m_ItmeName)
                    {
                        m_Slots[i].Set_SlotCount(_Count);
                        ItemAdd = true;
                        break;
                    }
                }
            }
        }

        // ���� ���ڸ� ä���
        if (!ItemAdd)
        {
            for (int i = 0; i < m_Slots.Length; i++)
            {
                if (m_Slots[i].m_Item == null)
                {
                    m_Slots[i].Add_Item(_item, _Count);
                    ItemAdd = true;
                    break;
                }
            }
        }

        if (_item.m_ItemType == Item.ItemType.StatusItem)
        {
            m_Player.GetComponent<ActiveItem>().Apply_ItemEffect(_item, m_ActiveItemObject);
        }
    }

   

    public void Use_Item(Item _Item)
    {
        // Active ������ ã��
        GameObject activeItemObject = Get_ActiveItemObject(_Item);

    }


    private GameObject Get_ActiveItemObject(Item _Item)
    {
        foreach(var obj in m_ActiveItemObject)
        {
            if (obj.name == _Item.m_ItmeName)
            {
                return obj;
            }
            
            else
            {
                Debug.Log("�ش� �������� ã�� ���߽��ϴ�. ������ �̸��� �´��� Ȯ�����ּ���");
            }
        }

        return null;
    }
}
