using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Lemurian : MonsterBase
{
    
    [Header("Effect")]
    [SerializeField] private ParticleSystem m_SpawnEffect;

    [Header("Fire Ball")]
    [SerializeField] private Transform m_FirePoint;
    [SerializeField] private ParticleSystem m_ChargeFire;
    [SerializeField] private GameObject m_FireBall;
    
    [SerializeField] private bool m_isFireBall = false;
    [SerializeField] private bool m_CanFireBall = true;
    [SerializeField] private float m_FireBallTime;
    [SerializeField] private float m_FireBallCoolTime;
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
        m_ChargeFire.Stop();
        TransitionToState(State.SPAWN);
    }


    protected override void Update()
    {
        base.Update();

       

        if (m_CurrentHp <= 0)
        {
            TransitionToState(State.DEAD);
        }

        if (!m_isSturn)
        {
            Aim_Anim();
        }

        Update_State();

        if (m_isSpawned && !m_isDead && !m_isAttack && m_CanFireBall 
            && !Can_MeleeAttack() && Can_ThrowAttack(5f,22f) && !m_isSturn)
        {
            StartCoroutine(Corutine_FireBall());
        }
    }

    #region SPAWN
    private void Enter_Spawn()
    {
       
        Set_AnimLayerWeight(1, 0);
        AudioManager.Instance.Random_SoundPlay(gameObject,58, 3);
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
        AudioManager.Instance.Random_SoundPlay(gameObject,50, 5);
        Set_AnimLayerWeight(1, 1);
        m_Agent.speed = 0f;
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

        if (Can_MeleeAttack() && m_CanAttack && !m_isFireBall)
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
        AudioManager.Instance.Random_SoundPlay(gameObject,37, 3);

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

    private void Enter_Dead()
    {
        Set_AnimLayerWeight(1, 0);
    }
    #endregion

    private void Enter_Sturn()
    {
        Debug.Log("스턴 상태");
        m_Agent.speed = 0f;
        m_Agent.velocity = Vector3.zero;
        m_Agent.isStopped = true;
        m_Anim.SetBool("Sturn", true);
        m_SturnVfx.Play();
        AudioManager.Instance.Random_SoundOnShot(gameObject,11, 3);
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
            case State.ATTACK:
                Update_Attack();
                break;
            case State.STURN:
                Update_Sturn();
                break;
            default:
                return;
        }
    }


    public void Anim_ShootFireBall()
    {
        Vector3 Direction = (m_Player.transform.position - transform.position).normalized;
        Direction.y -= 0.1f;
        Instantiate(m_FireBall, m_FirePoint.position, Quaternion.LookRotation(Direction, Vector3.up));
        AudioManager.Instance.Random_SoundOnShot(gameObject, 47, 3);
        m_ChargeFire.Stop();
    }

    public void Anim_ChageFireBall()
    {
        m_ChargeFire.Play();
        AudioManager.Instance.Random_SoundOnShot(gameObject,41, 3);
    }

    public void Anim_SpawnEffect()
    {
        m_SpawnEffect.Play();
    }

   


    protected IEnumerator Corutine_FireBall()
    {
        m_CanFireBall = false;
        m_isFireBall = true;

        m_Anim.SetTrigger("Fire");
        AudioManager.Instance.Random_SoundPlay(gameObject,47, 3);
        yield return new WaitForSeconds(m_FireBallTime);

        m_isFireBall = false;

        yield return new WaitForSeconds(m_FireBallCoolTime);
        m_CanFireBall = true;
    }

    public override void Anim_AttackHitBox_On()
    {
        base.Anim_AttackHitBox_On();
    }

    public override void Anim_AttackHitBox_Off()
    {
        base.Anim_AttackHitBox_Off();
    }
}


