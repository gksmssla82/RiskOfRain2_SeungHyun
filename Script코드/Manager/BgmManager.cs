using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : Singleton<BgmManager>
{
    public AudioClip[] m_Clips; // ������ǵ�
    private AudioSource m_Source;
    // �ݺ��� ������ new�� ���� ȣ��Ǹ� ���� �����ؼ� �������� ���ɼ� ����
    private WaitForSeconds m_WaitTime = new WaitForSeconds(0.01f);

    protected override void  Awake()
    {
        m_Source = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (m_Source == null)
        {
            m_Source = gameObject.AddComponent<AudioSource>();
            Debug.Log("AudioSorce������Ʈ�� �߰��Ǿ����ϴ�");
        }
    }

    public void Play(int _PlayMusicTrack)
    {
        if (m_Source != null)
        {
            m_Source.clip = m_Clips[_PlayMusicTrack];
            Debug.Log("�ҽ�Ŭ�� : " + m_Source.clip);
            m_Source.Play();
        }

        else
        {
            Debug.Log("Play���� m_Source�� null");
        }
    }

    public void Set_Volumn(float _Volumn)
    {
        m_Source.volume = _Volumn;
    }

    public void Pause()
    {
        m_Source.Pause();
    }

    public void Un_Pause()
    {
        m_Source.UnPause();
    }

    public void Stop()
    {
        m_Source.Stop();
    }


    public void FadeIn_Music()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn_MusicCoroutine());
    }
    public void FadeOut_Music()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut_MusicCoroutine());
    }

    IEnumerator FadeIn_MusicCoroutine()
    {
        for (float i = 0f; i <= 1f; i += 0.01f) // ������ 0.01�� 100�� Ŀ��
        {
            m_Source.volume = i;
            yield return m_WaitTime;
        }
    }

    IEnumerator FadeOut_MusicCoroutine()
    {
        for (float i = 1.0f; i >= 0f; i -= 0.01f) // ������ 0.01�� 100�� �۾���
        {
            m_Source.volume = i;
            yield return m_WaitTime;
        }
    }

}
