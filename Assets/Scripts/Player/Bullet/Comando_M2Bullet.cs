using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comando_M2Bullet : Bullet
{
    // �⺻���� 150% ����� 20%������ ���� �ִ� 6ȸ���� �ִ���� 270%

    [SerializeField] private Transform m_VfxM2BulletHit;
    [SerializeField] private Transform m_VfxM2Move;
    private float m_VfxMoveTime  = 0.3f;
    private float m_Timer = 0;
    private int m_PassCount = 0; // ���͸� ����� Ƚ��
    private bool m_BulletHit = false;

    protected override void Update()
    {
        m_Timer += Time.deltaTime;

        if (m_Timer >= m_VfxMoveTime)
        {

            GameObject moveVfx = PoolManager.m_Instance.Activate_Object(0);
            PoolManager.m_Instance.Set_ObjPosition(moveVfx, this.transform);
            m_Timer = 0;
        }
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster_Hitbox") && !m_BulletHit)
        {
            m_BulletHit = true;

            MonsterBase Monster = other.gameObject.GetComponentInParent<MonsterBase>();

            if (Monster != null)
            {
                // ü�¹� Ȱ��ȭ
                Monster.m_Canvas.SetActive(true);

                // ������ ��� / ũ��Ƽ�� ���
                float Damage = Calculate_BulletDamage(Monster);

                // ���� ������ ���
                float PieiceDamge = Bullet_Pass_DamageUp(Damage);

                // ������ ����
                Monster.Take_Damage(PieiceDamge);

                // Hit����Ʈ
                Instantiate(m_VfxM2BulletHit, transform.position, Quaternion.identity);

                m_PassCount++;

                // 5������� �Ѿ˻���
                if (m_PassCount >= 6)
                {
                    Destroy_Bullet();
                }
                else
                {
                    m_BulletHit = false;
                }
            }
        }

       
    }

    private float Bullet_Pass_DamageUp(float _Damage)
    {
       
        float damage = _Damage * 1.5f; // �⺻������ ���� 150;
        float passDamage = 1f + (0.2f * m_PassCount); // �н��� 20%�� �߰�
        float finaldamage = damage * passDamage;

        return Mathf.RoundToInt(finaldamage);
    }

}
