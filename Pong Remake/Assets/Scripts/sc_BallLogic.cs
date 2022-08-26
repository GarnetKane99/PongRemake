using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_BallLogic : MonoBehaviour
{
    //Single Player Ball Logic
    [SerializeField] private GameObject Player, AI;

    [SerializeField] private float BallSpeed;

    [SerializeField] private Vector3 OldPosition, NewPosition;

    private bool CantHitPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("InitialMovement", 3.0f);
    }

    private void InitialMovement()
    {
        OldPosition = NewPosition;

        //NewPosition = Random.value > 0.5f ? Player.transform.position : AI.transform.position;
        NewPosition = Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != NewPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, NewPosition, BallSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            OldPosition = NewPosition;

            if (collision.collider.tag == "Player")
            {
                if (!CantHitPlayer)
                {
                    switch (collision.gameObject.GetComponent<sc_PlayerCollider>().Position)
                    {

                        case sc_PlayerCollider.SinglePlayerCollision.Top:
                            // where the * 2 determines strength of direction
                            NewPosition = OldPosition.y > 0 ? new Vector3(-OldPosition.x, OldPosition.y * (Random.Range(1, 20)), 0) : new Vector3(-OldPosition.x, -OldPosition.y * (Random.Range(1, 20)), 0);
                            break;
                        case sc_PlayerCollider.SinglePlayerCollision.Middle:
                            NewPosition = new Vector3(-OldPosition.x, OldPosition.y, 0);
                            break;
                        case sc_PlayerCollider.SinglePlayerCollision.Bottom:
                            // where the * 2 determines strength of direction
                            NewPosition = OldPosition.y > 0 ? new Vector3(-OldPosition.x, OldPosition.y * (Random.Range(-1, -20)), 0) : new Vector3(-OldPosition.x, -OldPosition.y * (Random.Range(-1, -20)), 0);
                            break;
                    }
                    CantHitPlayer = true;
                    Invoke("ResetHit", 0.5f);
                }
            }
            else if (collision.collider.tag == "Enemy")
            {
                if (!CantHitPlayer)
                {
                    switch (collision.gameObject.GetComponent<sc_PlayerCollider>().Position)
                    {

                        case sc_PlayerCollider.SinglePlayerCollision.Top:
                            // where the * 2 determines strength of direction
                            NewPosition = OldPosition.y > 0 ? new Vector3(-OldPosition.x, OldPosition.y * (Random.Range(1, 20)), 0) : new Vector3(-OldPosition.x, -OldPosition.y * (Random.Range(1, 20)), 0);
                            break;
                        case sc_PlayerCollider.SinglePlayerCollision.Middle:
                            NewPosition = new Vector3(-OldPosition.x, OldPosition.y, 0);
                            break;
                        case sc_PlayerCollider.SinglePlayerCollision.Bottom:
                            // where the * 2 determines strength of direction
                            NewPosition = OldPosition.y > 0 ? new Vector3(-OldPosition.x, OldPosition.y * (Random.Range(-1, -20)), 0) : new Vector3(-OldPosition.x, -OldPosition.y * (Random.Range(-1, -20)), 0);
                            break;
                    }
                    CantHitPlayer = true;
                    Invoke("ResetHit", 0.5f);
                }
            }
            else
            {
                //if (!CantHitPlayer)
                //{
                    NewPosition = -OldPosition.y > 0 ? new Vector3(NewPosition.x, Random.Range(-OldPosition.y / 6, 0), 0) :
                        new Vector3(NewPosition.x, Random.Range(-OldPosition.y / 6, 0), 0);
                   // Invoke("ResetHit", 0.5f);
                //}
            }
        }
    }

    private void ResetHit()
    {
        CantHitPlayer = false;
    }
}
