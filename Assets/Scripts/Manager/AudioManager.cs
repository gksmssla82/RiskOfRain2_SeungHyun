 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region SoundClass
[System.Serializable] // Ŀ���� Ŭ������ ����Ƽ���� ������ ���ٲٴ°� �ٿ�
public class Sound
{
    public string m_Name; // �����̸�

    public AudioClip m_Clip; // ���� ����
    public AudioSource m_Source; // ���� �÷��̾�
   
    public float m_Volum;
    public float m_Pitch;
    public bool m_Loop;
    public bool m_isUI = false;

    public void SetSource(AudioSource _Source)
    {
        if (_Source == null)
        {
            Debug.LogError("AudioSource�� �����ϴ�");
            return;
        }

        m_Source = _Source;
        m_Source.clip = m_Clip;
        m_Source.loop = m_Loop;
        m_Source.volume = AudioManager.m_Instnace.m_MasterVolume * m_Volum;

        m_Source.spatialBlend = m_isUI ? 0f : 1f; // 2D/3D ���� ����

        m_Source.minDistance = 13f; // �ּ� �Ÿ� (�ִ� ����)
        m_Source.maxDistance = 70f; // �ִ� �Ÿ� (�ּ� ����)
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
        // �̱��� ����
        if (m_Instnace == null)
        {
            m_Instnace = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ������Ʈ ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� ���� ����
            return;
        }

       

    }
    private void Start()
    {
        m_SoundDictionary = new Dictionary<string, Sound>();

        for (int i = 0; i < m_Sounds.Length; i++)
        {
            GameObject SoundObject = new GameObject("���� ���� �̸� : " + i + " = " + m_Sounds[i].m_Name);
            AudioSource source = SoundObject.AddComponent<AudioSource>();
            m_Sounds[i].SetSource(source);
            SoundObject.transform.SetParent(this.transform);

            // Dictionary�� �߰�
            m_SoundDictionary.Add(m_Sounds[i].m_Name, m_Sounds[i]);
        }
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            m_Volume += 0.1f;
            SetMasterVolume(m_Volume);
            Debug.Log("Vfx�������� = " + m_MasterVolume);

        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            m_Volume -= 0.1f;
            SetMasterVolume(m_Volume);
            Debug.Log("Vfx�������� = " + m_MasterVolume);

        }
    }

    // Ư�� ������Ʈ�� AudioSource ����
    public void InitializeSoundForObject(GameObject _targetObject, string _soundName)
    {
        if (m_SoundDictionary.TryGetValue(_soundName, out Sound sound))
        {
            AudioSource source = _targetObject.AddComponent<AudioSource>();
            sound.SetSource(source);
        }
        else
        {
            Debug.LogWarning("���� " + _soundName + "�� ã�� �� �����ϴ�.");
        }
    }

   

    public void Play(GameObject _targetObject, string _soundName)
    {
        if (m_SoundDictionary.TryGetValue(_soundName, out Sound sound))
        {
            AudioSource source = _targetObject.GetComponent<AudioSource>();

            if (source == null)
            {
                // AudioSource�� ������ �߰�
                source = _targetObject.AddComponent<AudioSource>();
            }

            sound.SetSource(source);
            sound.Play();
            

        }
        else
        {
            Debug.LogWarning("���� " + _soundName + "�� ã�� �� �����ϴ�.");
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
                // AudioSource�� ������ �߰�
                source = audioObject.AddComponent<AudioSource>();
            }

            audioObject.transform.position = _targetObject.transform.position;

            sound.SetSource(source);
            sound.Play();

            Destroy(audioObject, sound.m_Clip.length);
        }
        else
        {
            Debug.LogWarning("���� " + _soundName + "�� ã�� �� �����ϴ�.");
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

            // MasterVolume ����
            source.PlayOneShot(sound.m_Clip, m_MasterVolume * _volumeScale);
        }
        else
        {
            Debug.LogWarning("���� " + _soundName + "�� ã�� �� �����ϴ�.");
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
            Debug.LogWarning("���� " + _soundName + "�� ã�� �� �����ϴ�.");
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
            Debug.LogWarning("���� " + _soundName + "�� ã�� �� �����ϴ�.");
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
            Debug.LogWarning("���� " + _soundName + "�� ã�� �� �����ϴ�.");
        }
    }


    
    public void SetMasterVolume(float _Volume)
    {
        m_MasterVolume = Mathf.Clamp01(_Volume); // 0~1�� ����

        foreach (var sound in m_Sounds)
        {
            sound.SetVolumn(m_MasterVolume * sound.m_Volum);
        }
    }


    public void Random_SoundPlay(GameObject _targetObject, int _StartIndex, int _SoundCount)
    {
        // ���� ������ ���� �ε��� ����
        int index = Random.Range(_StartIndex, _StartIndex + _SoundCount);

        if (index >= 0 && index < m_Sounds.Length)
        {
            // �ش� ���带 ã��, targetObject�� AudioSource���� ���
            Sound sound = m_Sounds[index];
            AudioSource source = _targetObject.GetComponent<AudioSource>();
            

            if (source == null)
            {
                // AudioSource�� ������ �߰�
                source = _targetObject.AddComponent<AudioSource>();
            }



            // Sound ������ ������ ��, ���
            sound.SetSource(source);
            sound.Play();
        }
    }

    public void Random_SoundOnShot(GameObject _targetObject, int _StartIndex, int _SoundCount, float _volumeScale = 1.0f)
    {
        // ���� ������ ���� �ε��� ����
        int index = Random.Range(_StartIndex, _StartIndex + _SoundCount);

        if (index >= 0 && index < m_Sounds.Length)
        {
            // �������� ���õ� ����
            Sound sound = m_Sounds[index];

            // Ÿ�� ��ü���� AudioSource �������� �Ǵ� �߰�
            AudioSource source = _targetObject.GetComponent<AudioSource>();
            if (source == null)
            {
                source = _targetObject.AddComponent<AudioSource>();
            }

            // MasterVolume �����Ͽ� PlayOneShot���� ���
            source.PlayOneShot(sound.m_Clip, m_MasterVolume * _volumeScale);
        }
        else
        {
            Debug.LogWarning("���� ���� �ε����� �߸��Ǿ����ϴ�.");
        }
    }


}

