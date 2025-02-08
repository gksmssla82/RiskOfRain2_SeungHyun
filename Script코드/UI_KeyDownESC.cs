using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_KeyDownESC : MonoBehaviour
{
    [SerializeField] private GameObject m_SettingMenu;
 
    void Awake()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameObject.activeSelf == true)
            {
                AudioManager.Instance.Play(gameObject, "Button_Exit");
                //PauseManager.m_Instance.PauseGame();
                gameObject.SetActive(false);
            }


        }
    }


    

    public void OnClick_Select_Play()
    {
        gameObject.SetActive(false);
    }


    public void OnClick_Select_SettingMenu()
    {

        m_SettingMenu.SetActive(true);
        gameObject.SetActive(false);
        
    }


    public void OnClick_Select_MainMenu()
    {
        BgmManager.Instance.Stop();
        SceneManager.LoadScene("MainMenu");
        gameObject.SetActive(false);
    }

    public void OnClick_Select_Quit()
    {
        gameObject.SetActive(false);
#if UNITY_EDITOR
        AudioManager.Instance.Play(gameObject, "Button_Click");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
