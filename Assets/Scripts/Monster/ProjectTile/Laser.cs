using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private Transform m_VfxHitLaser;
    public Transform m_AimPoint;
    public Transform m_LaserOrigin;
    public LineRenderer m_WaringLaser;
    public LineRenderer m_RealLaser;
    public ParticleSystem m_WaringParticle;
    public float m_WarringTime = 3f;
    public float m_RealTime = 2f;
    public float m_LaserRange = 2f;
    public Vector3 m_TargetPoint = Vector3.zero;
    public LayerMask m_HitLayer;
    public Material m_LaserMat;
    private Animator m_Anim;

    private bool m_isFire = false;
    private bool m_isCanFire = true;
    private float m_FireCoolTime = 5f;
    private bool m_DamgeApplied = false;

    

    // Start is called before the first frame update
    void Start()
    {
        m_Anim = GetComponentInParent<Animator>();
      
        m_LaserMat = m_RealLaser.GetComponent<LineRenderer>().material;
       
    }

  


    public bool isCanFire_Check()
    {
        return m_isCanFire;
    }

    public void Start_Laser()
    {
        StartCoroutine(Fire_Laser());
    }

  

    IEnumerator Fire_Laser()
    {
        
        m_isCanFire = false;
        // �߻������� üũ
        m_isFire = true;
        // ��� ��ƼŬ ���
        m_WaringParticle.Play();
        AudioManager.Instance.PlayOneShot(gameObject,"Golem_LazerCharge");


        // ������ ���� ��ġ
        Vector3 targetPosition = Vector3.zero;

        // Ray Hit
        RaycastHit hit;

        // ��� Ÿ�̸�
        float WaringTimer = 0f;

        // ������� �ð�
        while (WaringTimer < m_WarringTime)
        {
            // ���׸��� Dessolve�ʱ�ȭ
            m_LaserMat.SetFloat("_DessolveAmount", 0f);
            // ��������Ʈ�� �չ�������
            Vector3 Direction = m_AimPoint.forward;
            Vector3 StartPoint = m_LaserOrigin.position;

            if (Physics.Raycast(StartPoint, Direction, out hit, m_LaserRange, m_HitLayer))
            {
                // �������� � ��ü�� �浹�ߴٸ�, �� ������ �������� ��
                m_TargetPoint = hit.point;
            }

            // ��� ������ ���η����� ����
            m_WaringLaser.SetPosition(0, StartPoint);
            m_WaringLaser.SetPosition(1, m_TargetPoint);

            // ��� ������ ����
            m_WaringLaser.enabled = true;

           

            // Ÿ�̸� ������Ʈ
            WaringTimer += Time.deltaTime;

            yield return null;
        }

        m_WaringLaser.enabled = false;

        ////////////////////////// ��� ������ �� ////////////////////////


        ////////////////////////// ���� ������ ///////////////////////////

        
        float RealTimer = 0f;
        bool IsDameged = false;
        AudioManager.Instance.PlayOneShot(gameObject,"Golem_LazerFire");



        while (RealTimer < m_RealTime)
        {
            Vector3 StartPoint = m_LaserOrigin.position;

            m_RealLaser.SetPosition(0, StartPoint);
            m_RealLaser.SetPosition(1, m_TargetPoint);

            m_RealLaser.enabled = true;

            if (m_LaserMat.GetFloat("_DessolveAmount") < 1f)
            {
                m_LaserMat.SetFloat("_DessolveAmount", RealTimer);
            }

            if (!IsDameged && Physics.Raycast(StartPoint, (m_TargetPoint - StartPoint).normalized, out hit, m_LaserRange, m_HitLayer))
            {
                if (hit.collider != null && hit.collider.CompareTag("Player_Hitbox"))
                {
                    FindAnyObjectByType<Player>().Take_Damage(GetComponentInParent<Monster_Golem>().Calculate_Damage());
                    Instantiate(m_VfxHitLaser, hit.transform.position, Quaternion.identity);
                    IsDameged = true;
                }
            }

            RealTimer += Time.deltaTime;
           

            yield return null;
        }
       
        m_RealLaser.enabled = false;
        m_isFire = false;

        yield return new WaitForSeconds(m_FireCoolTime);

        m_isCanFire = true;

    }
}
