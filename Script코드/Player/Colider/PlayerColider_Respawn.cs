using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColider_ResapwnZone : Player_Colider
{
    public bool m_Active = false;
    protected override void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Respawn_Point"))
        {
            RespawnPoint point = other.GetComponent<RespawnPoint>();

            if (point != null)
            {
                Respawn_Manager.m_Instance.Activate_RespawnPoint(point);
            }
            
           
        }
    }
    protected override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Respawn_Point"))
        {


            Respawn_Manager.m_Instance.Update_StayTime();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Respawn_Point"))
        {
            Respawn_Manager.m_Instance.Deactivate_RespawnPoint();
        }
    }



}
