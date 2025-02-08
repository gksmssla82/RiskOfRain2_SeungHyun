using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2Shoot : Skill
{
    public M2Shoot(Player _Player, float _Duration, float _CoolTime, Skill_CoolDown _SkillCoolDownIcon ) 
        : base(_Player)
    {
        this.m_CoolDownIcon = _SkillCoolDownIcon;
        this.m_Duration = _Duration;
        this.m_CooldownTime = _CoolTime;
    }

    public override void Use_Skill(string _SkillName)
    {
      
    }

    protected override IEnumerator Corutine_Skill(string _SkillName)
    {
        m_CoolDownIcon.Use_Skill(m_CooldownTime, _SkillName);
        //m_CanUse = true;
        //m_IsUsing = true;
        //m_Player.m_Anim.SetTrigger(_SkillName);

        //float ElapsdTime = 0f;
        //float SkillDuration = m_Duration;

        //while (ElapsdTime < SkillDuration)
        //{
        //    Is_Shoot();
        //    ElapsdTime += Time.deltaTime;
        //    yield return null;
        //}
        //m_IsUsing = false;



        //// 쿨타임에서 스킬 지속시간을 뻄
        //yield return new WaitForSeconds(m_CooldownTime - SkillDuration);

        //m_CanUse = false;
        return null;
    }



}
