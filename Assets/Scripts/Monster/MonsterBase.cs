using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class MonsterBase : MonoBehaviour, IMove, IDamage, IStatus
{

    // Component
    protected NavMeshAgent m_Agent;
    protected Animator m_Anim;
    protected Player m_Player;
    protected Collider m_Colider;
    //protected AudioSource m_AudioSouce;

    [Header("Material")]
    [SerializeField] public Material m_ToonMaterial;
    private float m_ChangeDuration = 0.5f; // ȿ�����ӽð�
    private float m_StayDuration = 0.1f; // ���������� �����Ǵ� �ð�
    private float m_OriginRimLightValue = 1f; // ���� RimLight��
    private float m_TargetValue = 0.01f; // �ǰݽ� ��ȭ�ϴ� RimLight��
    private bool m_isRimLight = false;
    public bool m_isCritical = false;
    

    [Header("UI")]
    [SerializeField] public GameObject m_Canvas;
    [SerializeField] public Image m_HpBar;

    [Header("Boss_UI")]
    [SerializeField] protected bool m_BossCheck = false;
    [SerializeField] protected Image m_BossHpFliiBar;
    [SerializeField] protected TextMeshProUGUI m_TestBossName;
    [SerializeField] protected TextMeshProUGUI m_TestBossAlias;
    [SerializeField] protected TextMeshProUGUI m_TestBossHp;
    [SerializeField] protected float m_LerpSpeed = 2;
    private float m_LerpedHealth;

    [Header("Status")]
    [SerializeField] protected float MaxHp = 10;
    [SerializeField] protected float CurrentHp;
    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected float m_RotationSpeed;
    [SerializeField] protected int Defence = 0;
    [SerializeField] protected float MinDamage;
    [SerializeField] protected float MaxDamage;
    [SerializeField] protected int m_Exp;
    [SerializeField] protected float CriticalDamage;
    [SerializeField] protected float CriticalProbability;
    [SerializeField] protected int m_DropMoney;
    

    [Header("State")]
    public bool m_isAttack = false;
    public bool m_isStagger = false;
    public bool m_isDead = false;
    public bool m_isSpawned = false;
    public bool IsMove = false;
    public bool m_isIdle = false;
    public bool m_isRandomMove = true;
    public bool m_NotStagger = false;
    public bool m_isSturn = false;



    [Header("Attack")]
    public bool m_CanAttack = true;
    public float m_AttackTime = 0.84f;
    public float m_AttackCoolTime = 2f;

    [Header("State Time")]
    [SerializeField] private float m_DeadTime = 3f;
    [SerializeField] private float m_StaggerTime = 0.3f;
    [SerializeField] protected float m_SpawnTime = 1.0f;

    [Header("Aim")]
    [SerializeField] private Transform m_AimAssist;
    private float m_CurrentHorizontal;
    private float m_CurrentVertical;
    private float m_VerticalVelocity;
    private float m_HoriznontalVelocity;
    [SerializeField] private float m_SmothTime = 0.5f;
    [SerializeField] private float m_AimRotSpeed = 0.5f;


    [Header("AIMove")]
    [SerializeField] private float m_RandomMoveRadius = 20f;
    [SerializeField] private float m_RandomMoveTimer = 5f;
    [SerializeField] private float m_Timer;
    [SerializeField] private float m_Distance;
    [SerializeField] private bool m_isFirstMove = true;

    [Header("Collider")]
    [SerializeField] protected GameObject m_HitBoxColiider;
    [SerializeField] private GameObject m_AttackHitBox;

    [Header("Sturn")]
    public bool m_SturnMonster = false;
    protected bool m_StateSturn = false;
    [SerializeField] protected float m_SturnTime = 3f;
    protected float m_SturnTimer = 0f;
    [SerializeField] protected ParticleSystem m_SturnVfx;


    private bool m_GetExpCheck = false;
    public float m_MoveSpeed { get => MoveSpeed; set => MoveSpeed = value; }

    public bool m_isMove  {get => IsMove; set => IsMove = value;}
    public float m_MaxHp { get => MaxHp; set => MaxHp = value; }
    public float m_CurrentHp { get => CurrentHp; set => CurrentHp = value; }
    public int m_Defence { get => Defence; set => Defence = value; }
    public float m_MinDamage { get => MinDamage; set => MinDamage = value; }
    public float m_MaxDamage { get => MaxDamage; set => MaxDamage = value; }
    public float m_CriticalDamage { get => CriticalDamage; set => CriticalDamage = value; }
    public float m_CriticalProbability { get => CriticalProbability; set => CriticalProbability = value; }

    protected virtual void Start()
    {
        m_ToonMaterial = GetComponentInChildren<Renderer>().material;
        m_Agent = GetComponent<NavMeshAgent>();
        m_Anim = GetComponent<Animator>();
        m_Player = FindAnyObjectByType<Player>();
        m_Colider = GetComponent<Collider>();
        //m_AudioSouce = GetComponent<AudioSource>();
        Initialize_HP();
        Initialize_ToonShader();
        if (m_AttackHitBox != null)
        {
            m_AttackHitBox.SetActive(false);
        }


    }

    // Update is called once per frame
    protected virtual void Update()
    {

        Update_HP();
    }

    protected bool Can_MeleeAttack()
    {
        bool IsRange = Vector3.Distance(transform.position, m_Player.transform.position) <= m_Agent.stoppingDistance;

        return IsRange;
    }

    protected bool Can_ThrowAttack(float _Min, float _Max)
    {
        float Distance = Player_Distance();
        // �÷��̾���� �Ÿ��� 5����ũ�� �÷��̾���� �Ÿ��� 20���� ������
        bool IsRange = Distance >= _Min && Distance <= _Max;

        return IsRange;
    }

  
    protected void SelectMove_AI()
    {
        float Distance = Player_Distance();

        
        if (Distance < 15f) // �÷��̾ ������������ �߰ݽ���
        {
            m_isMove = true;
        }

        else if (Distance > 15f && Distance < 30f) // �߰� �Ÿ��� �� �߰� ����
        {
            m_isMove = false;
        }

        else if (Distance >= 30f) // �ʹ� �־����� �ٽ� �߰�
        {
            m_isMove = true;
        }

        if (m_isMove)
        {
            m_isRandomMove = false;
            Chase_Player();
        }

        else if (!m_isMove)
        {
            Random_Move();
        }
    }

    protected void Chase_Player()
    {
        if (m_Player != null && !m_isDead && !m_isStagger)
        {
            m_Agent.speed = m_MoveSpeed;
            MoveAnim_Controll();
            m_Agent.SetDestination(m_Player.transform.position);
            LookAt_Player();
           
        }
    }

    protected void Random_Move()
    {
        m_Timer += Time.deltaTime;
        m_Agent.speed = m_MoveSpeed;
        

        if (m_isFirstMove || m_Timer >= m_RandomMoveTimer)
        {
            Vector3 RandomDirection = Random.insideUnitSphere * m_RandomMoveRadius;
            RandomDirection += transform.position; // ���� ��ġ�� �������� ������ ���� ����

            NavMeshHit Hit;
            NavMesh.SamplePosition(RandomDirection, out Hit, m_RandomMoveRadius, 1);
            Vector3 FinalPosition = Hit.position;

            m_Agent.SetDestination(FinalPosition);
            m_Timer = 0;

            m_isFirstMove = false;
        }

        MoveAnim_Controll();
    }

    protected void Initialize_HP()
    {
        m_CurrentHp = m_MaxHp;
    }

    protected void Update_HP()
    {
      
        if (m_HpBar != null)
        {
            m_HpBar.fillAmount = m_CurrentHp / m_MaxHp;
        }

        if (m_BossHpFliiBar != null)
        {
            m_TestBossHp.text = $"{m_CurrentHp} / {m_MaxHp}";
            m_LerpedHealth = Mathf.Lerp(m_LerpedHealth, m_CurrentHp, Time.deltaTime * m_LerpSpeed);
            m_BossHpFliiBar.fillAmount = m_LerpedHealth / m_MaxHp;
        }

        Dead_Check();

      
    }

 

    protected void Aim_Anim()
    {
        
        // �÷��̾� �������� AimAssist�� �ε巴�� ȸ��
        Vector3 directionToPlayer = m_Player.transform.position - m_AimAssist.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // ȸ�� �ӵ��� �ϰ��ǰ� ���߱� ���� �Ӹ� ȸ������ Slerp ����
        m_AimAssist.transform.rotation = Quaternion.Slerp(
            m_AimAssist.transform.rotation,
            targetRotation,
            Time.deltaTime * m_AimRotSpeed // �Ӹ� ȸ�� �ӵ�
        );

        // AimAssist�� ���� Y�� ȸ���� ��������
        float YRotation = m_AimAssist.transform.localEulerAngles.y;
        float HorizontalAngle;

        // Y�� ȸ������ -180 ~ 180 ������ ��ȯ
        if (YRotation > 180)
        {
            HorizontalAngle = YRotation - 360;
        }
        else
        {
            HorizontalAngle = YRotation;
        }

        // -45 ~ 45 ������ -1 ~ 1�� ��ȯ
        float NormalizeHorizontal = (Mathf.InverseLerp(-45f, 45f, HorizontalAngle) * 2 - 1);

        // X�� ȸ���� �������� (���� X�� ȸ������ -180 ~ 180���� ��ȯ)
        float XRotation = m_AimAssist.rotation.eulerAngles.x;
        if (XRotation > 180)
        {
            XRotation -= 360;
        }

        // -30 ~ 40 ������ -1 ~ 1�� ��ȯ�ϰ� ���� ����
        float verticalAngle = (Mathf.InverseLerp(-30f, 40f, XRotation) * 2 - 1) * -1;

        // �ִϸ��̼��� �ε巯�� ��ȯ�� ���� ���� SmoothDamp ���
        m_CurrentHorizontal = Mathf.SmoothDamp(m_CurrentHorizontal, NormalizeHorizontal, ref m_HoriznontalVelocity, m_SmothTime);
        m_CurrentVertical = Mathf.SmoothDamp(m_CurrentVertical, verticalAngle, ref m_VerticalVelocity, m_SmothTime);

        // �ִϸ��̼ǿ� �� ����
        m_Anim.SetFloat("Aim_X", m_CurrentHorizontal);
        m_Anim.SetFloat("Aim_Y", m_CurrentVertical);
    }

    protected void Set_AnimLayerWeight(int _LayerNum, float _Weight)
    {
        m_Anim.SetLayerWeight(_LayerNum, _Weight);
    }

    protected void LookAt_Player()
    {
        Vector3 Direction = (m_Player.transform.position - transform.position).normalized;
        Quaternion LookRotation = Quaternion.LookRotation(Direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, Time.deltaTime * m_RotationSpeed);
    }

    protected void MoveAnim_Controll()
    {
        if (m_Agent.velocity.magnitude > 0.1f)
        {
            m_Anim.SetBool("Move", true);
        }
        else if (m_Agent.velocity.magnitude < 0.1f)
        {
            m_Anim.SetBool("Move", false);
        }


    }

    protected float Player_Distance()
    {
        return m_Distance = Vector3.Distance(transform.position, m_Player.transform.position);
    }

    protected void Initialize_ToonShader()
    {
        StopAllCoroutines();
        m_ToonMaterial.SetColor("_RimLightColor", new Color(0.77f, 0.47f, 0f));
        m_ToonMaterial.SetFloat("_RimLight_InsideMask", 1f);
    }

    IEnumerator Corutine_Dle()
    {
        m_isDead = true;
        StopCoroutine(Corutine_ChangeRimLightPower());
        m_ToonMaterial.SetFloat("_RimLight_InsideMask", 1f);
        m_Agent.speed = 0;
        m_HitBoxColiider.SetActive(false);
        m_Anim.SetTrigger("Dead");
        m_Colider.enabled = false;
        if (!m_GetExpCheck)
        {
            m_Player.Gain_Exp(m_Exp);
            m_Player.Money_Change(m_DropMoney);
            Debug.Log("���� ���� ����ġ ���� : " + m_Exp);
            m_GetExpCheck = true;
        }

        
        yield return new WaitForSeconds(m_DeadTime);
        Destroy(gameObject);
    }

    IEnumerator Corutine_Stagger()
    {
        m_isStagger = true;
        m_Agent.speed = 0;
        
        yield return new WaitForSeconds(m_StaggerTime);
        
        m_isStagger = false;
        m_Agent.speed = m_MoveSpeed;

    }

    protected IEnumerator Corutine_Attack()
    {
        // �ǰ� Color�� 0.77, 0.47, 0, 1
        m_CanAttack = false;
        m_isAttack = true;

        m_ToonMaterial.SetColor("_RimLightColor", new Color(1f, 0.32f, 0.25f));
        
        StopCoroutine(Corutine_ChangeRimLightPower());
        StartCoroutine(Corutine_ChangeRimLightPower());
      
        m_Agent.speed = 0;
        this.transform.LookAt(m_Player.transform);
        m_Anim.SetTrigger("Attack");
      

        yield return new WaitForSeconds(m_AttackTime);
        
        m_ToonMaterial.SetColor("_RimLightColor", new Color(0.77f, 0.47f, 0f));
        m_isAttack = false;
        m_Agent.speed = m_MoveSpeed;

        yield return new WaitForSeconds(m_AttackCoolTime);
        m_CanAttack = true;
    }

    protected IEnumerator Corutine_Spawn()
    {
        m_Anim.SetTrigger("Spawn");
        yield return new WaitForSeconds(m_SpawnTime);
        m_isSpawned = true;
        m_HitBoxColiider.SetActive(true);
    }

    protected IEnumerator Corutine_ChangeRimLightPower()
    {
        m_isRimLight = true;
        // Oringin = 1 ���� TargetValue 0���� RimLight�� ��ȭ
        yield return StartCoroutine(Corutine_LerpRimLight(m_OriginRimLightValue, m_TargetValue, m_ChangeDuration));

        // TargetValue���� ���� �ð� ���
        yield return new WaitForSeconds(m_StayDuration);

        // �ٽ� RimLightPower ������� ����
        yield return StartCoroutine(Corutine_LerpRimLight(m_TargetValue, m_OriginRimLightValue, m_ChangeDuration));

        m_isRimLight = false;
    }

    private IEnumerator Corutine_LerpRimLight(float _StartValue, float _EndValue, float _Duration)
    {
        float elapsed = 0f;

        while (elapsed < _Duration)
        {
            // ���൵ ��� 0~1
            float t = elapsed / _Duration;

            float newValue = Mathf.Lerp(_StartValue, _EndValue, t);
            m_ToonMaterial.SetFloat("_RimLight_InsideMask", newValue);

            // ����ð� ����
            elapsed += Time.deltaTime;

            // ���� �����ӱ��� ���
            yield return null;
        }

        // ������ ����
        m_ToonMaterial.SetFloat("_RimLight_InsideMask", _EndValue);
    }

    public void Move()
    {
        throw new System.NotImplementedException();
    }

    public void Take_Damage(float _Damage)
    {

        float Damage = _Damage - m_Defence;

        if (Damage < 0f)
        {
            Damage = 0f;
        }
        m_CurrentHp -= Damage;

        Vector3 RandomPos = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0f, 0.5f), 0f);

        if (!m_isCritical)
        {
            DamagePopUp.m_Instace.CreatePopUp(transform.position + RandomPos, Damage.ToString(), Color.white);
        }

        else
        {
            Debug.Log("ũ��Ƽ��!");
            AudioManager.Instance.Random_SoundOnShot(gameObject, 101, 3);
            DamagePopUp.m_Instace.CreatePopUp(transform.position + RandomPos, Damage.ToString(), Color.red);
            m_isCritical = false;
        }

        Audio_HitCheck();

        StopCoroutine(Corutine_ChangeRimLightPower());

        if (!m_isAttack && !m_isRimLight)
        {
            StartCoroutine(Corutine_ChangeRimLightPower());
        }

        if (!m_BossCheck || !m_NotStagger)
        {
            if (m_CurrentHp > 0 && !m_isAttack)
            {
                if (m_NotStagger)
                    return;

                int Temp = Random.Range(1, 11);

                switch (Temp)
                {
                    case 1:
                        m_Anim.SetTrigger("Hurt1");
                        StartCoroutine(Corutine_Stagger());
                        break;
                    case 2:
                        m_Anim.SetTrigger("Hurt2");
                        StartCoroutine(Corutine_Stagger());
                        break;
                    default:
                        return;
                }
            }
        }

        
            Anim_AttackHitBox_Off();
        
    }

    public void Dead_Check()
    {
        if (m_CurrentHp <= 0)
        {
            Anim_AttackHitBox_Off();
            m_Canvas.SetActive(false);

            if (m_SturnMonster)
            {
                m_isSturn = false;
                m_Anim.SetBool("Sturn", false);
            }
            
            StartCoroutine(Corutine_Dle());
            return;
        }
    }

    public void Damage_Up()
    {
        ((IStatus)this).Damage_Up();
    }

    public float Calculate_Damage()  
    {
        float Damage = Random.Range(m_MinDamage, m_MaxDamage);

     
        // �������� ������ �ݿø��Ͽ� ����
        return Mathf.RoundToInt(Damage);

    }

    protected void Initialize_BossHpUI(string _Name, string _Alias)
    {
        m_TestBossName.text = _Name;
        m_TestBossAlias.text = _Alias;
    }

   public virtual void Anim_AttackHitBox_On()
    {
        m_AttackHitBox.SetActive(true);
    }

   public virtual void Anim_AttackHitBox_Off()
    {
        if (m_AttackHitBox != null)
        {
            if (m_AttackHitBox.activeSelf == true)
            {
                m_AttackHitBox.SetActive(false);
            }
        }
    }


   

    protected void State_Sturn()
    {
        if (m_isSturn)
        {
            m_SturnTime -= Time.deltaTime;

            if (m_SturnTime <= 0)
            {
                m_isSturn = false;
            }
        }
    }

    protected void SturnCheck()
    {
        if (m_isSturn)
        {
            m_SturnTimer -= Time.deltaTime;

            if (m_SturnTimer <= 0f)
            {
                m_isSturn = false;
                OnSturnEnd();
            }

            return;
        }
    }

    public void Sturn(float _Duration)
    {
        m_isSturn = true;
        m_SturnTime = _Duration;
        m_SturnTimer = _Duration;
        m_StateSturn = true;
    }

   

    private void OnSturnEnd()
    {
        Debug.Log(this.gameObject.name + "������ �������ϴ�.");
        m_Anim.SetBool("Sturn", false);
    }

    private void Audio_HitCheck()
    {
        if (gameObject.GetComponent<Monster_Beetle>() != null)
        {
            AudioManager.Instance.Random_SoundOnShot(gameObject, 90, 3);
        }

        if (gameObject.GetComponent<Monster_Golem>() != null)
        {
            AudioManager.Instance.Random_SoundOnShot(gameObject, 93, 3);
        }

        if (gameObject.GetComponent<Monster_Lemurian>() != null)
        {
            AudioManager.Instance.Random_SoundOnShot(gameObject, 96, 3);
        }
    }

}
