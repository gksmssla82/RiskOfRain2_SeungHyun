using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Manager : MonoBehaviour
{
    public static Respawn_Manager m_Instance { get; private set; }

    public List<RespawnPoint> m_RespawnPoint = new List<RespawnPoint>();
    public float m_SpawnDuration = 8f;

    private RespawnPoint m_ActivePoint = null;
    private float m_StayTimer = 0f;

  

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Obsolete]
    void Start()
    {
        RespawnPoint[] points = FindObjectsOfType<RespawnPoint>(true);  // true = ��Ȱ�� ����
        m_RespawnPoint.AddRange(points);
    }

    public void Activate_RespawnPoint(RespawnPoint _Point)
    {
        if (m_ActivePoint != _Point)
        {
            m_ActivePoint = _Point;
            m_StayTimer = 0f; // Ÿ�̸� �ʱ�ȭ
            Debug.Log("Ȱ��ȭ�� Point = " + _Point.gameObject.name);
        }
    }

    public void Update_StayTime()
    {
        if (m_ActivePoint != null)
        {
            m_StayTimer += Time.deltaTime;

            if (m_StayTimer >= m_SpawnDuration)
            {
                m_ActivePoint.Spawn_Monster();
                Debug.Log(m_ActivePoint.gameObject.name + " ���� ���Ͱ� �����Ǿ����ϴ�");
                m_StayTimer = 0f;
            }
        }
    }

    public void Deactivate_RespawnPoint()
    {
        m_ActivePoint = null;
        m_StayTimer = 0f;
    }

    
  
}
