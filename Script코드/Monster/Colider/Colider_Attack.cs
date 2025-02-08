using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colider_Attack : Monster_Colider
{
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player_Hitbox"))
        {
            m_Player.Take_Damage(m_Monster.Calculate_Damage());
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
       
    }

    protected override void OnTriggerStay(Collider other)
    {
        
    }

   
}
