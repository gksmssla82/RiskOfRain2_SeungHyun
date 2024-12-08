using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Ward : MonsterBase
{
    private Rigidbody m_Rigid = null;
    [SerializeField] private float m_Speed = 0f;
    [SerializeField] private float m_CurrentSpeed = 0f;
    [SerializeField] private float m_Amplitude = 0.5f; // 如甸覆 农扁
    [SerializeField] private float m_Frequency = 2f; // 如甸覆 后档
    [SerializeField] private float m_StopDistance = 2f;
    [SerializeField] private Material m_Material;
    [SerializeField] private float m_EmissiveSpeed = 1f;
    [SerializeField] private float m_intensitiy = 0f;
    [SerializeField] private GameObject m_Boom;
    [SerializeField] private Transform m_ExplosionVfx;
    [SerializeField] private ParticleSystem m_SpawnVfx;
    private Color m_initialColor;
  
    private bool m_Spawned = false;
    private bool m_isChasing = true;
    private bool m_isEmission = false;

    private Vector3 m_InitializePos;
    
                                                      
    protected override void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Player = FindAnyObjectByType<Player>();
        m_Rigid = GetComponent<Rigidbody>();
        m_Material = m_Boom.GetComponent<Renderer>().material;


        Set_MatColor();
        Initialize_HP();
        Initialize_ToonShader();
        

        StartCoroutine(Corutine_Spawn());
    }

    // Update is called once per frame
    protected override void Update()
    {
        
        if (m_Player != null && m_Spawned && m_isChasing)
        {
            if (Player_Distance() > m_StopDistance)
            {
                Vector3 dir = (m_Player.transform.position - transform.position).normalized;
                Vector3 offset = new Vector3(0, Mathf.Sin(Time.time * m_Frequency) * m_Amplitude, 0);

                transform.position += (dir * m_Speed * Time.deltaTime) + offset;
                transform.LookAt(m_Player.transform);
            }

            else
            {
                StartCoroutine(Corutine_Explosion());
            }
        }

        if (m_HpBar != null)
        {
            m_HpBar.fillAmount = m_CurrentHp / m_MaxHp;
        }

        if (m_CurrentHp <= 0)
        {
            StartCoroutine(Corutine_Explosion());
        }

        if (m_isEmission)
        {
            Emission_Color();
        }

    }

    private IEnumerator Corutine_Spawn()
    {
       
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, 2f, 0);

        float time = 0;
        m_SpawnVfx.Play();
        while (time < 1.5f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, time / 1.5f);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        m_Anim.SetBool("Idle", true);

        yield return new WaitForSeconds(2.5f);
        
        m_Spawned = true;

        AudioManager.m_Instnace.Play(gameObject, "BettleWard_Idle");
    }

    private IEnumerator Corutine_Explosion()
    {
        m_HitBoxColiider.SetActive(false);
        m_isEmission = true;
        m_isChasing = false;
        AudioManager.m_Instnace.Play(gameObject, "BettleWard_ExplosionStart");
        yield return new WaitForSeconds(2f);

        Debug.Log("Ward 气惯");
        AudioManager.m_Instnace.Play_newObject(gameObject, "BettleWard_Explosion");
        Instantiate(m_ExplosionVfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Emission_Color()
    {
        m_intensitiy += m_EmissiveSpeed * Time.deltaTime;
        Color startColor = new Color(191f, 9f, 1f);


        m_Material.SetColor("_Emissive_Color", startColor);
        m_initialColor = m_Material.GetColor("_Emissive_Color");
        Color color = m_initialColor * m_intensitiy;
        m_Material.SetColor("_Emissive_Color", color);



    }

    private void Set_MatColor()
    {
        m_initialColor = m_Material.GetColor("_Emissive_Color");
        m_Material.SetColor("_Emissive_Color", Color.black); // Emission Black
    }
}
