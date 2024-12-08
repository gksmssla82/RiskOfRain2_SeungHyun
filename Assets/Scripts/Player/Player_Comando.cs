using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Player_Comando : Player
{
    [Header("Bullet")]
    [SerializeField] private Transform m_LeftGunMuzzle;
    [SerializeField] private Transform m_RightGunMuzzle;
    [SerializeField] private Transform m_CenterGunMuzzle;
    [SerializeField] private GameObject m_NormalBullet;
    [SerializeField] private GameObject m_M2Bullet;
    [SerializeField] private GameObject m_RBullet;
    [SerializeField] private ParticleSystem m_NormalLeftBulletFlash;
    [SerializeField] private ParticleSystem m_NormalRightBulletFlash;
    [SerializeField] private ParticleSystem m_M2CenterBulletFlash;

    [Header("Sliding")]
    [SerializeField] private float m_SlideSpeed = 15f;
    [SerializeField] private float m_SlideDuration = 1.5f;
    [SerializeField] private float m_SlideCoolTime = 3f;
    private Sliding m_SkillSliding;

    [Header("M2Shoot")]
    public bool m_CanM2Shoot = false;
    public bool m_isM2Shoot = false;
    public float m_M2ShootTime = 1f;
    public float m_M2ShootCoolTime = 5f;

    [Header("RShoot")]
    public bool m_CanRShoot = false;
    public bool m_isRShoot = false;
    public float m_RShootTime = 2f;
    public float m_RShootCoolTime = 8f;


    [Header("UI")]
    [SerializeField] private Skill_CoolDown m_SlidingCoolDown;
    [SerializeField] private Skill_CoolDown m_M2ShootCoolDown;
    [SerializeField] private Skill_CoolDown m_RShootCoolDown;
   
    private bool m_NormalBulletFire_Left;
    private bool m_NormalBulletFire_Right;
    private bool m_RBulletFire_Right;
    private bool m_M2BulletFire;
   

    protected override void Start()
    {
        base.Start();
       
        m_SkillSliding = new Sliding(this, m_SlideSpeed, m_SlideDuration, m_SlideCoolTime, m_SlidingCoolDown);
       
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Shoot();
        Use_Skill();
    }

    private void LateUpdate()
    {
        Attack_Projectile();
    }

    protected void Use_Skill()
    {
        m_SkillSliding.Use_Skill("Sliding");
        
    }

    private void Shoot()
    {

        Shoot_Ray();

        if (m_Input.m_NormalShoot)
        {
            Is_Shoot();
            m_Anim.SetBool("NormalShot", true);
        }

        else if (m_Input.m_M2Shoot && !m_CanM2Shoot)
        {
            StartCoroutine(Corutine_M2Shoot()); 
        }

        else if (m_Input.m_RShoot && !m_CanRShoot)
        {
            StartCoroutine(Corutine_RShoot());
        }

        else if (!m_Input.m_NormalShoot || !m_isM2Shoot || !m_isRShoot)
        {

            Not_Shoot();
            m_Anim.SetBool("NormalShot", false);

            // Shot애니메이션 비활성화
            //Set_AnimLayerWeight(2, 0f);
        }
    }

    public void Attack_Projectile()
    {
        if (m_NormalBulletFire_Left)
        {
            NormalShoot_Left();
        }

        if (m_NormalBulletFire_Right)
        {
            NormalShoot_Right();
        }

        if (m_M2BulletFire)
        {
            M2Shoot_Center();
        }

        if (m_RBulletFire_Right)
        {
            RShoot_Right();
        }
    }

   
    #region Ingredient
    private void ShootMove()
    {
        if (m_Controller.m_isMove && m_Input.m_NormalShoot ||
            m_Controller.m_isMove && m_isM2Shoot ||
            m_Controller.m_isMove && m_isRShoot)
        {
            Vector2 MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            m_Anim.SetBool("ShootMove", true);

            m_Anim.SetFloat("Move_X", MoveInput.x);
            m_Anim.SetFloat("Move_Y", MoveInput.y);
        }

        else
        {
            m_Anim.SetBool("ShootMove", false);
        }
    }

    private void NormalShoot_Left()
    {
        Vector3 AimDir = (m_MouseWorldPos - m_LeftGunMuzzle.position).normalized;
        AudioManager.m_Instnace.Random_SoundPlay(gameObject,2, 3);
        m_NormalLeftBulletFlash.Play();
        Instantiate(m_NormalBullet, m_LeftGunMuzzle.position, Quaternion.LookRotation(AimDir, Vector3.up));
      
        m_NormalBulletFire_Left = false;
    }

    private void NormalShoot_Right()
    {
        Vector3 AimDir = (m_MouseWorldPos - m_RightGunMuzzle.position).normalized;
        AudioManager.m_Instnace.Random_SoundPlay(gameObject,2, 3);
        m_NormalRightBulletFlash.Play();
        Instantiate(m_NormalBullet, m_RightGunMuzzle.position, Quaternion.LookRotation(AimDir, Vector3.up));
      
        m_NormalBulletFire_Right = false;
    }

    private void RShoot_Right()
    {
        Vector3 AimDir = (m_MouseWorldPos - m_RightGunMuzzle.position).normalized;
        AudioManager.m_Instnace.Random_SoundPlay(gameObject,8, 3);
        m_NormalRightBulletFlash.Play();
        Instantiate(m_RBullet, m_RightGunMuzzle.position, Quaternion.LookRotation(AimDir, Vector3.up));
        m_RBulletFire_Right = false;
    }

    private void M2Shoot_Center()
    {
        Vector3 AimDir = (m_MouseWorldPos - m_CenterGunMuzzle.position).normalized;
        AudioManager.m_Instnace.Random_SoundPlay(gameObject,5, 3);
        m_M2CenterBulletFlash.Play();
        Instantiate(m_M2Bullet, m_CenterGunMuzzle.position, Quaternion.LookRotation(AimDir, Vector3.up));
        m_M2BulletFire = false;
    }

   

    private void Is_Shoot()
    {
        // 크로스헤어
        Change_CrossHair(false, true);
        // 마우스 민감도
        m_Controller.Set_Sensitivity(SensitivityManager.m_Instance.m_ShootingSensitivity);
        // 카메라 키보드 입력방향으로 향하는거 막기
        m_Controller.Set_RotateOnMove(false);

        // 스프린트중이면 스프린트 해제
        if (m_Input.sprint)
        {
            m_Input.sprint = false;
        }

        Vector3 WorldAimTarget = m_MouseWorldPos;
        WorldAimTarget.y = transform.position.y;
        Vector3 AimDirection = (WorldAimTarget - transform.position).normalized;

        // Shot애니메이션 활성화
        //Set_AnimLayerWeight(2, 0.5f);


        ShootMove();


        transform.forward = Vector3.Lerp(transform.forward, AimDirection, Time.deltaTime * 20f);

    }

    private void Not_Shoot()
    {
        Change_CrossHair(true, false);
        m_Controller.Set_Sensitivity(SensitivityManager.m_Instance.m_CameraSensitivity);
        m_Controller.Set_RotateOnMove(true);
        ShootMove();

    }

    

    #endregion

    #region Corutine
    

    private IEnumerator Corutine_M2Shoot()
    {
        m_M2ShootCoolDown.Use_Skill(m_M2ShootCoolTime, "M2Shoot");
        m_CanM2Shoot = true;
        m_isM2Shoot = true;
        m_Anim.SetTrigger("M2Shoot");

        float ElapsdTime = 0f;
        float SkillDuration = m_M2ShootTime;

        while (ElapsdTime < SkillDuration)
        {
            Is_Shoot();
            ElapsdTime += Time.deltaTime;
            yield return null;
        }
        m_isM2Shoot = false;



        // 쿨타임에서 스킬 지속시간을 뻄
        yield return new WaitForSeconds(m_M2ShootCoolTime - SkillDuration);

        m_CanM2Shoot = false;
    }

    private IEnumerator Corutine_RShoot()
    {
        m_RShootCoolDown.Use_Skill(m_RShootCoolTime, "RShoot");
        m_CanRShoot = true;
        m_isRShoot = true;
        m_Anim.SetTrigger("RShoot");

        float ElapsdTime = 0f;
        float SkillDuration = m_RShootTime;

        while (ElapsdTime < SkillDuration)
        {
            Is_Shoot();
            ElapsdTime += Time.deltaTime;
            yield return null;
        }
        m_isRShoot = false;



        // 쿨타임에서 스킬지속시간을 뻄
        yield return new WaitForSeconds(m_RShootCoolTime - SkillDuration);

        m_CanRShoot = false;
    }


    #endregion


    #region Anim Event
    public void AnimTrigger_NormalShootLeft()
    {
        m_NormalBulletFire_Left = true;
    }

    public void AnimTrigger_NormalShootRight()
    {
        m_NormalBulletFire_Right = true;
    }

    public void AnimTrigger_M2BulletShoot()
    {
        m_M2BulletFire = true;
    }

    public void AnimTrigger_RBulletShoot()
    {
        m_RBulletFire_Right = true;
    }

    public void AnimWalk_Sound()
    {
        AudioManager.m_Instnace.Random_SoundOnShot(gameObject,17, 5);
    }
    public void AnimJump_Sound()
    {
        AudioManager.m_Instnace.Random_SoundOnShot(gameObject, 22, 3);
    }
    public void AnimLand_Sound()
    {
        AudioManager.m_Instnace.Random_SoundOnShot(gameObject, 25, 3);
    }
    #endregion
}
