using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_BettleQueen : MonsterBase
{
    [SerializeField] private ParticleSystem m_ParticleRespawn;

    [Header("Setting")]
    [SerializeField] private Canvas m_Canvas_Boss;

    [Header("Acid")]
    [SerializeField] private ParticleSystem[] m_Acid;
    [SerializeField] private ParticleSystem m_ParticleCanWarCry;
    [SerializeField] private bool m_isFireAcid = false;
    [SerializeField] private bool m_CanFireAcid = true;
    [SerializeField] private float m_FireAcidTime = 5;
    [SerializeField] private float m_FireAcidCoolTime = 10;

    [Header("WarCry")]
    private bool m_WarcryHpChecked = false;
    [SerializeField] private bool m_isWarcry = false;
    [SerializeField] private bool m_CanWarcry = false;
    [SerializeField] private float m_WarcryTime = 10;
    [SerializeField] private float m_WarcryCoolTime = 20;
    [SerializeField] private GameObject m_Grub;
    [SerializeField] private Transform[] m_BoomPos;

    [Header("SpawnWard")]
    [SerializeField] private ParticleSystem m_ParticleSpawnWard;
    [SerializeField] private GameObject m_MonsterWard;
    [SerializeField] private bool m_CanSpawnWard = true;
    [SerializeField] private bool m_isSpawnWard = false;
    [SerializeField] private float m_SpawnWardTime = 5;
    [SerializeField] private float m_SpawnWardCoolTime = 12;
    [SerializeField] private int m_WardCount = 0;

    private enum State
    {
        SPAWN,
        IDLE, 
        CHASE_MOVE,
        WARCRY,
        SPIT,
        SPAWN_WARD,
        DEAD
    }

    private State m_CurrentState;


    private void Awake()
    {


        Canvas bossCanvas = GetComponentInChildren<Canvas>();
        if (bossCanvas != null)
        {
            bossCanvas.renderMode = RenderMode.ScreenSpaceCamera; // 또는 WorldSpace

            // "UICamera"라는 이름의 GameObject를 찾습니다.
            GameObject cameraObject = GameObject.Find("UICamera");
            if (cameraObject != null)
            {
                // Camera 컴포넌트를 가져옵니다.
                Camera cameraComponent = cameraObject.GetComponent<Camera>();
                if (cameraComponent != null)
                {
                    bossCanvas.worldCamera = cameraComponent; // Render Camera 설정
                }
                else
                {
                    Debug.LogError("Camera 컴포넌트를 찾을 수 없습니다!");
                }
            }
            else
            {
                Debug.LogError("UICamera라는 이름의 GameObject를 찾을 수 없습니다!");
            }
        }
        else
        {
            Debug.LogError("Canvas를 찾을 수 없습니다!");
        }
        InitializeArray();
    }

    protected override void Start()
    {
        base.Start();
     
        Initialize_BossHpUI("여왕 딱정벌레", "무리의 어미");

        TransitionToState(State.SPAWN);  
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        Aim_Anim();

        Update_State();

        HalfHp_Check();
       
        
    }

    private void Enter_Spawn()
    {
        m_ParticleRespawn.Play();
        AudioManager.Instance.Play(gameObject, "BettleQueen_Spawn");
        AudioManager.Instance.PlayOneShot(gameObject, "BettleQueen_Spawn_VO");
        Set_AnimLayerWeight(1, 0);
        StartCoroutine(Corutine_Spawn());
        
    }

    private void Update_Spawn()
    {
        // 스폰이끝나면 Idle로
        if (m_isSpawned)
        {
            m_Canvas.SetActive(true);
            TransitionToState(State.IDLE);
        }
    }

    private void Enter_Idle()
    {
        AudioManager.Instance.Random_SoundOnShot(gameObject, 119, 3);
        Set_AnimLayerWeight(1, 1);
        m_Agent.speed = 0f;
    }

    private void Update_Idle()
    {
        Debug.Log("Idle상태");

        if (!Can_MeleeAttack() && !m_isFireAcid && !m_isSpawnWard && !m_isWarcry)
        {

            TransitionToState(State.CHASE_MOVE);
        }

        if (m_CanFireAcid)
        {
            TransitionToState(State.SPIT);
        }

        if (m_CanWarcry)
        {
            TransitionToState(State.WARCRY);
        }

        if (m_CanSpawnWard)
        {
            TransitionToState(State.SPAWN_WARD);
        }
    }

    private void Enter_ChaseMove()
    {


    }

    private void Update_ChaseMove()
    {
        Debug.Log("ChaseMove상태");
        SelectMove_AI();

        if (Can_MeleeAttack())
        {
            m_Anim.SetBool("Move", false);
        }

        else
            m_Anim.SetBool("Move", true);


        if (m_CanFireAcid)
        {
            TransitionToState(State.SPIT);
        }

        if (m_CanWarcry)
        {
            TransitionToState(State.WARCRY);
        }

        if (m_CanSpawnWard)
        {
            TransitionToState(State.SPAWN_WARD);
        }

    }

    private void Enter_Spit()
    {
        m_Agent.speed = 0f;

        if (m_isSpawned && !m_isDead && m_CanFireAcid && !m_isFireAcid)
        {
            StartCoroutine(Corutine_FireAcid());
        }
    }

    private void Update_Spit()
    {
        LookAt_Player();

        if (!m_isFireAcid)
        {
            TransitionToState(State.IDLE);
        }
    }

    private void Enter_WarCry()
    {
        m_Agent.speed = 0f;

        if (m_isSpawned && !m_isDead && m_CanWarcry && !m_isWarcry)
        {
            StartCoroutine(Corutine_Warcry());
        }


    }

    private void Update_WarCry()
    {
        if (!m_isWarcry)
        {
            TransitionToState(State.IDLE);
        }
    }

    private void Enter_SpawnWard()
    {
        m_Agent.speed = 0f;

        if (m_isSpawned && !m_isDead && m_CanSpawnWard && !m_isSpawnWard)
        {
            StartCoroutine(Corutine_SpawnWard());
        }
    }

    private void Update_SpawnWard()
    {
        Debug.Log("SpawnWard상태");
        if (!m_isSpawnWard)
        {
            TransitionToState(State.IDLE);
        }
    }
    private void TransitionToState(State _NewState)
    {
        m_CurrentState = _NewState;

        switch (_NewState)
        {
            // Enter
            case State.IDLE:
                Enter_Idle();
                break;
            case State.SPAWN:
                Enter_Spawn();
                break;
            case State.CHASE_MOVE:
                Enter_ChaseMove();
                break;
            case State.SPIT:
                Enter_Spit();
                break;
            case State.WARCRY:
                Enter_WarCry();
                break;
            case State.SPAWN_WARD:
                Enter_SpawnWard();
                    break;
            default:
                return;


        }
    }

    private void Update_State()
    {
        switch (m_CurrentState)
        {
            // Update
            case State.IDLE:
                Update_Idle();
                break;
            case State.SPAWN:
                Update_Spawn();
                break;
            case State.CHASE_MOVE:
                Update_ChaseMove();
                break;
            case State.SPIT:
                Update_Spit();
                break;
            case State.WARCRY:
                Update_WarCry();
                break;
            case State.SPAWN_WARD:
                Update_SpawnWard();
                break;
            default:
                return;
        }
    }

    public void Anim_ShootFireAcid()
    {
        for (int i = 0; i < m_Acid.Length; i++)
        {
            if (m_Acid[i] != null)
            {
                m_Acid[i].Play();
            }
        }
    }

    protected IEnumerator Corutine_FireAcid()
    {
        Debug.Log("스플릿 코루틴");
        m_isFireAcid = true;
        m_CanFireAcid = false;

        
        m_Anim.SetTrigger("Fire");
        

        yield return new WaitForSeconds(m_FireAcidTime);

        m_isFireAcid = false;

        yield return new WaitForSeconds(m_FireAcidCoolTime);
        m_CanFireAcid = true;
    }

    protected IEnumerator Corutine_Warcry()
    {
        m_isWarcry = true;
        m_CanWarcry = false;

        m_Anim.SetTrigger("Warcry");
       

        StartCoroutine(Corutine_WarcrySpawn(m_Grub, m_BoomPos));

        yield return new WaitForSeconds(m_WarcryTime);

        m_isWarcry = false;

        yield return new WaitForSeconds(m_WarcryCoolTime);
        m_CanWarcry = true;
    }

    protected IEnumerator Corutine_SpawnWard()
    {
        m_isSpawnWard = true;
        m_CanSpawnWard = false;

        m_Anim.SetTrigger("SpawnWard");
        
        yield return new WaitForSeconds(0.3f);
        m_ParticleSpawnWard.Play();
       
        
        yield return new WaitForSeconds(m_SpawnWardTime);

        m_isSpawnWard = false;

        yield return new WaitForSeconds(m_SpawnWardCoolTime);

        m_CanSpawnWard = true;
    }

    private void HalfHp_Check()
    {
        if (m_CurrentHp <= m_MaxHp / 2 && !m_WarcryHpChecked)
        {
            m_ParticleCanWarCry.Play();
            m_CanWarcry = true;
            m_WarcryHpChecked = true;
        }

        if (m_CurrentHp <= 0)
        {
            m_ParticleCanWarCry.Stop();
        }
    }

    protected IEnumerator Corutine_WarcrySpawn(GameObject _Prepab, Transform[] _Pos)
    {
        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < _Pos.Length; i++)
        {
            if (_Pos[i] != null)
            {
                Instantiate(_Prepab, _Pos[i].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.3f);
            }
        }

        
    }


    


    void OnEnable()
    {
        InitializeArray();
    }

    void InitializeArray()
    {
        if (m_BoomPos == null || m_BoomPos.Length == 0)
        {
            Debug.LogError("m_BoomPos 배열이 Null이거나 초기화되지 않았습니다!");
        }
        else
        {
            for (int i = 0; i < m_BoomPos.Length; i++)
            {
                if (m_BoomPos[i] == null)
                {
                    Debug.LogError($"m_BoomPos[{i}] 요소가 Null입니다.");
                }
            }
        }
    }

    private void Anim_SpawnWard()
    {
        StartCoroutine(Corutine_CreateWard());
    }

    protected IEnumerator Corutine_CreateWard()
    {
        Vector3 basePosition = m_ParticleSpawnWard.transform.position;
        Instantiate(m_MonsterWard, basePosition + new Vector3(-3, 3, 0), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        Instantiate(m_MonsterWard, basePosition + new Vector3(0, 3, 0), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        Instantiate(m_MonsterWard, basePosition + new Vector3(3, 3, 0), Quaternion.identity);

    }


}
