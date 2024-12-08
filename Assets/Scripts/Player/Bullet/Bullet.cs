using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IMove
{
    protected Player m_Player;
    protected Rigidbody m_Rigid;
    protected MonsterBase m_Monster;

    [Header("Bullet Status")]
    [SerializeField] protected float Speed = 50f;
    
    [SerializeField] protected float m_DestroyTime = 3;

   
    public float m_MoveSpeed { get => Speed; set => Speed = value; }

    public bool m_isMove => throw new System.NotImplementedException();

    protected  virtual void Awake()
    {
   
      

        m_Rigid = GetComponent<Rigidbody>();
        m_Monster = FindAnyObjectByType<MonsterBase>();
        m_Player = FindAnyObjectByType<Player>();
    }


    protected virtual void Update()
    {
        TimeOut_BulletDestroy();
       

        Move();


    }

    protected void TimeOut_BulletDestroy()
    {
        m_DestroyTime -= Time.deltaTime;

        if (m_DestroyTime <= 0)
        {
            Destroy_Bullet();
        }
    }

  

    protected void Destroy_Bullet()
    {
        Destroy(gameObject);
    }

    protected void ActiveFalse_Bullet()
    {
        gameObject.SetActive(false);
    }



    protected void TimeOut_BulletActiveFalse()
    {
        m_DestroyTime -= Time.deltaTime;

        if (m_DestroyTime <= 0)
        {
            ActiveFalse_Bullet();
        }
    }

    public float Calculate_BulletDamage(MonsterBase Monster)
    {
        float Damage = Random.Range(m_Player.m_MinDamage, m_Player.m_MaxDamage);

        if (Check_Critical())
        {
            Monster.m_isCritical = true;
            Damage *= m_Player.m_CriticalDamage;
        }

        else
        {
            Monster.m_isCritical = false;
        }

        // 데미지를 정수로 반올림하여 리턴
        return Mathf.RoundToInt(Damage);

    }

  

    private bool Check_Critical()
    {
        // RandomValue는 0.0 ~ 1.0을 반환
        return Random.value < m_Player.m_CriticalProbability;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        MonsterBase monster = other.GetComponent<MonsterBase>();
        
       
        Destroy_Bullet();

    }

    public void Move()
    {
        Vector3 PreviousPosition = transform.position;

        m_Rigid.velocity = transform.forward * m_MoveSpeed;

        RaycastHit hit;

        if (Physics.Raycast(PreviousPosition, transform.forward, out hit, m_MoveSpeed * Time.deltaTime))
        {
            OnTriggerEnter(hit.collider);
        }
    }
}
