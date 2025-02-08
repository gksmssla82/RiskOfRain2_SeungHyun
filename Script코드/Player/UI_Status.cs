using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;
public class UI_Status : MonoBehaviour
{
    [SerializeField] private GameObject m_UIStauts;
    private Player m_Player;

    [SerializeField] private TextMeshProUGUI m_StatusTextHp;
    [SerializeField] private TextMeshProUGUI m_StatusTextDamage;
    [SerializeField] private TextMeshProUGUI m_StatusTextDefence;
    [SerializeField] private TextMeshProUGUI m_StatusTextSpeed;
    [SerializeField] private TextMeshProUGUI m_StatusTextCritical;
    [SerializeField] private TextMeshProUGUI m_StatusTextCriticalDamage;

    void Start()
    {
        m_Player = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Update_StatusUI();
            m_UIStauts.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            m_UIStauts.SetActive(false);
        }
    }


    private void Update_StatusUI()
    {
        m_StatusTextHp.text = "최대 HP : " + m_Player.m_MaxHp;
        m_StatusTextDamage.text = "데미지 : " + m_Player.m_MinDamage + " ~ " + m_Player.m_MaxDamage;
        m_StatusTextDefence.text = "방어력 : " + m_Player.m_Defence;
        m_StatusTextSpeed.text = "이동속도 : " + m_Player.GetComponent<ThirdPersonController>().m_MoveSpeed;
        m_StatusTextCritical.text = "크리티컬 확률 : " + m_Player.m_CriticalProbability * 100f + "%";
        m_StatusTextCriticalDamage.text = "크리티컬 데미지 : " + m_Player.m_CriticalDamage * 100f + "%";

    }

}
