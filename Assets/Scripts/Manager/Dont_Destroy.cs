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
        // 싱글톤 설정
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 오브젝트 유지
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
            return;
        }
    }

   
}
