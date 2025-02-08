using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : Singleton<BgmManager>
{
    public AudioClip[] m_Clips; // 배경음악들
    private AudioSource m_Source;
    // 반복문 내에서 new가 자주 호출되면 따로 선언해서 쓰는편이 성능성 유리
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
            Debug.Log("AudioSorce컴포넌트가 추가되었습니다");
        }
    }

    public void Play(int _PlayMusicTrack)
    {
        if (m_Source != null)
        {
            m_Source.clip = m_Clips[_PlayMusicTrack];
            Debug.Log("소스클립 : " + m_Source.clip);
            m_Source.Play();
        }

        else
        {
            Debug.Log("Play에서 m_Source가 null");
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
        for (float i = 0f; i <= 1f; i += 0.01f) // 볼륨이 0.01씩 100번 커짐
        {
            m_Source.volume = i;
            yield return m_WaitTime;
        }
    }

    IEnumerator FadeOut_MusicCoroutine()
    {
        for (float i = 1.0f; i >= 0f; i -= 0.01f) // 볼륨이 0.01씩 100번 작아짐
        {
            m_Source.volume = i;
            yield return m_WaitTime;
        }
    }

}
