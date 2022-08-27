using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_BallLogic : MonoBehaviour
{
    [SerializeField] private sc_GameManager GameManager = sc_GameManager.instance;

    //Single Player Ball Logic
    [SerializeField] private GameObject Player, AI;

    [Header("Ball Properties")]

    [SerializeField] private float CurrentBallSpeed;
    [SerializeField] private float MaxBallSpeed;
    [SerializeField] private float MinBallSpeed;

    [Header("Directional Properties")]

    [SerializeField] private Vector3 OldPosition;
    [SerializeField] private Vector3 NewPosition;
    [SerializeField] private int TrueDirection;

    [SerializeField] private Vector3 Velocity;

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
        OldPosition = NewPosition;

        //NewPosition = Random.value > 0.5f ? new Vector3(-GameManager.WorldWidth, Random.Range(-GameManager.WorldHeight, GameManager.WorldHeight)) :
        //    new Vector3(GameManager.WorldWidth, Random.Range(-GameManager.WorldHeight, GameManager.WorldHeight));
        // NewPosition = Player.transform.position;
        //Velocity = (Player.transform.position - transform.position).normalized * MinBallSpeed;
        //Velocity = Random.value > 0.5f ? new Vector3
        Vector2 RandomDir = new Vector2(Random.Range(0.3f, 1.0f) * Random.value > 0.5f ? -1 : 1, Random.Range(-1.0f, 1.0f)).normalized * MinBallSpeed;
        Velocity = RandomDir;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(0, 0, 0);
            NewPosition = Player.transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            //Velocity = NewPosition;
            //Velocity = new Vector3(-Velocity.x, Velocity.y, 0);

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
                TrueDirection = DirToGo < 0 ? Mathf.RoundToInt(DirToGo - GameManager.PaddleOffset) : Mathf.RoundToInt(DirToGo + GameManager.PaddleOffset);
                //Check if bounce is in positive velocity (ball going up)
                if (Mathf.Sign(Velocity.y) > 0)
                {
                    float _theta = Mathf.Atan(-Velocity.y / Velocity.x);

                    float _vMaxY = -Velocity.x * Mathf.Sin(_theta) + Velocity.y * Mathf.Cos(_theta);

                    float randY = Random.Range(Velocity.y, _vMaxY);

                    if (_vMaxY < Velocity.y)
                    {
                        Debug.Log("Fuck");
                    }

                    switch (TrueDirection)
                    {
                        case 0:
                            Velocity = new Vector3(-Velocity.x, Random.Range(Velocity.y - 0.2f, Velocity.y + 0.2f), 0);
                            break;

                        case 1:
                            Velocity = randY > 0 ? new Vector2(-Velocity.x, randY) : new Vector2(-Velocity.x, -randY);
                            CurrentBallSpeed++;
                            break;
                        case -1:
                            Velocity = randY > 0 ? new Vector2(-Velocity.x, -randY) : new Vector2(-Velocity.x, randY);
                            CurrentBallSpeed++;
                            break;
                    }
                }
                //Check if bounce is in negative velocity (ball going down)
                else if (Mathf.Sign(Velocity.y) < 0)
                {
                    float _theta = 2 * Mathf.PI - Mathf.Atan(-Velocity.y / Velocity.x);

                    float _vMaxY = -Velocity.x * Mathf.Sin(_theta) + Velocity.y * Mathf.Cos(_theta);

                    float randY = Random.Range(Velocity.y, _vMaxY);

                    if (_vMaxY < Velocity.y)
                    {
                        Debug.Log("Fuck");
                    }

                    switch (TrueDirection)
                    {
                        case 0:
                            Velocity = new Vector3(-Velocity.x, Random.Range(Velocity.y - 0.2f, Velocity.y + 0.2f), 0);
                            break;

                        case 1:
                            Velocity = randY > 0 ? new Vector2(-Velocity.x, randY) : new Vector2(-Velocity.x, -randY);
                            CurrentBallSpeed++;
                            break;
                        case -1:
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
