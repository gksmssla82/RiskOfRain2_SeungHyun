using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_Menu : MonoBehaviour
{

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameObject.activeSelf == true)
            {
                AudioManager.m_Instnace.Play(gameObject, "Button_Exit");
                //PauseManager.m_Instance.PauseGame();
                gameObject.SetActive(false);
            }


        }
    }

}
