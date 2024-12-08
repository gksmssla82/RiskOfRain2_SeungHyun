 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region SoundClass
[System.Serializable] // 커스텀 클래스를 유니티에서 강제로 값바꾸는걸 뛰움
public class Sound
{
    public string m_Name; // 사운드이름

    public AudioClip m_Clip; // 사운드 파일
    public AudioSource m_Source; // 사운드 플레이어
   
    public float m_Volum;
    public float m_Pitch;
    public bool m_Loop;
    public bool m_isUI = false;

    public void SetSource(AudioSource _Source)
    {
        if (_Source == null)
        {
            Debug.LogError("AudioSource가 없습니다");
            return;
        }

        m_Source = _Source;
        m_Source.clip = m_Clip;
        m_Source.loop = m_Loop;
        m_Source.volume = AudioManager.m_Instnace.m_MasterVolume * m_Volum;

        m_Source.spatialBlend = m_isUI ? 0f : 1f; // 2D/3D 사운드 설정

        m_Source.minDistance = 13f; // 최소 거리 (최대 볼륨)
        m_Source.maxDistance = 70f; // 최대 거리 (최소 볼륨)
    }

    public void SetVolumn(float _volume)
    {
        if (m_Source != null)
        {
            m_Source.volume = _volume;
        }
    }

   


    public void Play()
    {
        if (m_Source != null)
        {
            m_Source.Play();
        }
    }

    public void Stop()
    {
        if (m_Source != null)
        {
            m_Source.Stop();
        }
    }

    public void SetLoop()
    {
        if (m_Source != null)
        {
            m_Source.loop = true;
        }
    }

    public void SetLoopCancel()
    {

        if (m_Source != null)
        {
            m_Source.loop = false;
        }
    }

    public void PlaySegment(float _StartTime)
    {
        m_Source.time = _StartTime;
        m_Source.Play();
    }

    public void StopAfterTime()
    {
        Stop();
    }
}
#endregion


public class AudioManager : MonoBehaviour
{
    public Sound[] m_Sounds;
    public static AudioManager m_Instnace;
    public float m_MasterVolume = 1.0f;
    private Dictionary<string, Sound> m_SoundDictionary;
    private float m_Volume = 1;

    private void Awake()
    {
        // 싱글톤 설정
        if (m_Instnace == null)
        {
            m_Instnace = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 오브젝트 유지
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
            return;
        }

       

    }
    private void Start()
    {
        m_SoundDictionary = new Dictionary<string, Sound>();

        for (int i = 0; i < m_Sounds.Length; i++)
        {
            GameObject SoundObject = new GameObject("사운드 파일 이름 : " + i + " = " + m_Sounds[i].m_Name);
            AudioSource source = SoundObject.AddComponent<AudioSource>();
            m_Sounds[i].SetSource(source);
            SoundObject.transform.SetParent(this.transform);

            // Dictionary에 추가
            m_SoundDictionary.Add(m_Sounds[i].m_Name, m_Sounds[i]);
        }
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            m_Volume += 0.1f;
            SetMasterVolume(m_Volume);
            Debug.Log("Vfx볼륨증가 = " + m_MasterVolume);

        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            m_Volume -= 0.1f;
            SetMasterVolume(m_Volume);
            Debug.Log("Vfx볼륨감소 = " + m_MasterVolume);

        }
    }

    // 특정 오브젝트에 AudioSource 설정
    public void InitializeSoundForObject(GameObject _targetObject, string _soundName)
    {
        if (m_SoundDictionary.TryGetValue(_soundName, out Sound sound))
        {
            AudioSource source = _targetObject.AddComponent<AudioSource>();
            sound.SetSource(source);
        }
        else
        {
            Debug.LogWarning("사운드 " + _soundName + "를 찾을 수 없습니다.");
        }
    }

   

    public void Play(GameObject _targetObject, string _soundName)
    {
        if (m_SoundDictionary.TryGetValue(_soundName, out Sound sound))
        {
            AudioSource source = _targetObject.GetComponent<AudioSource>();

            if (source == null)
            {
                // AudioSource가 없으면 추가
                source = _targetObject.AddComponent<AudioSource>();
            }

            sound.SetSource(source);
            sound.Play();
            

        }
        else
        {
            Debug.LogWarning("사운드 " + _soundName + "를 찾을 수 없습니다.");
        }

    }
    public void Play_newObject(GameObject _targetObject, string _soundName)
    {
        if (m_SoundDictionary.TryGetValue(_soundName, out Sound sound))
        {
            GameObject audioObject = new GameObject("SoundObject");
            AudioSource source = audioObject.GetComponent<AudioSource>();

            if (source == null)
            {
                // AudioSource가 없으면 추가
                source = audioObject.AddComponent<AudioSource>();
            }

            audioObject.transform.position = _targetObject.transform.position;

            sound.SetSource(source);
            sound.Play();

            Destroy(audioObject, sound.m_Clip.length);
        }
        else
        {
            Debug.LogWarning("사운드 " + _soundName + "를 찾을 수 없습니다.");
        }

    }

    public void PlayOneShot(GameObject _targetObject, string _soundName, float _volumeScale = 1.0f)
    {
        if (m_SoundDictionary.TryGetValue(_soundName, out Sound sound))
        {
            AudioSource source = _targetObject.GetComponent<AudioSource>();
            if (source == null)
            {
                source = _targetObject.AddComponent<AudioSource>();
                sound.SetSource(source);
            }

            // MasterVolume 적용
            source.PlayOneShot(sound.m_Clip, m_MasterVolume * _volumeScale);
        }
        else
        {
            Debug.LogWarning("사운드 " + _soundName + "를 찾을 수 없습니다.");
        }
    }

    public void Stop(GameObject _targetObject, string _soundName)
    {
        AudioSource source = _targetObject.GetComponent<AudioSource>();

        if (m_SoundDictionary.TryGetValue(_soundName, out Sound sound))
        {
            source.Stop();
        }
        else
        {
            Debug.LogWarning("사운드 " + _soundName + "를 찾을 수 없습니다.");
        }
    }

    public void SetLoop(string _soundName)
    {
        if (m_SoundDictionary.TryGetValue(_soundName, out Sound sound))
        {
            sound.SetLoop();
        }
        else
        {
            Debug.LogWarning("사운드 " + _soundName + "를 찾을 수 없습니다.");
        }
    }

 

    private IEnumerator StopSoundAfterTime(Sound _sound, float _time)
    {
        yield return new WaitForSeconds(_time);
        _sound.Stop();
    }

    public void SetLoopCancel(string _soundName)
    {
        if (m_SoundDictionary.TryGetValue(_soundName, out Sound sound))
        {
            sound.SetLoopCancel();
        }
        else
        {
            Debug.LogWarning("사운드 " + _soundName + "를 찾을 수 없습니다.");
        }
    }


    
    public void SetMasterVolume(float _Volume)
    {
        m_MasterVolume = Mathf.Clamp01(_Volume); // 0~1로 제한

        foreach (var sound in m_Sounds)
        {
            sound.SetVolumn(m_MasterVolume * sound.m_Volum);
        }
    }


    public void Random_SoundPlay(GameObject _targetObject, int _StartIndex, int _SoundCount)
    {
        // 범위 내에서 랜덤 인덱스 선택
        int index = Random.Range(_StartIndex, _StartIndex + _SoundCount);

        if (index >= 0 && index < m_Sounds.Length)
        {
            // 해당 사운드를 찾아, targetObject의 AudioSource에서 재생
            Sound sound = m_Sounds[index];
            AudioSource source = _targetObject.GetComponent<AudioSource>();
            

            if (source == null)
            {
                // AudioSource가 없으면 추가
                source = _targetObject.AddComponent<AudioSource>();
            }



            // Sound 설정을 적용한 후, 재생
            sound.SetSource(source);
            sound.Play();
        }
    }

    public void Random_SoundOnShot(GameObject _targetObject, int _StartIndex, int _SoundCount, float _volumeScale = 1.0f)
    {
        // 범위 내에서 랜덤 인덱스 선택
        int index = Random.Range(_StartIndex, _StartIndex + _SoundCount);

        if (index >= 0 && index < m_Sounds.Length)
        {
            // 랜덤으로 선택된 사운드
            Sound sound = m_Sounds[index];

            // 타겟 객체에서 AudioSource 가져오기 또는 추가
            AudioSource source = _targetObject.GetComponent<AudioSource>();
            if (source == null)
            {
                source = _targetObject.AddComponent<AudioSource>();
            }

            // MasterVolume 적용하여 PlayOneShot으로 재생
            source.PlayOneShot(sound.m_Clip, m_MasterVolume * _volumeScale);
        }
        else
        {
            Debug.LogWarning("랜덤 사운드 인덱스가 잘못되었습니다.");
        }
    }


}

