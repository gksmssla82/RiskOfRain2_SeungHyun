using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_BlurImg;
    [SerializeField] private GameObject m_SettingMenu;
    private GameObject m_KeyDownEscUI;
    private GameObject m_UIController;
    //[SerializeField] private AudioManager m_AudioManager;
    //[SerializeField] private BgmManager m_BgmManager;

    void Start()
    {
        BgmManager.m_Instnace.Play(0);
        CursorManager.m_Instance.Show_Cursor(true);
        if (Dont_Destroy.m_Instance.m_SettingMenu != null)
        {
            m_SettingMenu = Dont_Destroy.m_Instance.m_SettingMenu;
        }

        if (Dont_Destroy.m_Instance.m_KeyDownExcUI != null)
        {
            m_KeyDownEscUI = Dont_Destroy.m_Instance.m_KeyDownExcUI;
        }

        if (Dont_Destroy.m_Instance.m_UIController != null)
        {
            m_UIController = Dont_Destroy.m_Instance.m_UIController;
        }

        m_UIController.SetActive(false);
    }

   


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if (m_BlurImg.activeSelf == true)
            //{
               
            //    //m_BlurImg.SetActive(false);
            //}

            if (m_SettingMenu.activeSelf == true)
            {
                AudioManager.m_Instnace.Play(gameObject, "Button_Exit");
                m_SettingMenu.SetActive(false);
            }
        }
    }

    public void OnClick_GameStart()
    {
        AudioManager.m_Instnace.Play(gameObject,"Button_Click");
        BgmManager.m_Instnace.Stop();
        m_UIController.SetActive(true);
        SceneManager.LoadScene("Loading");
        
    }

    public void OnClick_Setting()
    {

        AudioManager.m_Instnace.Play(gameObject,"Button_Click");
      
        //m_BlurImg.SetActive(true);
        m_SettingMenu.SetActive(true);
    }

    public void OnClick_Quit()
    {
#if UNITY_EDITOR
        AudioManager.m_Instnace.Play(gameObject,"Button_Click");
        UnityEditor.EditorApplication.isPlaying = false;
#else

        Application.Quit();
#endif
    }
}
