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
        // �̱��� ����
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ������Ʈ ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� ���� ����
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
            Debug.Log("�ƿ�����");
            Set_Hover_Cursor();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Set_Default_Cursor();
    }
}
