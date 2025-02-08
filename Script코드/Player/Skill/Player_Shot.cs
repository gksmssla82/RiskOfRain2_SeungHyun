using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shot : Skill
{
    public Player_Shot(Player _Player) : base(_Player)
    {
    }

    public override void Use_Skill(string _SkillName)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Corutine_Skill(string _SkillName)
    {
        throw new System.NotImplementedException();
    }

    //private void Is_Shoot()
    //{
    //    // 크로스헤어
    //    Change_CrossHair(false, true);
    //    // 마우스 민감도
    //    m_Controller.Set_Sensitivity(m_NormalSenstivity);
    //    // 카메라 키보드 입력방향으로 향하는거 막기
    //    m_Controller.Set_RotateOnMove(false);

    //    // 스프린트중이면 스프린트 해제
    //    if (m_Input.sprint)
    //    {
    //        m_Input.sprint = false;
    //    }

    //    Vector3 WorldAimTarget = m_MouseWorldPos;
    //    WorldAimTarget.y = transform.position.y;
    //    Vector3 AimDirection = (WorldAimTarget - transform.position).normalized;

    //    // Shot애니메이션 활성화
    //    //Set_AnimLayerWeight(2, 0.5f);


    //    ShootMove();


    //    transform.forward = Vector3.Lerp(transform.forward, AimDirection, Time.deltaTime * 20f);

    //}

    //private void Change_CrossHair(bool _Normal, bool _Shot)
    //{
    //   m_CrossHair_Normal.SetActive(_Normal);
    //    m_CrossHair_Shot.SetActive(_Shot);
    //}


}
