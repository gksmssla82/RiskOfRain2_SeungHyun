using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comando_M1Bullet : Bullet
{
    [SerializeField]
    private Transform m_VfxNormalBulletHit;

    protected override void Update()
    {
        TimeOut_BulletDeActive("Bullet_Normal");
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
                Instantiate(m_VfxNormalBulletHit, transform.position, Quaternion.identity);

                // �Ѿ� ����
                //Destroy_Bullet();
                DeActivate_Bullet("Bullet_Normal");
            }
        }

        else if (other.CompareTag("Object"))
        {
            // Hit����Ʈ
            Instantiate(m_VfxNormalBulletHit, transform.position, Quaternion.identity);

            // �Ѿ� ����
            //Destroy_Bullet();
            DeActivate_Bullet("Bullet_Normal");
        }
    }

}
