using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionController : MonoBehaviour
{
    [SerializeField] private float m_Range;
    private bool m_PickupActivated = false; // ½Àµæ °¡´ÉÇÒ½Ã true
    private RaycastHit m_HitInfo; // Ãæµ¹Ã¼ Á¤º¸
    [SerializeField] private LayerMask m_Layer;
    [SerializeField] GameObject m_ActionUI;
    [SerializeField] TextMeshProUGUI m_ActionText;
    [SerializeField] Inventory m_Inventory;

    [SerializeField] GameObject m_ItemInfo_UI;
    [SerializeField] TextMeshProUGUI m_ItemNameText;
    [SerializeField] TextMeshProUGUI m_itemInfomationText;
    [SerializeField] Image m_ItemImg;

    // Update is called once per frame
    void Update()
    {
        Check_Item();
        Try_Action();   
    }

    private void Try_Action()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Check_Item();
            Can_PickUp();
        }
    }

    private void Check_Item()
    {
        Debug.DrawRay(transform.position, transform.forward * m_Range, Color.red);
        
        if (Physics.Raycast(transform.position, transform.forward, out m_HitInfo, m_Range, m_Layer))
        {
            if (m_HitInfo.transform.tag == "Item")
            {
                Debug.Log(m_HitInfo.transform.name);
                ItemInfo_Appear();
            }
        }
        else
            Info_Disappear();
        
    }

    private void ItemInfo_Appear()
    {
        m_PickupActivated = true;
        m_ActionUI.SetActive(true);
        m_ActionText.text = "(" + m_HitInfo.transform.GetComponent<ItemPickUp>().m_Item.m_ItmeName + ") È¹µæ";
    }

    private void Info_Disappear()
    {
        m_PickupActivated = false;
        m_ActionUI.SetActive(false);
    }

    private void Can_PickUp()
    {
        if (m_PickupActivated)
        {
            if (m_HitInfo.transform != null)
            {
                m_Inventory.Acquire_Item(m_HitInfo.transform.GetComponent<ItemPickUp>().m_Item);
                UI_ItemInfo();
                Destroy(m_HitInfo.transform.gameObject);
                AudioManager.m_Instnace.Random_SoundPlay(gameObject, 109, 4);
                Info_Disappear();
            }
        }
    }


    private void UI_ItemInfo()
    {
       
        ItemPickUp Item = m_HitInfo.transform.GetComponent<ItemPickUp>();
        m_ItemImg.sprite = Item.m_Item.m_ItemImg;
        m_ItemNameText.text = Item.m_Item.m_ItmeName;
        m_itemInfomationText.text = Item.m_Item.m_ItemInfo;
        m_ItemInfo_UI.SetActive(true);
        m_ItemInfo_UI.GetComponent<Fade>().Fade_InAndOut();
        
    }
}
