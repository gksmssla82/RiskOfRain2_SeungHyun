using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Skill_CoolDown : MonoBehaviour
{
    private Player m_Player;
    [SerializeField]
    private Image m_ImageCoolDown;
    [SerializeField]
    private GameObject m_FinishCoolDownImage;
    [SerializeField]
    private TMP_Text m_TextCoolDown;

    public bool m_isCooldown = false;
    public float m_CoolDownTime = 10f;
    private float m_CoolDownTimer = 0.0f;
    private string m_SkillName;
    private Animator m_FinishAnim;
    // Start is called before the first frame update
    void Start()
    {
        m_TextCoolDown.gameObject.SetActive(false);
        m_ImageCoolDown.fillAmount = 0.0f;

        if(m_FinishCoolDownImage != null)
        {
            m_FinishAnim = m_FinishCoolDownImage.GetComponent<Animator>();

            if (m_FinishAnim == null)
            {
                Debug.LogError("FInsh 이미지의 애니메이터를 찾을 수 없습니다.");
            }
        }

        else
        {
            Debug.LogError("Finsh 이미지를 찾을 수 없습니다.");
        }

        m_Player = FindAnyObjectByType<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {

        

        if (m_isCooldown)
        {
            Apply_Cooldown();
            if (!m_isCooldown)
            {
                Finish_Anim();
            }
        }

        
    }

    void Apply_Cooldown()
    {
        m_CoolDownTimer -= Time.deltaTime;

        if (m_CoolDownTimer < 0.0f)
        {
            m_isCooldown = false;
            m_TextCoolDown.gameObject.SetActive(false);
            m_ImageCoolDown.fillAmount = 0.0f;
            
        }

        else
        {
            m_TextCoolDown.text = Mathf.RoundToInt(m_CoolDownTimer).ToString();
            m_ImageCoolDown.fillAmount = m_CoolDownTimer / m_CoolDownTime;
          
        }

        

        
         
        

        
    }

    public void Use_Skill(float _CoolDownTime, string _SkillName)
    {
        if (m_isCooldown)
            return;


        
        m_isCooldown = true;
        m_TextCoolDown.gameObject.SetActive(true);
        m_CoolDownTime = _CoolDownTime;
        m_CoolDownTimer = m_CoolDownTime;
        this.m_SkillName = _SkillName;

        
    }

    public void Finish_Anim()
    {
        if (m_FinishAnim != null)
        {
            StartCoroutine(Disable_Finish_Anim());
        }

        else
        {
            Debug.Log("피니쉬 애님 못찾음");
        }
    }

    private IEnumerator Disable_Finish_Anim()
    {

        m_FinishCoolDownImage.SetActive(true);
        AnimatorStateInfo StateInfo = m_FinishAnim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(StateInfo.length);
        m_FinishCoolDownImage.SetActive(false);
        AudioManager.m_Instnace.PlayOneShot(m_Player.gameObject, "CoolDown");
    }

}
