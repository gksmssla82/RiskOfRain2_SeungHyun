using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    public float m_MaxHp { get; set; }
    public float m_CurrentHp { get; set; }
    public int m_Defence { get; set; }
    public void Take_Damage(float _Damage);
    public void Dead_Check();
}
