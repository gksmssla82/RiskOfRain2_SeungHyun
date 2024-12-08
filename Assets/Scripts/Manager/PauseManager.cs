using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager m_Instance;

    public bool m_isPaused = false;

    public void Start()
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
