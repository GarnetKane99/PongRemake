using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class sc_BallLogic : MonoBehaviour
{
    private enum PaddlePosition
    {
        TOP,
        MIDDLE,
        BOTTOM
    }

    [SerializeField] private sc_GameManager GameManager = sc_GameManager.instance;

    //Single Player Ball Logic
    [SerializeField] private GameObject Player, AI;

    [Header("Ball Properties")]

    [SerializeField] private float CurrentBallSpeed;
    [SerializeField] private float MaxBallSpeed;
    [SerializeField] private float MinBallSpeed;

    [Header("Directional Properties")]

    [SerializeField] private Vector3 Velocity;
    [SerializeField] private PaddlePosition PaddleHitPosition;

    private void Awake()
    {
        if (GameManager == null)
        {
            GameManager = FindObjectOfType<sc_GameManager>();
        }

        MinBallSpeed = GameManager.MinBallSpeed;
        MaxBallSpeed = GameManager.MaxBallSpeed;
        CurrentBallSpeed = MinBallSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("InitialMovement", 3.0f);
    }

    private void InitialMovement()
    {
        Vector2 RandomDir = new Vector2(Random.Range(0.3f, 1.0f) * Random.value > 0.5f ? -1 : 1, Random.Range(-1.0f, 1.0f)).normalized;
        Velocity = RandomDir;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVelocity();

        //Debugging
        DisplayDistance();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    private void UpdateVelocity()
    {
        transform.Translate(Velocity * CurrentBallSpeed * Time.deltaTime);
    }

    private void DisplayDistance()
    {
        Debug.DrawLine(transform.position, Player.transform.position, Color.red);
        Debug.DrawLine(transform.position, AI.transform.position, Color.red);
        Debug.DrawRay(transform.position, new Vector3(Velocity.x * 10, Velocity.y * 10, 0), Color.blue);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            bool HorizontalReflection = false;  //Walls
            bool VerticalReflection = false;    //Player/Paddles

            if (collision.collider.tag == "Player" || collision.collider.tag == "Enemy")
            {
                VerticalReflection = true;
            }
            else if (collision.collider.tag == "Wall")
            {
                HorizontalReflection = true;
            }

            if (VerticalReflection)
            {
                //Velocity = new Vector3(-Velocity.x, Velocity.y, 0);
                float DirToGo = (transform.position.y - collision.transform.position.y) / (collision.transform.lossyScale.y / 2);
                DirToGo = DirToGo < 0 ? Mathf.RoundToInt(DirToGo - GameManager.PaddleOffset) : Mathf.RoundToInt(DirToGo + GameManager.PaddleOffset);
                PaddleHitPosition = DirToGo == -1 ? PaddlePosition.BOTTOM : DirToGo == 1 ? PaddlePosition.TOP : PaddlePosition.MIDDLE;
                //Check if bounce is in positive velocity (ball going up)
                if (Mathf.Sign(Velocity.y) > 0)
                {
                    float _vMaxY = -Velocity.x * Mathf.Sin(Mathf.PI/4) + Velocity.y * Mathf.Cos(Mathf.PI / 4);

                    float randY = Random.Range(Velocity.y, _vMaxY);

                    switch (PaddleHitPosition)
                    {
                        case PaddlePosition.MIDDLE:
                            Velocity = new Vector3(-Velocity.x, Random.Range(0, 0.1f), 0);
                            break;
                        case PaddlePosition.TOP:
                            Velocity = randY > 0 ? new Vector2(-Velocity.x, randY) : new Vector2(-Velocity.x, -randY);
                            CurrentBallSpeed++;
                            break;
                        case PaddlePosition.BOTTOM:
                            Velocity = randY > 0 ? new Vector2(-Velocity.x, -randY) : new Vector2(-Velocity.x, randY);
                            CurrentBallSpeed++;
                            break;
                    }
                }
                //Check if bounce is in negative velocity (ball going down)
                else if (Mathf.Sign(Velocity.y) < 0)
                {
                    float _vMaxY = -Velocity.x * Mathf.Sin(Mathf.PI / 4) + Velocity.y * Mathf.Cos(Mathf.PI / 4);

                    float randY = Random.Range(Velocity.y, _vMaxY);

                    switch (PaddleHitPosition)
                    {
                        case PaddlePosition.MIDDLE:
                            Velocity = new Vector3(-Velocity.x, Random.Range(-0.1f, 0), 0);
                            break;

                        case PaddlePosition.TOP:
                            Velocity = randY > 0 ? new Vector2(-Velocity.x, randY) : new Vector2(-Velocity.x, -randY);
                            CurrentBallSpeed++;
                            break;
                        case PaddlePosition.BOTTOM:
                            Velocity = randY > 0 ? new Vector2(-Velocity.x, -randY) : new Vector2(-Velocity.x, randY);
                            CurrentBallSpeed++;
                            break;
                    }
                }
            }
            if (HorizontalReflection)
            {
                Velocity = new Vector3(Velocity.x, -Velocity.y, 0);
            }

            CurrentBallSpeed = Mathf.Clamp(CurrentBallSpeed, MinBallSpeed, MaxBallSpeed);
        }
    }
}
