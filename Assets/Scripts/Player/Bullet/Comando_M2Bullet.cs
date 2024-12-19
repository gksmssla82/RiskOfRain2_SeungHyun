using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comando_M2Bullet : Bullet
{
    // 기본배율 150% 관통시 20%데미지 증가 최대 6회관통 최대배율 270%

    [SerializeField] private Transform m_VfxM2BulletHit;
    [SerializeField] private Transform m_VfxM2Move;
    private float m_VfxMoveTime  = 0.3f;
    private float m_Timer = 0;
    private int m_PassCount = 0; // 몬스터를 통과한 횟수
    private bool m_BulletHit = false;

    protected override void Update()
    {

        TimeOut_BulletDeActive("Bullet_M2");

        m_Timer += Time.deltaTime;

        if (m_Timer >= m_VfxMoveTime)
        {
            PoolManager.Instance.ActivateParticle("Vfx_M2BulletMove", transform.position, Quaternion.identity);
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
                // 체력바 활성화
                Monster.m_Canvas.SetActive(true);

                // 데미지 계산 / 크리티컬 계산
                float Damage = Calculate_BulletDamage(Monster);

                // 관통 데미지 계산
                float PieiceDamge = Bullet_Pass_DamageUp(Damage);

                // 데미지 적용
                Monster.Take_Damage(PieiceDamge);

                // Hit이펙트
                Instantiate(m_VfxM2BulletHit, transform.position, Quaternion.identity);

                m_PassCount++;

                // 6번이상 통과시 총알삭제
                if (m_PassCount >= 6)
                {
                    PoolManager.Instance.DeactivateObject("Bullet_M2", this.gameObject);
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
       
        float damage = _Damage * 1.5f; // 기본데미지 배율 150;
        float passDamage = 1f + (0.2f * m_PassCount); // 패스시 20%씩 추가
        float finaldamage = damage * passDamage;

        return Mathf.RoundToInt(finaldamage);
    }

}
