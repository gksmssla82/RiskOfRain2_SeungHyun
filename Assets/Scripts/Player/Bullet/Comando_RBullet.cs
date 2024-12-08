using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comando_RBullet : Bullet
{
    [SerializeField]
    private Transform m_VfxRBulletHit;

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster_Hitbox"))
        {

            MonsterBase Monster = other.gameObject.GetComponentInParent<MonsterBase>();

            if (Monster != null)
            {
                // 체력바 활성화
                Monster.m_Canvas.SetActive(true);

                // 데미지 계산 / 크리티컬 계산
                float Damage = Calculate_BulletDamage(Monster);

                // 데미지 적용
                Monster.Take_Damage(Damage);

                // Hit이펙트
                Instantiate(m_VfxRBulletHit, transform.position, Quaternion.identity);

                // 스턴 몬스터 체크
                if (Monster.m_SturnMonster && !Monster.m_isSturn)
                {
                    Monster.m_isSturn = true;
                }

                // 총알 삭제
                Destroy_Bullet();


            }
        }

    }

}
