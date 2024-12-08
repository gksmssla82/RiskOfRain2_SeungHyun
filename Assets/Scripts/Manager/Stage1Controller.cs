using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (BgmManager.m_Instnace != null)
        {
            BgmManager.m_Instnace.Play(1);
        }
    }

 
}
