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
    //    // ũ�ν����
    //    Change_CrossHair(false, true);
    //    // ���콺 �ΰ���
    //    m_Controller.Set_Sensitivity(m_NormalSenstivity);
    //    // ī�޶� Ű���� �Է¹������� ���ϴ°� ����
    //    m_Controller.Set_RotateOnMove(false);

    //    // ������Ʈ���̸� ������Ʈ ����
    //    if (m_Input.sprint)
    //    {
    //        m_Input.sprint = false;
    //    }

    //    Vector3 WorldAimTarget = m_MouseWorldPos;
    //    WorldAimTarget.y = transform.position.y;
    //    Vector3 AimDirection = (WorldAimTarget - transform.position).normalized;

    //    // Shot�ִϸ��̼� Ȱ��ȭ
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
