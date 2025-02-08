using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamagePopUp : MonoBehaviour
{
    public static DamagePopUp m_Instace;
    public GameObject m_Prefab;


    private void Awake()
    {
        m_Instace = this;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F10))
        {
            Debug.Log("CreatePopUp");
            CreatePopUp(new Vector3(407.86f,15.63f,282.21f), Random.Range(0, 1000).ToString(), Color.yellow);
        }
    }

    public void CreatePopUp(Vector3 _Position, string _Text, Color _Color)
    {
        
        // Var =  변수형식을 컴파일러가 정해줌
        var PopUp = Instantiate(m_Prefab, _Position, Quaternion.identity);
        var Temp = PopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Material TextMaterial = Temp.fontMaterial;
        Temp.text = _Text;
        Temp.faceColor = _Color;
        TextMaterial.SetColor("_GlowColor", _Color);

        // 삭제시간
        Destroy(PopUp, 1f);
    }
}
