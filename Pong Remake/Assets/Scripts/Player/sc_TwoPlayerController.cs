using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_TwoPlayerController : MonoBehaviour
{
    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;
    public float speed;
    [SerializeField] private int playerID;

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
        if (!ManagerInstance.GameReset)
        {
            HandleInput();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0), 3.0f * Time.deltaTime);
        }
    }

    #region Input
    //Gets Input of Y axis (based on pong rules only up/down movement)
    private void HandleInput()
    {
        Vector2 moveVector = new Vector2(GetXInput(), GetYInput()); //Sets a movement vector for where it will update to

        Vector3 target = transform.position + (new Vector3(moveVector.x, moveVector.y, 0f)); //Sets the new target direction by updating transform target 
        //4.25 //- 4.25
        target.y = Mathf.Clamp(target.y, -ManagerInstance.WorldHeight + (transform.lossyScale.y / 2) + 0.25f, ManagerInstance.WorldHeight - (transform.lossyScale.y / 2) - 0.25f);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime); //Updates actual transform
    }

    private float GetXInput()
    {
        return 0f;
    }

    private float GetYInput()
    {
        //float moveY = Input.GetAxisRaw("Vertical"); //in-built unity method, will return 0, -1 or 1 (nothing in between for snappy movement)
        float moveY = default;
        if (playerID == 0)
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveY = 1;
            }else if (Input.GetKey(KeyCode.S))
            {
                moveY = -1;
            }
        }else if(playerID == 1)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveY = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                moveY = -1;
            }
        }

        return moveY;
    }
    #endregion
}
