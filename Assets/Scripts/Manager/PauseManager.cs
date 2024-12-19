using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    public bool m_isPaused = false;
    public void PauseGame()
    {
        Time.timeScale = 0f;         // ���� �ð� ����
        m_isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;          // ���� �ð� �簳
        m_isPaused = false;
    }
}
