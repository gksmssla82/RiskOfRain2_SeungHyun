using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dont_Destroy : MonoBehaviour
{
    public static Dont_Destroy m_Instance;
    

    public GameObject m_SettingMenu;
    public GameObject m_KeyDownExcUI;
    public GameObject m_UIController;
    void Awake()
    {
        // �̱��� ����
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ������Ʈ ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� ���� ����
            return;
        }
    }

   
}
