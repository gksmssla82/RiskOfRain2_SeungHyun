using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitivityManager : Singleton<SensitivityManager>
{
    public float m_CameraSensitivity = 1f;
    public float m_ShootingSensitivity = 1f;

    public void Set_CameraSensitivity(float _Sensitivity)
    {
        m_CameraSensitivity = _Sensitivity;
    }

    public void Set_ShootingSensitivity(float _Sensitivity)
    {
        m_ShootingSensitivity = _Sensitivity;
    }
    



}
