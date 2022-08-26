using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_PlayerCollider : MonoBehaviour
{
    public enum SinglePlayerCollision
    {
        Top,
        Middle,
        Bottom
    }

    public SinglePlayerCollision Position;
}
