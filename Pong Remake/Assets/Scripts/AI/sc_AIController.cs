using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_AIController : MonoBehaviour
{
    [SerializeField] private GameObject Ball;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Ball.transform.position.y);
    }
}
