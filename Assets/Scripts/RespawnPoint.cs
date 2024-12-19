using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public bool m_IsActive = false;
    public GameObject m_Bettle;
    public GameObject m_Rumanian;
    public GameObject m_Golem;
    public float m_CoolDownTime = 5f;
    private float m_LastSpawnTime;
    

    public void Spawn_Monster()
    {
        if (Time.time >= m_LastSpawnTime + m_CoolDownTime)
        {
            Select_Monster();
            m_LastSpawnTime = Time.time;
        }
    }

    public void Select_Monster()
    {
        int Temp = Random.Range(0, 3);
        Debug.Log("Temp : " + Temp);

        switch (Temp)
        {
            case 0:
                Instantiate(m_Bettle, this.transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(m_Rumanian, this.transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(m_Golem, this.transform.position, Quaternion.identity);
                break;
        }
    }
}


