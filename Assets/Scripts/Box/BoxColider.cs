using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoxColider : MonoBehaviour
{
    [SerializeField] private GameObject m_ItemMessage;
    [SerializeField] private TextMeshProUGUI m_ItemPrice;
    public bool m_Open = false;
    private Player m_Player;

    public void Start()
    {
        m_Player = FindAnyObjectByType<Player>();
    }


    public void Box_Open()
    {
        Animator Anim = GetComponentInParent<ItemBox>().m_Anim;
        AudioManager.Instance.PlayOneShot(m_Player.gameObject, "ItemBox_Open");
        m_Open = true;
        Anim.SetTrigger("Open");
        this.gameObject.SetActive(false);

    }

    private void Item_Buy()
    {
        int price = GetComponentInParent<ItemBox>().m_Price;

        if (m_Player.m_Money >= price)
        {
            m_Player.Money_Change(-price);
            Debug.Log("¹Ú½º¿ÀÇÂ");
            m_ItemMessage.SetActive(false);
            Box_Open();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if(other.CompareTag("Player") && !m_Open)
        {
            float Price = GetComponentInParent<ItemBox>().m_Price;
            m_ItemPrice.text = $"(${Price})";
            m_ItemMessage.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Item_Buy();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_ItemMessage.SetActive(false);
        }
    }
}
