using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Golem: MonsterBase
{
    [Header("Effect")]
    [SerializeField] private ParticleSystem m_SpawnEffect;
    [SerializeField] private ParticleSystem m_SmakeEffect;
 
   
    private Color m_RespawnColor = new Color(0, 0, 0);
    private Color m_RespawnedColor = new Color(255f, 145f, 0f);
    private Laser m_Laser;

 
    private enum State
    {
        SPAWN,
        IDLE,
        CHASE_MOVE,
        RANDOM_MOVE,
        ATTACK,
        STURN,
        HURT,
        DEAD
    }

    private State m_CurrentState;

    protected override void Start()
    {
        base.Start();
        
        m_Laser = GetComponentInChildren<Laser>();
        TransitionToState(State.SPAWN);
    }


    protected override void Update()
    {
        base.Update();

        if (m_CurrentHp <= 0)
        {
            TransitionToState(State.DEAD);
        }
       

        if (m_isSpawned && !m_isDead && !m_isAttack && m_Laser.isCanFire_Check()
            && !Can_MeleeAttack() && Can_ThrowAttack(5f,15f) && !m_isSturn)
        {
            m_Laser.Start_Laser();
        }


        if (!m_isSturn)
        {
            Aim_Anim();
        }

        Update_State();

       
    }

    #region SPAWN
    private void Enter_Spawn()
    {
        AudioManager.m_Instnace.Random_SoundPlay(gameObject,83, 3);
        Set_AnimLayerWeight(1, 0);
        m_ToonMaterial.SetColor("_Emissive_Color", new Color(0f,0f,0f));
        StartCoroutine(Corutine_LerpShader(0f, 1f, 6f));
        StartCoroutine(Corutine_SpawnLerpColor(m_RespawnColor, m_RespawnedColor, 8f));
        StartCoroutine(Corutine_Spawn());
    }

    private void Update_Spawn()
    {
        // 스폰이끝나면 Idle로
        if (m_isSpawned)
        {
            TransitionToState(State.IDLE);
        }
    }
    #endregion
    #region IDLE
    private void Enter_Idle()
    {
        
        Set_AnimLayerWeight(1, 1);
        m_Agent.speed = 0f;
        AudioManager.m_Instnace.Random_SoundPlay(gameObject,77, 4);
    }

    private void Update_Idle()
    {

        if (Can_MeleeAttack() && m_CanAttack)
        {
            TransitionToState(State.ATTACK);
        }

        if (!Can_MeleeAttack())
        {
            TransitionToState(State.CHASE_MOVE);
        }

        if (m_isSturn)
        {
            TransitionToState(State.STURN);
        }

    }




    private void Enter_ChaseMove()
    {
       

    }

    private void Update_ChaseMove()
    {
        SelectMove_AI();

        if (Can_MeleeAttack())
        {
            m_Anim.SetBool("Move", false);
            TransitionToState(State.IDLE);
        }

        if (Can_MeleeAttack() && m_CanAttack)
        {
            m_Anim.SetBool("Move", false);
            TransitionToState(State.ATTACK);
        }

        if (m_isSturn)
        {
            TransitionToState(State.STURN);
        }
    }

    private void Enter_Attack()
    {
        AudioManager.m_Instnace.Random_SoundPlay(gameObject,71, 2);
    }

    private void Update_Attack()
    {
        if (m_CanAttack && !m_isDead)
        {
            StartCoroutine(Corutine_Attack());
        }

        if (!m_isAttack)
        {
            TransitionToState(State.IDLE);
        }
    }

    private void Enter_Sturn()
    {
        Debug.Log("스턴 상태");
        m_Agent.speed = 0f;
        m_Agent.velocity = Vector3.zero;
        m_Agent.isStopped = true;
        m_Anim.SetBool("Sturn", true);
        AudioManager.m_Instnace.Random_SoundOnShot(gameObject, 11, 3);
        m_SturnVfx.Play();
    }

    private void Update_Sturn()
    {
        if (m_isSturn)
        {
            m_SturnTimer += Time.deltaTime;

            if (m_SturnTimer >= m_SturnTime)
            {
                m_isSturn = false;
                m_Anim.SetBool("Sturn", false);
                m_SturnTimer = 0f;
                m_Agent.isStopped = false;
                TransitionToState(State.IDLE);
            }
        }
    }

    private void Enter_Dead()
    {
        Set_AnimLayerWeight(1, 0);
    }
    #endregion

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
            case State.RANDOM_MOVE:
                break;
            case State.ATTACK:
                Enter_Attack();
                break;
            case State.STURN:
                Enter_Sturn();
                break;
            case State.DEAD:
                Enter_Dead();
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
            case State.RANDOM_MOVE:
                break;
            case State.STURN:
                Update_Sturn();
                break;
            case State.ATTACK:
                Update_Attack();
                break;
            default:
                return;
        }
    }

    public void Anim_SpawnEffect()
    {
        m_SpawnEffect.Play();
    }

    public void Anim_Smake()
    {
        
        
    }




    private IEnumerator Corutine_LerpShader(float _StartValue, float _EndValue, float _Duration)
    {
        
        float elapsed = 0f;

        while (elapsed < _Duration)
        {
            
            // 진행도 계산 0~1
            float t = elapsed / _Duration;

            float newValue = Mathf.Lerp(_StartValue, _EndValue, t);
            m_ToonMaterial.SetFloat("_Clipping_Level", newValue);

            // 결과시간 누적
            elapsed += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 최종값 보정
        m_ToonMaterial.SetFloat("_Clipping_Level", _EndValue);
    }

    private IEnumerator Corutine_SpawnLerpColor(Color _StartCoror, Color _EndColor, float _Duration)
    {
        float EmmisiveTime = m_SpawnTime - 1f;
        yield return new WaitForSeconds(EmmisiveTime);

        float elapsed = 0f;

        while (elapsed < _Duration)
        {
           
            // 진행도 계산 0~1
            float t = elapsed / _Duration;

            Color newValue = Color.Lerp(_StartCoror, _EndColor, t);
            m_ToonMaterial.SetColor("_Emissive_Color", newValue);

            // 결과시간 누적
            elapsed += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 최종값 보정
        m_ToonMaterial.SetColor("_Emissive_Color", _EndColor);
    }

    public override void Anim_AttackHitBox_On()
    {
        base.Anim_AttackHitBox_On();
        m_SmakeEffect.Play();
    }

    public override void Anim_AttackHitBox_Off()
    {
        base.Anim_AttackHitBox_Off();
    }

}


