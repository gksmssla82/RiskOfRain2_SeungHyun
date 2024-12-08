using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    [SerializeField] private GameObject m_UI_KeyDownESC;
    [SerializeField] private GameObject m_SettingMenu;
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_UI_KeyDownESC.activeSelf == false)
            {
                m_UI_KeyDownESC.SetActive(true);
            }
        }

        if (m_UI_KeyDownESC.activeSelf == true || m_SettingMenu.activeSelf == true)
        {
            PauseManager.m_Instance.PauseGame();
            CursorManager.m_Instance.Show_Cursor(true);
        }
        else
        {
            PauseManager.m_Instance.ResumeGame();
            CursorManager.m_Instance.Show_Cursor(false);
        }
    }

   
}
