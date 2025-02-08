using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Text;
    [HideInInspector] public Animator m_Anim;
    public int m_Price;
    [SerializeField] private GameObject[] m_Items;
    private GameObject m_Select_Itme;
    [SerializeField] private float m_DestroyDelay = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        
        m_Anim = GetComponent<Animator>();
        Initialize_Price();
    }


  


    private void Initialize_Price()
    {
        m_Text.text = $"${m_Price}";
    }


    private void SelectRandom_Item()
    {
        int random = Random.Range(0, m_Items.Length);
        m_Select_Itme = m_Items[random];
        Vector3 SpawnPos = transform.position;
        SpawnPos.y += 2;
        Instantiate(m_Select_Itme, SpawnPos, Quaternion.identity);
    }

   


    public void Anim_BoxOpen()
    {
        SelectRandom_Item();
        StartCoroutine(Destroy_Box(m_DestroyDelay));
    }

    private IEnumerator Destroy_Box(float _Delay)
    {
        yield return new WaitForSeconds(_Delay);
        Destroy(gameObject);
    }
}
