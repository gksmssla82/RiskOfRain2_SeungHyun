using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    public Item m_Item;
    public int m_ItemCount;
    public Image m_ItemImg;

    [SerializeField] private TextMeshProUGUI m_TextCount;

    private void SetColor(float _Alpha)
    {
        Color color = m_ItemImg.color;
        color.a = _Alpha;
        m_ItemImg.color = color;
    }

    public void Add_Item(Item _Item, int _Count = 1)
    {
        m_Item = _Item;
        m_ItemCount = _Count;
        m_ItemImg.sprite = m_Item.m_ItemImg;

        m_TextCount.text = m_ItemCount.ToString();

        SetColor(1);
    }


    public void Set_SlotCount(int _Count)
    {
        m_ItemCount += _Count;
        m_TextCount.text = m_ItemCount.ToString();

        if(m_ItemCount <= 0)
        {
            Clear_Slot();
        }
    }

    private void Clear_Slot()
    {
        m_Item = null;
        m_ItemCount = 0;
        m_ItemImg.sprite = null;
        SetColor(0);

        m_TextCount.text = "0";
    }
}
