using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
public abstract class Skill 
{
    protected Player m_Player;
    public bool m_CanUse { get; set; } = true; // 사용가능한지?
    public bool m_IsUsing { get; set; } = false; // 사용중인지
    public float m_CooldownTime { get; set; } // 쿨타임 시간
    public float m_Duration { get; set; } // 지속 시간
    public float m_LastUseTime { get; set; }
    public Image m_Icon { get; set; } // 스킬 아이콘

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
