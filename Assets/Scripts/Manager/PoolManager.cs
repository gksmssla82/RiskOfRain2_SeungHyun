using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
   
    [SerializeField] private GameObject[] m_Prefabs;
    private Dictionary<string, Queue<GameObject>> m_ObjectPools;
    private int m_PoolSize = 3;
  

    void Start()
    {
        Initialize_ObjPool();
    }

    private void Initialize_ObjPool()
    {
        m_ObjectPools = new Dictionary<string, Queue<GameObject>>();

       foreach (var prefab in m_Prefabs)
        {
            var queue = new Queue<GameObject>();

            for (int i = 0; i < m_PoolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            m_ObjectPools.Add(prefab.name, queue);
        }
    }

    public GameObject ActivateObject(string _Name, Vector3 _Position, Quaternion _Rotation)
    {
        if (!m_ObjectPools.ContainsKey(_Name))
        {
            Debug.LogError("프리펩 이름이 Queue에 없습니다. 이름을 확인하세요.");
            return null;
        }

        var queue = m_ObjectPools[_Name];

        GameObject obj;

        if (queue.Count > 0)
        {
            obj = queue.Dequeue(); // 풀에서 가져오기
        }
        else
        {
            // 풀 크기 초과 시 새로운 객체 생성
            GameObject prefab = GetPrefabByName(_Name);

            if (prefab == null)
            {
                Debug.LogError("프리팹을 새로 생성하지 못했습니다.");
                return null;
            }
            obj = Instantiate(prefab);
        }

        // 위치 설정후 Active
        obj.transform.position = _Position;
        obj.transform.rotation = _Rotation;
        obj.SetActive(true);

        // Rigidbody 초기화
        var rigid = obj.GetComponent<Rigidbody>();

        if (rigid != null)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }

        return obj;
    }

    public GameObject ActivateParticle(string _Name, Vector3 _Position, Quaternion _Rotation)
    {
        GameObject particle = ActivateObject(_Name, _Position, _Rotation); // 기존 ActivateObject 활용

        if (particle != null)
        {
            // 파티클 자동 비활성화 관리
            var particleSystem = particle.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                StartCoroutine(DeactivateAfterDuration(_Name, particle, particleSystem.main.duration));
            }
            else
                Debug.LogError(_Name + " 파티클 시스템 컴포넌트를 찾을수 없습니다 해당 오브젝트가 파티클인지 확인하세요.");
        }

        return particle;
    }

    public void DeactivateObject(string _Name, GameObject _Obj)
    {

        // Active 해제후 포지션 변경
        _Obj.SetActive(false);
        _Obj.transform.position = Vector3.zero;
        _Obj.transform.rotation = Quaternion.identity;


        if (m_ObjectPools.ContainsKey(_Name))
        {
            m_ObjectPools[_Name].Enqueue(_Obj); // 비활성화 후 풀로 되돌림
        }
        else
        {
            Destroy(_Obj); // 풀에 없는 객체는 삭제
        }
    }


    private IEnumerator DeactivateAfterDuration(string _Name, GameObject _Particle, float _Duration)
    {
        yield return new WaitForSeconds(_Duration);
        DeactivateObject(_Name, _Particle);
    }

    private GameObject GetPrefabByName(string prefabName)
    {
        foreach (var prefab in m_Prefabs)
        {
            if (prefab.name == prefabName)
            {
                return prefab;
            }
        }
        return null;
    }

}
 
