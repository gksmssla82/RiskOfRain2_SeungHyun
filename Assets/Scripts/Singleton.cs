using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = (T)FindAnyObjectByType(typeof(T));

                if (m_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    m_instance = obj.GetComponent<T>();
                }
            }

            return m_instance;
        }
    }

    protected virtual void Awake()
    {
        // 중복 생성 방지
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        m_instance = this as T;

        // DontDestroyOnLoad 설정
        if (transform.parent != null && transform.root != null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}

