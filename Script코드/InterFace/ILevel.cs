using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevel
{
    public float m_Level { get; set; }
    public float m_MaxExp { get; set; }
    public float m_CurrentExp { get; set; }

    public void Level_Up();

    public void Gain_Exp(float _Exp);
    
}
