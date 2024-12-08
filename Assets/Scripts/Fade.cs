using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fade : MonoBehaviour
{
    public CanvasGroup m_FadeGroup;
    public float m_FadeTime = 1f;
    public float m_FadeInAndOutTime = 1f;

    // UI Fade Out (������ �����)
    public void Fade_Out()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    // UI Fade In (������ ��Ÿ��)
    public void Fade_In()
    {
        m_FadeGroup.gameObject.SetActive(true);
        StartCoroutine(FadeInCoroutine());
    }

    // UI Fade In And Out (��Ÿ���ٰ� �����)

    public void Fade_InAndOut()
    {
        m_FadeGroup.gameObject.SetActive(true);
        StartCoroutine(FadeInAndOutCorutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float startAlpha = m_FadeGroup.alpha;
        float time = 0f;

        while (time < m_FadeTime)
        {
            time += Time.deltaTime;
            m_FadeGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / m_FadeTime); // ��������
            yield return null;
        }

        m_FadeGroup.alpha = 0f;
        m_FadeGroup.gameObject.SetActive(false);
    }

    private IEnumerator FadeInCoroutine()
    {
        float startAlpha = m_FadeGroup.alpha;
        float time = 0f;

        while (time < m_FadeTime)
        {
            time += Time.deltaTime;
            m_FadeGroup.alpha = Mathf.Lerp(startAlpha, 1f, time / m_FadeTime); // ����������
            yield return null;
        }

        m_FadeGroup.alpha = 1f;
    }

    private IEnumerator FadeInAndOutCorutine()
    {
        float startAlpha = m_FadeGroup.alpha;
        float time = 0f;

        while (time < m_FadeTime)
        {
            time += Time.deltaTime;
            m_FadeGroup.alpha = Mathf.Lerp(startAlpha, 1f, time / m_FadeTime); // ����������
            yield return null;
        }

        m_FadeGroup.alpha = 1f;

        yield return new WaitForSeconds(m_FadeInAndOutTime);

        startAlpha = m_FadeGroup.alpha;
        time = 0f;

        while (time < m_FadeTime)
        {
            time += Time.deltaTime;
            m_FadeGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / m_FadeTime); // ��������
            yield return null;
        }

        m_FadeGroup.alpha = 0f;
        m_FadeGroup.gameObject.SetActive(false);
    }
}