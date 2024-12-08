using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus
{
    public float m_MinDamage { get; set; }
    public float m_MaxDamage { get; set; }
    public float m_CriticalDamage { get; set; }

    public float m_CriticalProbability { get; set; }

    public void Damage_Up();

}
