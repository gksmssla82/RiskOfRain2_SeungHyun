using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : Singleton<CursorManager>, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Texture2D m_DefaultCursor;
    [SerializeField] private Texture2D m_HoverCursor;


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
        if (eventData.pointerEnter.name == "OutLine")
        { 
            Set_Hover_Cursor();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Set_Default_Cursor();
    }
}
