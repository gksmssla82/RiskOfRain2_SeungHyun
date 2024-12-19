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
        // 발사중인지 체크
        m_isFire = true;
        // 경고 파티클 재생
        m_WaringParticle.Play();
        AudioManager.Instance.PlayOneShot(gameObject,"Golem_LazerCharge");


        // 레이저 끝점 위치
        Vector3 targetPosition = Vector3.zero;

        // Ray Hit
        RaycastHit hit;

        // 경고 타이머
        float WaringTimer = 0f;

        // 경고레이저 시간
        while (WaringTimer < m_WarringTime)
        {
            // 머테리얼 Dessolve초기화
            m_LaserMat.SetFloat("_DessolveAmount", 0f);
            // 에임포인트에 앞방향으로
            Vector3 Direction = m_AimPoint.forward;
            Vector3 StartPoint = m_LaserOrigin.position;

            if (Physics.Raycast(StartPoint, Direction, out hit, m_LaserRange, m_HitLayer))
            {
                // 레이저가 어떤 물체와 충돌했다면, 그 점에서 끝나도록 함
                m_TargetPoint = hit.point;
            }

            // 경고 레이저 라인렌더러 설정
            m_WaringLaser.SetPosition(0, StartPoint);
            m_WaringLaser.SetPosition(1, m_TargetPoint);

            // 경고 레이저 생성
            m_WaringLaser.enabled = true;

           

            // 타이머 업데이트
            WaringTimer += Time.deltaTime;

            yield return null;
        }

        m_WaringLaser.enabled = false;

        ////////////////////////// 경고 레이저 끝 ////////////////////////


        ////////////////////////// 리얼 레이저 ///////////////////////////

        
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
