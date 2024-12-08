using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    public float m_MoveSpeed { get; set; }
    public bool m_isMove { get;}

    public void Move();

}
