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
    //[Tooltip("�⺻ ī�޶� �ΰ����Դϴ�.")]
    //public float m_NormalSenstivity = 1f;
    //[Tooltip("��� ī�޶� �ΰ����Դϴ�.")]
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
    [SerializeField] private float CriticalProbability = 0.2f; // ũ��Ƽ��Ȯ��
    [SerializeField] private float CriticalDamage = 1.5f; // ũ��Ƽ�� ������

    [Header("Item")]
    public int m_Money = 0;
    public int m_DisplayMoney = 0;
    public TextMeshProUGUI m_MoneyText;
    private Coroutine m_MoneyLerpCoroutine;  // �ڷ�ƾ ���� ����


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
    private float m_RecoveryInterval = 1f; // 1�ʿ� �ѹ��� ȸ��
    private bool m_isRecovering = false;
    private bool m_isInRecoverDelay = false; // ȸ�� ������ ���� Ȯ��
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
            // Aim�ִϸ��̼� ����
            Set_AnimLayerWeight(1, 0f);
        }

        else
        {
            m_ShiftSkillCam.gameObject.SetActive(false);
            // Aim�ִϸ��̼� Ȱ��ȭ
            Set_AnimLayerWeight(1, 1f);
        }
    }

    private void Aim_Anim()
    {
        // FocusPoint�� ���ý����̽� ���Ϸ�Yȸ����
        float YRotation = m_FocusPoint.transform.localEulerAngles.y;
        float HorizontalAngle;

        // �ν����� ȸ���� �״�� -180 ~ 180���� �޾ƿ������� 360�� ����
        if (YRotation > 180)
        {
            HorizontalAngle = YRotation - 360;
        }

        else
        {
            HorizontalAngle = YRotation;
        }

        // -45 ~ 45 ������ -1 ~ 1������ ��ȯ
        float NormalizeHorizontal = (Mathf.InverseLerp(-45, 45f, HorizontalAngle) * 2 - 1);


        // ī�޶���  X�� ������ -30 ~ 40������ -1 ~ 1�� ��ȯ�ϰ� �������
        float XRotation = m_FocusPoint.rotation.eulerAngles.x;
        if (XRotation > 180)
        {
            XRotation -= 360;
        }
        float verticalAngle = (Mathf.InverseLerp(-30f, 40f, XRotation) * 2 - 1) * -1;

        // �ε巴�� �Ķ���� �� ��ȭ
        m_CurrentHorizontal = Mathf.SmoothDamp(m_CurrentHorizontal, NormalizeHorizontal, ref m_HoriznontalVelocity, m_SmothTime);
        m_CurrentVertical = Mathf.SmoothDamp(m_CurrentVertical, verticalAngle, ref m_VerticalVelocity, m_SmothTime);

        // �Ķ���� �� ������Ʈ
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
        // ���콺 ������ǥ ��ġ
        m_MouseWorldPos = Vector3.zero;

        // ��ũ�� ���� ���� �ݾ����� �߾��� ����Ű����
        Vector2 ScreenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        // Ray�� ��ũ�� �����
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
        m_isInRecoverDelay = true; // ȸ�� ������ ����
        m_isRecovering = false; // ȸ�� ����

        yield return new WaitForSeconds(m_RecoveryDelay); // �������� ȸ��

        // �����̰� ������ ȸ�� ����
        if (!m_isRecovering) // ȸ���� �̹� ���� ���� �ƴ� ��쿡�� ����
        {
            m_isRecovering = true;
            m_RecoveryCorutine = StartCoroutine(Corutine_RecoverHp());
        }

        // �����̰� ������ ȸ�� ������ ���� ����
        m_isInRecoverDelay = false;
    }
    IEnumerator Corutine_RecoverHp()
    {
        // ü���� ���� ���� ������ ȸ��
        while (m_CurrentHp < m_MaxHp && m_isRecovering)
        {
            m_CurrentHp += m_MaxHp * m_RecoveryRate; // �ִ�ü�¿� �ۼ�Ʈ��ŭ ȸ��
            m_CurrentHp = Mathf.Clamp(m_CurrentHp, 0, m_MaxHp); // 0~MaxHP������ �ּ��ִ�ġ ����
            Update_HpUI(); // ui������Ʈ
            yield return new WaitForSeconds(m_RecoveryInterval); // 1�� ���
        }

        m_isRecovering = false; // ��Ŀ���� ����
        m_RecoveryCorutine = null;

    }
    public void Damage_Up()
    {
        m_MinDamage *= 1.1f;
        m_MaxDamage *= 1.1f;

        Debug.Log("�ּҵ����� = " + m_MinDamage);
    }

    public void Take_Damage(float _Damage)
    {
        if (m_RecoveryCorutine != null)
        {
            // ȸ�����̸� ȸ�� �ߴ�
            StopCoroutine(m_RecoveryCorutine);
            m_RecoveryCorutine = null;
            m_isRecovering = false; // ��� ȸ�� ���� ���·� ����
        }

        m_CurrentHp -= _Damage;
        m_CurrentHp = Mathf.Clamp(m_CurrentHp, 0, m_MaxHp); // ü���� 0 ���Ϸ� �������� �ʵ��� ����
        Update_HpUI(); // UI������Ʈ

        // ���ο� �������� ���� �� ȸ�� �����̰� �������� �ƴϸ� ������ �ڷ�ƾ ����
        if (!m_isInRecoverDelay)
        {
            // �������� �޾����� ȸ���� ����
            StartCoroutine(Corutine_StartRecoveryDelay());
        }
    }

    public void Dead_Check()
    {
        throw new System.NotImplementedException();
    }

    public void Level_Up()
    {
        //������ ����Ʈ ���
        m_LevelUpEffect.Play();
        AudioManager.m_Instnace.PlayOneShot(gameObject, "Player_LevelUp");
        // ü��/����ġ �����
        m_MaxHp += 30f;
        m_MaxExp += 50f;

        // ������ ��� ����
        Damage_Up();

        // ü��/����ġ �ʱ�ȭ
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
            // �������ý���
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
        m_LevelText.text = "���� : " + m_Level;
    }

    public void Money_Change(int _MoneyValue)
    {
        m_Money += _MoneyValue;

        if (m_MoneyLerpCoroutine != null)
        {
            StopCoroutine(m_MoneyLerpCoroutine);
        }
        m_MoneyLerpCoroutine = StartCoroutine(Money_Lerp(m_Money));  // Lerp�� �������� UI ������Ʈ ����
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
