using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_SinglePlayerController : MonoBehaviour
{
    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;

    [SerializeField] private float speed;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    //Gets Input of Y axis (based on pong rules only up/down movement)
    private void HandleInput()
    {
        Vector2 moveVector = new Vector2(GetXInput(), GetYInput()); //Sets a movement vector for where it will update to

        Vector3 target = transform.position + (new Vector3(moveVector.x, moveVector.y, 0f)); //Sets the new target direction by updating transform target 
        target.y = Mathf.Clamp(target.y, -ManagerInstance.WorldHeight + (transform.localScale.y / 2), ManagerInstance.WorldHeight - (transform.localScale.y / 2)); //Clamps y value between board height

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime); //Updates actual transform
    }

    private float GetXInput()
    {
        return 0f;
    }

    private float GetYInput()
    {
        float moveY = Input.GetAxisRaw("Vertical"); //in-built unity method, will return 0, -1 or 1 (nothing in between for snappy movement)

        return moveY;
    }
}
