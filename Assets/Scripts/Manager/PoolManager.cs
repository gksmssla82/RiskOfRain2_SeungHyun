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
            Debug.LogError("������ �̸��� Queue�� �����ϴ�. �̸��� Ȯ���ϼ���.");
            return null;
        }

        var queue = m_ObjectPools[_Name];

        GameObject obj;

        if (queue.Count > 0)
        {
            obj = queue.Dequeue(); // Ǯ���� ��������
        }
        else
        {
            // Ǯ ũ�� �ʰ� �� ���ο� ��ü ����
            GameObject prefab = GetPrefabByName(_Name);

            if (prefab == null)
            {
                Debug.LogError("�������� ���� �������� ���߽��ϴ�.");
                return null;
            }
            obj = Instantiate(prefab);
        }

        // ��ġ ������ Active
        obj.transform.position = _Position;
        obj.transform.rotation = _Rotation;
        obj.SetActive(true);

        // Rigidbody �ʱ�ȭ
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
        GameObject particle = ActivateObject(_Name, _Position, _Rotation); // ���� ActivateObject Ȱ��

        if (particle != null)
        {
            // ��ƼŬ �ڵ� ��Ȱ��ȭ ����
            var particleSystem = particle.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                StartCoroutine(DeactivateAfterDuration(_Name, particle, particleSystem.main.duration));
            }
            else
                Debug.LogError(_Name + " ��ƼŬ �ý��� ������Ʈ�� ã���� �����ϴ� �ش� ������Ʈ�� ��ƼŬ���� Ȯ���ϼ���.");
        }

        return particle;
    }

    public void DeactivateObject(string _Name, GameObject _Obj)
    {

        // Active ������ ������ ����
        _Obj.SetActive(false);
        _Obj.transform.position = Vector3.zero;
        _Obj.transform.rotation = Quaternion.identity;


        if (m_ObjectPools.ContainsKey(_Name))
        {
            m_ObjectPools[_Name].Enqueue(_Obj); // ��Ȱ��ȭ �� Ǯ�� �ǵ���
        }
        else
        {
            Destroy(_Obj); // Ǯ�� ���� ��ü�� ����
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
 
