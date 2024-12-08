using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Bullet
{
    [SerializeField] private float m_Damage = 10f;
    [SerializeField] private Transform m_VfxExplosion;
 

    protected void Start()
    {
        AudioManager.m_Instnace.Random_SoundPlay(gameObject, 44, 3);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player_Hitbox"))
        {
            AudioManager.m_Instnace.Random_SoundPlay(gameObject,55, 3);
            m_Player.Take_Damage(m_Damage);
            Instantiate(m_VfxExplosion, transform.position, Quaternion.identity);
            Destroy_Bullet();
        }
    }
}
