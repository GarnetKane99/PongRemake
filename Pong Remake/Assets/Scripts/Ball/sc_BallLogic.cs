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

    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;

    //Single Player Ball Logic
    [SerializeField] private GameObject PlayerOne, PlayerTwo;

    [SerializeField] private int HitCount;

    [Header("Ball Properties")]

    private float StartingBallSpeed;
    public float CurrentBallSpeed;
    [SerializeField] private float MaxBallSpeed;
    [SerializeField] private float MinBallSpeed;

    [Header("Directional Properties")]

    public Vector3 Velocity;
    [SerializeField] private PaddlePosition PaddleHitPosition;

    [SerializeField] private bool BotWall = false;
    [SerializeField] private bool TopWall = false;

    public delegate void OnBallHit(sc_BallLogic Ball);
    public static event OnBallHit BallHit;
    public delegate void OnWallHit(sc_BallLogic Ball);
    public static event OnWallHit WallHit;

    public delegate void OnScoreIncrease(int PlayerID);
    public static event OnScoreIncrease ScoreIncrease;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }

        ManagerInstance.SetupDelegate();
        MinBallSpeed = ManagerInstance.MinBallSpeed;
        MaxBallSpeed = ManagerInstance.MaxBallSpeed;
        CurrentBallSpeed = MinBallSpeed;
        StartingBallSpeed = CurrentBallSpeed;
    }

    //Randomises velocity direction for ball to go towards
    private void InitialMovement()
    {
        CurrentBallSpeed = StartingBallSpeed;
        MinBallSpeed = CurrentBallSpeed;
        transform.position = new Vector3(0, 0, 0);
        BotWall = false;
        TopWall = false;
        float randVal = Random.value;
        Vector2 RandomDir = new Vector2(Random.Range(0.3f, 1.0f) * randVal >= 0.5f ? -1 : 1, Random.Range(-0.5f, 0.5f)).normalized;
        Velocity = RandomDir;
        if (Velocity.x > 0)
        {
            if (WallHit != null)
            {
                WallHit(this);
            }
        }
        ManagerInstance.GameReset = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVelocity();
        if (!ManagerInstance.GameReset)
        {
            DetectCollisions();

            DisplayDistance();
        }
    }

    private void UpdateVelocity()
    {
        transform.Translate(Velocity * CurrentBallSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Function to detect collision based off of transforms
    /// </summary>
    private void DetectCollisions()
    {
        if (VerticalWallCollision()) //Method to check if ball collides with a wall
        {
            VerticalReflection(); //function to call that will allow a vertical reflection
        }

        if (Velocity.x <= 0)    //check if ball velocity is less than 0 (i.e. going left) *set <= so that instance of 0 is accounted =shouldn't ever occur=
        {
            if (transform.position.x <= (PlayerOne.transform.position.x + (PlayerOne.transform.lossyScale.x / 8)))    //check if ball current position is <= to player's position while taking into account the width of the paddle
            {
                BotWall = false;    //reset wall checks
                TopWall = false;
                if (Vector2.Distance(transform.position, PlayerOne.transform.position) < PlayerOne.transform.lossyScale.y / 2)    //check if distance between ball and paddle is less than the total height of the paddle (ensure paddle is in contact)
                {
                    HorizontalReflection(PlayerOne);   //Call Horizontal Reflection with reference to player
                }
                else
                {
                    if (transform.position.x < PlayerOne.transform.position.x - 0.25f)
                    {
                        ResetGame(1);
                        //Invoke("InitialMovement", 3.0f);
                    }
                }
            }
        }
        else if (Velocity.x > 0)    //check if ball velocity is more than 0 (i.e. going right)
        {
            if (transform.position.x >= (PlayerTwo.transform.position.x - (PlayerTwo.transform.lossyScale.x / 8)))    //check if ball current position is >= to ai's position while taking into account the width of the paddle
            {
                BotWall = false;    //reset wall checks
                TopWall = false;

                if (Vector2.Distance(transform.position, PlayerTwo.transform.position) < PlayerTwo.transform.lossyScale.y / 2) //check if distance between ball and paddle is less than the total height of the paddle (ensure paddle is in contact)
                {
                    HorizontalReflection(PlayerTwo);   //call Horizontal Reflection with reference to ai
                }
                else
                {
                    if (transform.position.x > PlayerTwo.transform.position.x + 0.25f)
                    {
                        ResetGame(0);
                        //Invoke("InitialMovement", 3.0f);
                    }
                }
            }
        }
    }

    //Method to check wall collision
    private bool VerticalWallCollision()
    {
        if (transform.position.y >= ManagerInstance.WorldHeight - 0.3130f || transform.position.y <= -ManagerInstance.WorldHeight + 0.3130f)    //Check if ball is >= or <= the screen limits (taking into account the border object)
        {
            if (Mathf.Sign(Velocity.y) <= 0) //check if ball is going down
            {
                if (!BotWall)   //ensure it doesn't try to collide with the bottom screen twice
                {
                    BotWall = true; //set bot wall check to true so that it doesn't return twice
                    TopWall = false;
                    return true;
                }
            }
            else if (Mathf.Sign(Velocity.y) > 0)    //check if ball is going up
            {
                if (!TopWall)   //ensure it doesn't try to collider with the top screen twice
                {
                    TopWall = true;
                    BotWall = false;
                    return true;
                }
            }
        }
        return false;
    }

    private void ResetGame(int playerID)
    {
        if (ScoreIncrease != null)
        {
            ScoreIncrease(playerID);
        }
    }

    //method with algorithm to deflect horizontally with some randomisation -> takes reference of which object it is deflecting
    private void HorizontalReflection(GameObject CollisionObject)
    {
        float DirToGo = (transform.position.y - CollisionObject.transform.position.y) / (CollisionObject.transform.lossyScale.y / 2);
        DirToGo = DirToGo < 0 ? Mathf.RoundToInt(DirToGo - ManagerInstance.PaddleOffset) : Mathf.RoundToInt(DirToGo + ManagerInstance.PaddleOffset);    //Used to check which part of the paddle is being hit
        PaddleHitPosition = DirToGo == -1 ? PaddlePosition.BOTTOM : DirToGo == 1 ? PaddlePosition.TOP : PaddlePosition.MIDDLE;  //PaddleHitPosition is updated based on float position (-1 = bot. of pad, 0 = mid. of pad, +1 = top of pad) 

        float _vMaxY = -Velocity.x * Mathf.Sin(Mathf.PI / 4) + Velocity.y * Mathf.Cos(Mathf.PI / 4);    //max direction that y can go

        float randY = Random.Range(Velocity.y, _vMaxY); //random direction that ball can travel towards

        switch (PaddleHitPosition)  //various cases depending on where on the paddle ball has hit
        {
            case PaddlePosition.MIDDLE:
                if (Mathf.Sign(Velocity.y) >= 0)    //Check if bounce is in positive velocity (ball going up)
                {
                    Velocity = new Vector3(-Velocity.x, Random.Range(0, 0.1f), 0);  //if middle, then return velocity will be similar to initial hit
                }
                else if (Mathf.Sign(Velocity.y) < 0) //Check if bounce is in negative velocity (ball going down)
                {
                    Velocity = new Vector3(-Velocity.x, Random.Range(-0.1f, 0), 0);
                }
                break;

            case PaddlePosition.TOP:
                Velocity = randY > 0 ? new Vector2(-Velocity.x, randY) : new Vector2(-Velocity.x, -randY);  //if top, also check if the randY velocity is > 0 -> this is so we know if we need to continue in the upwards velocity, or swap to negative velocity
                CurrentBallSpeed++; //increase ball speed on top and bottom paddle hits
                break;

            case PaddlePosition.BOTTOM:
                Velocity = randY > 0 ? new Vector2(-Velocity.x, -randY) : new Vector2(-Velocity.x, randY);  //if bot, also check if the randY velocity is > 0 -> this is so we know if we need to swap to negative velocity or continue in the upwards velocity
                CurrentBallSpeed++;
                break;
        }

        if (CollisionObject == PlayerOne)
        {
            if (BallHit != null)
            {
                BallHit(this);
            }
        }

        UpdateHitCount();
    }
    //hitcount is used to track the number of times the ball hits the paddle - this is so that the game does progressively get more difficult
    private void UpdateHitCount()
    {
        HitCount++;
        if (HitCount % 8 == 0)
        {
            MinBallSpeed++;
            if (MinBallSpeed > MaxBallSpeed)
            {
                MaxBallSpeed = MinBallSpeed;
            }
        }
        CurrentBallSpeed = Mathf.Clamp(CurrentBallSpeed, MinBallSpeed, MaxBallSpeed);
    }

    //vertical reflection mirrors the current y velocity in the opposite direction
    private void VerticalReflection()
    {
        Velocity = new Vector3(Velocity.x, -Velocity.y, 0);

        if (Random.value < 0.5f) //chance to slow the balls speed down when this occurs as well
        {
            CurrentBallSpeed--;
            CurrentBallSpeed = Mathf.Clamp(CurrentBallSpeed, MinBallSpeed, MaxBallSpeed);
        }

        if (Velocity.x > 0)
        {
            if (WallHit != null)
            {
                WallHit(this);
            }
        }
    }

    //Debugging
    private void DisplayDistance()
    {
        Debug.DrawLine(transform.position, PlayerOne.transform.position, Color.red);
        Debug.DrawLine(transform.position, PlayerTwo.transform.position, Color.red);
        Debug.DrawRay(transform.position, new Vector3(Velocity.x * 10, Velocity.y * 10, 0), Color.blue);
    }
}
