using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform m_BossSpawnPoint;
    [SerializeField] private GameObject m_SpawnBossPrepab;
    [SerializeField] private GameObject m_ActiveMessage;
    [SerializeField] private TextMeshProUGUI m_TextMessage;

    private Collider m_Collider;
    // Start is called before the first frame update
    void Start()
    {
        m_Collider = GetComponent<SphereCollider>();
    }

   
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("텔레포터 충돌");
            m_ActiveMessage.SetActive(true);
            m_TextMessage.text = "보스 소환";
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            m_Collider.enabled = false;
            m_ActiveMessage.SetActive(false);
            AudioManager.Instance.Play(gameObject, "Teleporter_BossSpawn");
            Instantiate(m_SpawnBossPrepab, m_BossSpawnPoint.position, Quaternion.identity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_ActiveMessage.SetActive(false);
        }
    }
}
