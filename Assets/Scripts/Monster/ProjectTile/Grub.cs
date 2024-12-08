using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grub : Bullet
{
    [SerializeField] private float m_BoomTime = 3f;
    [SerializeField] private float m_BoomDamage = 20f;
    [SerializeField] private Transform m_VfxExplosion;
    private bool m_isTurn = false;
    [SerializeField] private float m_RotationSpeed = 0f;
    [SerializeField] private float m_AccSpeed = 1f;
    private SphereCollider m_Colider;


    protected override void Awake()
    {
        m_Player = FindAnyObjectByType<Player>();
        m_Colider = GetComponent<SphereCollider>();
    }
    void Start()
    {
        m_Colider.enabled = false;
        StartCoroutine(Corutine_Grub());
    }

    
    void Update()
    {
        if (m_isTurn)
        {
            m_RotationSpeed += m_AccSpeed * Time.deltaTime;

            transform.Rotate(0, m_RotationSpeed * Time.deltaTime, 0, Space.World);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player_Hitbox"))
        {
            m_Player.Take_Damage(m_BoomDamage);
            Destroy_Bullet();
        }
    }

    private IEnumerator Corutine_Grub()
    {
        yield return new WaitForSeconds(1f);
        m_isTurn = true;
        yield return new WaitForSeconds(m_BoomTime);

        Instantiate(m_VfxExplosion, transform.position, Quaternion.identity);
        m_Colider.enabled = true;

        yield return new WaitForSeconds(0.2f);
        AudioManager.m_Instnace.Play_newObject(gameObject, "Grub_Explosion");
        Destroy_Bullet();

    }
}
