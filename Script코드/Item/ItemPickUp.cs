using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item m_Item;
    private float y;
    private void Update()
    {
        y += 50 * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, y, 0);
    }
}
