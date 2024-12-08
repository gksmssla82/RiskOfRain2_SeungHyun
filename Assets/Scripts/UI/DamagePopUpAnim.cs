using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamagePopUpAnim : MonoBehaviour
{
    public AnimationCurve m_OpacityCurve;
    public AnimationCurve m_ScaleCurve;
    public AnimationCurve m_HeightCurve;

    private TextMeshProUGUI m_Tmp;
    private float m_Time = 0f;
    private Vector3 m_Origin;
    private void Awake()
    {
        m_Tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        m_Origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 시간이 지날수록 알파값을 변화시킴
        m_Tmp.color = new Color(1, 1, 1, m_OpacityCurve.Evaluate(m_Time));
        transform.localScale = Vector3.one * m_ScaleCurve.Evaluate(m_Time);
        transform.position = m_Origin + new Vector3(0, 1 + m_HeightCurve.Evaluate(m_Time), 0);
        m_Time += Time.deltaTime;
    }
}
