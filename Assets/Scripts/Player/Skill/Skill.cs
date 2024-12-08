using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
public abstract class Skill 
{
    protected Player m_Player;
    public bool m_CanUse { get; set; } = true; // ��밡������?
    public bool m_IsUsing { get; set; } = false; // ���������
    public float m_CooldownTime { get; set; } // ��Ÿ�� �ð�
    public float m_Duration { get; set; } // ���� �ð�
    public float m_LastUseTime { get; set; }
    public Image m_Icon { get; set; } // ��ų ������

    public Skill_CoolDown m_CoolDownIcon;
    public StarterAssetsInputs m_Input;
    public CharacterController m_Controller;
    public abstract void Use_Skill(string _SkillName);

    protected abstract IEnumerator Corutine_Skill(string _SkillName);

 
    
        
   

    public Skill(Player _Player)
    {
        this.m_Player = _Player;
    }

}
