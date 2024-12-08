using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : Skill
{
    private float m_SlideSpeed = 8;

    public Sliding(Player _Player, float _SlideSpeed, float _Duration, float _CoolTime, Skill_CoolDown _CoolDownICon)
        : base(_Player)
    {
        this.m_SlideSpeed = _SlideSpeed;
        this.m_Duration = _Duration;
        this.m_CooldownTime = _CoolTime;
        this.m_CoolDownIcon = _CoolDownICon;
    }

  

    public override void Use_Skill(string _SkillName)
    {
        m_CanUse = m_Player.m_Input.m_Sliding && !m_IsUsing && Time.time >= m_LastUseTime + m_CooldownTime;

        if (m_CanUse)
        {

            m_Player.Start_SkillCoroutine(Corutine_Skill(_SkillName));
        }
    }

    protected override IEnumerator Corutine_Skill(string _SkillName)
    {
        m_CoolDownIcon.Use_Skill(m_CooldownTime, _SkillName);
        m_IsUsing = true;
        m_LastUseTime = Time.time;
        m_Player.m_Anim.SetTrigger(_SkillName);
        AudioManager.m_Instnace.Random_SoundPlay(m_Player.gameObject, 14, 3);
        Vector3 SlideDirection = m_Player.transform.forward;
        float StartTime = Time.time;
        float InitialSlideSpeed = m_SlideSpeed;
      
        if (m_Player.m_Input.sprint)
        {
            m_Player.m_Input.sprint = false;
        }

        while (Time.time < StartTime + m_Duration)
        {
            float elapsed = Time.time - StartTime; // 슬라이딩 경과 시간
            float normalizedTime = elapsed / m_Duration; // 슬라이딩 경과 시간 비율 (0부터 1까지)

            // 선형 보간을 통해 슬라이딩 속도를 점점 줄임
            float currentSlideSpeed = Mathf.Lerp(InitialSlideSpeed, 0, normalizedTime);
            // 이동
            m_Player.m_Controller._controller.Move(SlideDirection * currentSlideSpeed * Time.deltaTime);
            yield return null;
        }

        m_IsUsing = false;
    }

   

   
}
