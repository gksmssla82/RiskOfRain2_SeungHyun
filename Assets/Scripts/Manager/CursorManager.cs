using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static CursorManager m_Instance;
    [SerializeField] private Texture2D m_DefaultCursor;
    [SerializeField] private Texture2D m_HoverCursor;


    private void Awake()
    {
        // 싱글톤 설정
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 오브젝트 유지
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
            return;
        }
    }

    private void Start()
    {
        Set_Default_Cursor();
        Show_Cursor(true);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Set_Default_Cursor()
    {
        Cursor.SetCursor(m_DefaultCursor, Vector2.zero, CursorMode.Auto);
    }

    public void Set_Hover_Cursor()
    {
        Cursor.SetCursor(m_HoverCursor, Vector2.zero, CursorMode.Auto);
    }

    public void Show_Cursor(bool _Value)
    {
        Cursor.visible = _Value;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Name = " + eventData.pointerEnter.name);
        if (eventData.pointerEnter.name == "OutLine")
        {
            Debug.Log("아웃라인");
            Set_Hover_Cursor();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Set_Default_Cursor();
    }
}
