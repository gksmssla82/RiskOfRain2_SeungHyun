using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
public abstract class Skill 
{
    public Skill(Player _Player)
    {
        this.m_Player = _Player;
    }

    protected Player m_Player;
    protected bool m_CanUse { get; set; } = true; // ��밡������?
    protected bool m_IsUsing { get; set; } = false; // ���������
    protected float m_CooldownTime { get; set; } // ��Ÿ�� �ð�
    protected float m_Duration { get; set; } // ���� �ð�
    protected float m_LastUseTime { get; set; }
    protected Image m_Icon { get; set; } // ��ų ������

    protected Skill_CoolDown m_CoolDownIcon;
    protected StarterAssetsInputs m_Input;
    protected CharacterController m_Controller;
    public abstract void Use_Skill(string _SkillName);

    protected abstract IEnumerator Corutine_Skill(string _SkillName);

 
    
  

}
