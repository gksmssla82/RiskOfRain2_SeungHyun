using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comando_M1Bullet : Bullet
{
    [SerializeField]
    private Transform m_VfxNormalBulletHit;

    protected override void Update()
    {
        base.Update();
        //Debug.Log("총알 최소 데미지 :" + m_MinDamage);
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
                Instantiate(m_VfxNormalBulletHit, transform.position, Quaternion.identity);

                // 총알 삭제
                //Destroy_Bullet();
                ActiveFalse_Bullet();
            }
        }

        else if (other.CompareTag("Object"))
        {
            // Hit이펙트
            Instantiate(m_VfxNormalBulletHit, transform.position, Quaternion.identity);

            // 총알 삭제
            //Destroy_Bullet();
            ActiveFalse_Bullet();
        }
    }

}
