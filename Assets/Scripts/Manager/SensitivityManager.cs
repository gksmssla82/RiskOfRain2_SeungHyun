using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitivityManager : MonoBehaviour
{
    public static SensitivityManager m_Instance;

    public float m_CameraSensitivity = 1f;
    public float m_ShootingSensitivity = 1f;

    private void Awake()
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

    public void Set_CameraSensitivity(float _Sensitivity)
    {
        m_CameraSensitivity = _Sensitivity;
    }

    public void Set_ShootingSensitivity(float _Sensitivity)
    {
        m_ShootingSensitivity = _Sensitivity;
    }
    



}
