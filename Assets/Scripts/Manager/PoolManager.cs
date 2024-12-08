using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager m_Instance;

    [SerializeField] private GameObject[] m_Prefabs;
    private int m_PoolSize = 1;
    private List<GameObject>[] m_ObjPools;

    void Start()
    {
        m_Instance = this;

        Initialize_ObjPool();

    }

    private void Initialize_ObjPool()
    {
        m_ObjPools = new List<GameObject>[m_Prefabs.Length];

        for (int i = 0; i < m_Prefabs.Length; i++)
        {
            m_ObjPools[i] = new List<GameObject>();

            for (int j = 0; j < m_PoolSize; j++)
            {
                GameObject obj = Instantiate(m_Prefabs[i]);
                obj.SetActive(false);
                m_ObjPools[i].Add(obj);
            }
        }
    }

    public GameObject Activate_Object(int _Index)
    {
        GameObject obj = null;

        for (int i = 0; i < m_ObjPools[_Index].Count; i++)
        {
            if (!m_ObjPools[_Index][i].activeInHierarchy) // 하이어라키창 비활성 오브젝트 찾기
            {
                obj = m_ObjPools[_Index][i];
                obj.SetActive(true);
                return obj;
            }
        }

        obj = Instantiate(m_Prefabs[_Index]);
        m_ObjPools[_Index].Add(obj);
        obj.SetActive(true);

        return obj;
    }

    public void Set_ObjPosition(GameObject _Obj, Transform _Transform)
    {
        _Obj.transform.position = _Transform.position;
        
    }
}
 
