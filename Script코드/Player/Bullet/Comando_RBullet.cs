using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comando_RBullet : Bullet
{
    [SerializeField]
    private Transform m_VfxRBulletHit;

    protected override void Update()
    {
        TimeOut_BulletDeActive("Bullet_R");
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster_Hitbox"))
        {

            MonsterBase Monster = other.gameObject.GetComponentInParent<MonsterBase>();

            if (Monster != null)
            {
                // ü�¹� Ȱ��ȭ
                Monster.m_Canvas.SetActive(true);

                // ������ ��� / ũ��Ƽ�� ���
                float Damage = Calculate_BulletDamage(Monster);

                // ������ ����
                Monster.Take_Damage(Damage);

                // Hit����Ʈ
                Instantiate(m_VfxRBulletHit, transform.position, Quaternion.identity);

                // ���� ���� üũ
                if (Monster.m_SturnMonster && !Monster.m_isSturn)
                {
                    Monster.m_isSturn = true;
                }

                // �Ѿ� ����
                DeActivate_Bullet("Bullet_R");


            }
        }

    }

}
