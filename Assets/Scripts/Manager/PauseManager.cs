using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    public bool m_isPaused = false;
    public void PauseGame()
    {
        Time.timeScale = 0f;         // 게임 시간 멈춤
        m_isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;          // 게임 시간 재개
        m_isPaused = false;
    }
}
