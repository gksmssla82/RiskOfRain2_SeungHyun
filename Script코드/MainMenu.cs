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
        BgmManager.Instance.Play(0);
        CursorManager.Instance.Show_Cursor(true);
        if (UIManager.Instance.m_SettingMenu != null)
        {
            m_SettingMenu = UIManager.Instance.m_SettingMenu;
        }

        if (UIManager.Instance.m_KeyDownExcUI != null)
        {
            m_KeyDownEscUI = UIManager.Instance.m_KeyDownExcUI;
        }

        if (UIManager.Instance.m_UIController != null)
        {
            m_UIController = UIManager.Instance.m_UIController;
        }

        m_UIController.SetActive(false);
    }

   


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_SettingMenu.activeSelf == true)
            {
                AudioManager.Instance.Play(gameObject, "Button_Exit");
                m_SettingMenu.SetActive(false);
            }
        }
    }

    public void OnClick_GameStart()
    {
        AudioManager.Instance.Play(gameObject,"Button_Click");
        BgmManager.Instance.Stop();
        m_UIController.SetActive(true);
        SceneManager.LoadScene("Loading");
        
    }

    public void OnClick_Setting()
    {

        AudioManager.Instance.Play(gameObject,"Button_Click");
      
        //m_BlurImg.SetActive(true);
        m_SettingMenu.SetActive(true);
    }

    public void OnClick_Quit()
    {
#if UNITY_EDITOR
        AudioManager.Instance.Play(gameObject,"Button_Click");
        UnityEditor.EditorApplication.isPlaying = false;
#else

        Application.Quit();
#endif
    }
}
