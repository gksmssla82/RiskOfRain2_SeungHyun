using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour, IStatus, IDamage, ILevel
{
    // Component
    [HideInInspector] public StarterAssetsInputs m_Input;
    [HideInInspector] public ThirdPersonController m_Controller;
    [HideInInspector] public Animator m_Anim;

    [Header("Camera")]
    [SerializeField] protected CinemachineVirtualCamera m_ShiftSkillCam;
    [SerializeField] private Transform m_FocusPoint;

    [Header("Mouse")]
    //[Tooltip("기본 카메라 민감도입니다.")]
    //public float m_NormalSenstivity = 1f;
    //[Tooltip("사격 카메라 민감도입니다.")]
    //public float m_ShootSenstivity = 1f;
    protected Vector3 m_MouseWorldPos;

    [Header("Aim")]
    [SerializeField] private LayerMask m_AimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform m_DebugAimTransform;
    [SerializeField] private GameObject m_CrossHair_Normal;
    [SerializeField] private GameObject m_CrossHair_Shot;

    [Header("UI")]
    [SerializeField] private Image m_HpSlider;
    [SerializeField] private Image m_ExpSlider;
    [SerializeField] private TextMeshProUGUI m_HpText;
    [SerializeField] private TextMeshProUGUI m_LevelText;
    [SerializeField] private ParticleSystem m_LevelUpEffect;

    [Header("Damage")]
    [SerializeField] private float MinDamage = 10f;
    [SerializeField] private float MaxDamage = 30f;
    [SerializeField] private float CriticalProbability = 0.2f; // 크리티컬확률
    [SerializeField] private float CriticalDamage = 1.5f; // 크리티컬 데미지

    [Header("Item")]
    public int m_Money = 0;
    public int m_DisplayMoney = 0;
    public TextMeshProUGUI m_MoneyText;
    private Coroutine m_MoneyLerpCoroutine;  // 코루틴 참조 변수


    ////////////////////////////////// Interface ///////////////////////////
    public float m_MinDamage { get => MinDamage; set => MinDamage = value; }
    public float m_MaxDamage { get => MaxDamage; set => MaxDamage = value; }
    public float m_CriticalDamage { get => CriticalDamage; set => CriticalDamage = value; }
    public float m_CriticalProbability { get => CriticalProbability; set => CriticalProbability = value; }
    public float m_MaxHp { get; set; } = 100f;
    public float m_CurrentHp { get; set; }
    public int m_Defence { get; set; } = 10;
    public float m_Level { get; set; } = 1;
    public float m_MaxExp { get; set; } = 50f;
    public float m_CurrentExp { get; set; }
    ////////////////////////////////////////////////////////////////////////
    
  
    private float m_CurrentHorizontal;
    private float m_CurrentVertical;
    private float m_HoriznontalVelocity;
    private float m_VerticalVelocity;
    private float m_SmothTime = 0.1f;

    private float m_RecoveryDelay = 3f;
    public float m_RecoveryRate = 0.02f; // 5% = 0.05
    private float m_RecoveryInterval = 1f; // 1초에 한번씩 회복
    private bool m_isRecovering = false;
    private bool m_isInRecoverDelay = false; // 회복 딜레이 여부 확인
    private Coroutine m_RecoveryCorutine;

    protected virtual void Start()
    {
        m_Input = GetComponent<StarterAssetsInputs>();
        m_Controller = GetComponent<ThirdPersonController>();
        m_Anim = GetComponent<Animator>();
  
        m_CurrentHp = m_MaxHp;
    }

    // Update is called once per frame
    protected virtual void Update()
    { 
        Aim_Anim();
        ShiftCam_Chck();
    }


    

    protected void ShiftCam_Chck()
    {
        if (m_Input.sprint)
        {
            m_ShiftSkillCam.gameObject.SetActive(true);
            // Aim애니메이션 중지
            Set_AnimLayerWeight(1, 0f);
        }

        else
        {
            m_ShiftSkillCam.gameObject.SetActive(false);
            // Aim애니메이션 활성화
            Set_AnimLayerWeight(1, 1f);
        }
    }

    private void Aim_Anim()
    {
        // FocusPoint의 로컬스페이스 오일러Y회전값
        float YRotation = m_FocusPoint.transform.localEulerAngles.y;
        float HorizontalAngle;

        // 인스펙터 회전값 그대로 -180 ~ 180도를 받아오기위에 360을 뺴줌
        if (YRotation > 180)
        {
            HorizontalAngle = YRotation - 360;
        }

        else
        {
            HorizontalAngle = YRotation;
        }

        // -45 ~ 45 범위를 -1 ~ 1범위로 변환
        float NormalizeHorizontal = (Mathf.InverseLerp(-45, 45f, HorizontalAngle) * 2 - 1);


        // 카메라의  X축 방향을 -30 ~ 40범위를 -1 ~ 1로 변환하고 방향반전
        float XRotation = m_FocusPoint.rotation.eulerAngles.x;
        if (XRotation > 180)
        {
            XRotation -= 360;
        }
        float verticalAngle = (Mathf.InverseLerp(-30f, 40f, XRotation) * 2 - 1) * -1;

        // 부드럽게 파라미터 값 변화
        m_CurrentHorizontal = Mathf.SmoothDamp(m_CurrentHorizontal, NormalizeHorizontal, ref m_HoriznontalVelocity, m_SmothTime);
        m_CurrentVertical = Mathf.SmoothDamp(m_CurrentVertical, verticalAngle, ref m_VerticalVelocity, m_SmothTime);

        // 파라미터 값 업데이트
        m_Anim.SetFloat("Aim_X", m_CurrentHorizontal);
        m_Anim.SetFloat("Aim_Y", m_CurrentVertical);

    }

    protected void Change_CrossHair(bool _Normal, bool _Shot)
    {
        m_CrossHair_Normal.SetActive(_Normal);
        m_CrossHair_Shot.SetActive(_Shot);
    }

    protected void Set_AnimLayerWeight(int _LayerNum, float _Weight)
    {
        m_Anim.SetLayerWeight(_LayerNum, _Weight);
    }

    protected void Shoot_Ray()
    {
        // 마우스 월드좌표 위치
        m_MouseWorldPos = Vector3.zero;

        // 스크린 가로 세로 반씩나눠 중앙을 가리키게함
        Vector2 ScreenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        // Ray를 스크린 가운데로
        Ray ray = Camera.main.ScreenPointToRay(ScreenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, m_AimColliderLayerMask))
        {
            m_DebugAimTransform.position = raycastHit.point;
            m_MouseWorldPos = raycastHit.point;
        }
    }

    public void Start_SkillCoroutine(IEnumerator _Corutine)
    {
        StartCoroutine(_Corutine);
    }

    IEnumerator Corutine_StartRecoveryDelay()
    {
        m_isInRecoverDelay = true; // 회복 딜레이 시작
        m_isRecovering = false; // 회복 중지

        yield return new WaitForSeconds(m_RecoveryDelay); // 딜레이후 회복

        // 딜레이가 끝나면 회복 시작
        if (!m_isRecovering) // 회복이 이미 실행 중이 아닌 경우에만 실행
        {
            m_isRecovering = true;
            m_RecoveryCorutine = StartCoroutine(Corutine_RecoverHp());
        }

        // 딜레이가 끝나면 회복 딜레이 상태 해제
        m_isInRecoverDelay = false;
    }
    IEnumerator Corutine_RecoverHp()
    {
        // 체력이 가득 차기 전까지 회복
        while (m_CurrentHp < m_MaxHp && m_isRecovering)
        {
            m_CurrentHp += m_MaxHp * m_RecoveryRate; // 최대체력에 퍼센트만큼 회복
            m_CurrentHp = Mathf.Clamp(m_CurrentHp, 0, m_MaxHp); // 0~MaxHP까지만 최소최대치 설정
            Update_HpUI(); // ui업데이트
            yield return new WaitForSeconds(m_RecoveryInterval); // 1초 대기
        }

        m_isRecovering = false; // 리커버리 중지
        m_RecoveryCorutine = null;

    }
    public void Damage_Up()
    {
        m_MinDamage *= 1.1f;
        m_MaxDamage *= 1.1f;

        Debug.Log("최소데미지 = " + m_MinDamage);
    }

    public void Take_Damage(float _Damage)
    {
        if (m_RecoveryCorutine != null)
        {
            // 회복중이면 회복 중단
            StopCoroutine(m_RecoveryCorutine);
            m_RecoveryCorutine = null;
            m_isRecovering = false; // 즉시 회복 중지 상태로 설정
        }

        m_CurrentHp -= _Damage;
        m_CurrentHp = Mathf.Clamp(m_CurrentHp, 0, m_MaxHp); // 체력이 0 이하로 떨어지지 않도록 제한
        Update_HpUI(); // UI업데이트

        // 새로운 데미지를 받을 떄 회복 딜레이가 실행중이 아니면 딜레이 코루틴 실행
        if (!m_isInRecoverDelay)
        {
            // 데미지를 받았으면 회복을 지연
            StartCoroutine(Corutine_StartRecoveryDelay());
        }
    }

    public void Dead_Check()
    {
        throw new System.NotImplementedException();
    }

    public void Level_Up()
    {
        //레벨업 이펙트 재생
        m_LevelUpEffect.Play();
        AudioManager.m_Instnace.PlayOneShot(gameObject, "Player_LevelUp");
        // 체력/경험치 상승폭
        m_MaxHp += 30f;
        m_MaxExp += 50f;

        // 데미지 상승 로직
        Damage_Up();

        // 체력/경험치 초기화
        m_CurrentExp = 0f;
        m_CurrentHp = m_MaxHp;
        m_Level += 1;
        Update_ExpUI();
        Update_HpUI();
    }

    public void Gain_Exp(float _Exp)
    {
       
        m_CurrentExp += _Exp;
        AudioManager.m_Instnace.Random_SoundOnShot(gameObject, 105, 3);
        Update_ExpUI();

        if (m_CurrentExp >= m_MaxExp)
        {
            Level_Up();
            // 레벨업시스템
        }
    }



    public void Update_HpUI()
    {
        m_HpSlider.fillAmount = m_CurrentHp / m_MaxHp;
        m_HpText.text = $"{Mathf.RoundToInt(m_CurrentHp)} / {Mathf.RoundToInt(m_MaxHp)}";
    }

    private void Update_ExpUI()
    {
        m_ExpSlider.fillAmount = m_CurrentExp / m_MaxExp;
        m_LevelText.text = "레벨 : " + m_Level;
    }

    public void Money_Change(int _MoneyValue)
    {
        m_Money += _MoneyValue;

        if (m_MoneyLerpCoroutine != null)
        {
            StopCoroutine(m_MoneyLerpCoroutine);
        }
        m_MoneyLerpCoroutine = StartCoroutine(Money_Lerp(m_Money));  // Lerp로 점진적인 UI 업데이트 시작
    }

    private IEnumerator Money_Lerp(int _TagetMoney)
    {
        float floatMoney = m_DisplayMoney;

        AudioManager.m_Instnace.Random_SoundOnShot(gameObject, 105, 3);

        while(m_DisplayMoney != _TagetMoney)
        {
            floatMoney = Mathf.Lerp(floatMoney, _TagetMoney, Time.deltaTime * 3f);
            m_DisplayMoney = Mathf.RoundToInt(floatMoney);
            m_MoneyText.text = m_DisplayMoney.ToString();
            yield return null;
        }

        m_DisplayMoney = _TagetMoney;
        m_MoneyText.text = m_DisplayMoney.ToString();
        m_MoneyLerpCoroutine = null;
    }

}
