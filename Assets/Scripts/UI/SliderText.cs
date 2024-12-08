using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderText : MonoBehaviour
{
    [SerializeField] private List<Slider> m_Sliders;
    [SerializeField] private List<TextMeshProUGUI> m_ValueText;


    private void Start()
    {
        if (m_Sliders.Count != m_ValueText.Count)
        {
            Debug.Log("슬라이더와 텍스트의 갯수가 맞지 않습니다.");
            return;
        }

        for (int i = 0; i < m_Sliders.Count; i++)
        {
            int index = i;
            m_Sliders[i].onValueChanged.AddListener(value => UpdateSliderValue(index, value));
            UpdateSliderValue(index, m_Sliders[i].value);
            
        }
    }

    private void UpdateSliderValue(int _index, float _Value)
    {
        m_ValueText[_index].text = _Value.ToString("F2");
    }

}
