using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager m_Instance;

    public bool m_isPaused = false;

    public void Start()
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
