using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookUI : MonoBehaviour
{

    private Camera m_Cam;
    // Start is called before the first frame update
    void Start()
    {
        if (m_Cam == null)
        {
            m_Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
     if (m_Cam != null)
        {
            transform.LookAt(transform.position + m_Cam.transform.rotation * Vector3.forward,
                m_Cam.transform.rotation * Vector3.up);
        }
    }
}
