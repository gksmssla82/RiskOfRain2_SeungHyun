using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Beetle : MonsterBase
{
    [SerializeField] private ParticleSystem m_SpawnEffect;
    
    private enum State
    {
        SPAWN,
        IDLE,
        CHASE_MOVE,
        RANDOM_MOVE,
        ATTACK,
        HURT,
        STURN,
        DEAD
    }

    private State m_CurrentState;
   
    protected override void Start()
    {
        base.Start();
        
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

       
    }

    #region SPAWN
    private void Enter_Spawn()
    {
       
        Set_AnimLayerWeight(1, 0);
        AudioManager.Instance.Play(gameObject, "Bettle_Spawn");
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
        AudioManager.Instance.Random_SoundPlay(gameObject,29,3);
        Set_AnimLayerWeight(1, 1);
        m_Agent.speed = 0f;
    }

    private void Update_Idle()
    {
        Debug.Log("Idle 상태");

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
        Debug.Log("Chase 상태");
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

        AudioManager.Instance.Play(gameObject, "Bettle_Attack");
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
        AudioManager.Instance.Random_SoundOnShot(gameObject, 11,3);
        
        
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

        switch(_NewState)
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
            case State.DEAD:
                Enter_Dead();
                break;
            case State.STURN:
                Enter_Sturn();
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
    public void Anim_SpawnEffect()
    {
        m_SpawnEffect.Play();
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


