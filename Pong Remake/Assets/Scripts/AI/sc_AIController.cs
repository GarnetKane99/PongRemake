using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDifficulty
{
    EASY,
    MEDIUM,
    HARD
}

public class sc_AIController : MonoBehaviour
{
    //[SerializeField] private sc_BallLogic Ball;
    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;

    [SerializeField] private float PaddleSpeed;

    [SerializeField] private Vector2 velocity;

    [SerializeField] private Vector3 TargetPosition;

    [SerializeField] private GameObject Debugger;
    public EnemyDifficulty AIDifficulty;
    [SerializeField] private float yPredictionMin;
    [SerializeField] private float yPredictionMax;
    [SerializeField] private float velMin;
    [SerializeField] private float velMax;

    private sc_BallLogic Ball;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }

        Ball = FindObjectOfType<sc_BallLogic>();

        yPredictionMin = 13;
        yPredictionMax = 15;
        velMin = -1.0f;
        velMax = 1.0f;
        sc_BallLogic.BallHit += PredictPath;
        sc_BallLogic.WallHit += AdjustPath;
        AIDifficulty = EnemyDifficulty.EASY;
    }

    public void RemoveDelegate()
    {
        sc_BallLogic.BallHit -= PredictPath;
        sc_BallLogic.WallHit -= AdjustPath;
    }

    public void UpdateBrain(EnemyDifficulty NewDifficulty)
    {
        AIDifficulty = NewDifficulty;
        switch (AIDifficulty)
        {
            case EnemyDifficulty.EASY:
                velMin = -1.0f;
                velMax = 1.0f;
                yPredictionMin = 13;
                yPredictionMax = 15;
                break;
            case EnemyDifficulty.MEDIUM:
                velMin = -2.0f;
                velMax = 2.0f;
                yPredictionMin = 13.5f;
                yPredictionMax = 14.5f;
                break;
            case EnemyDifficulty.HARD:
                velMin = -3.0f;
                velMax = 3.0f;
                yPredictionMin = 13.75f;
                yPredictionMax = 14.25f;
                break;
        }
    }

    void Update()
    {
        if (!ManagerInstance.GameReset)
        {
            CheckPosition();
            transform.Translate(velocity * PaddleSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, 0), 3.0f * Time.deltaTime);
            velocity.y = 0;
        }
    }

    #region Ball Prediction Calculation
    void PredictPath(sc_BallLogic Ball)
    {
        Vector2 PredictedPosition = new Vector2(transform.position.x, Ball.Velocity.y * Random.Range(yPredictionMin, yPredictionMax) + Ball.transform.position.y);    //predicted position is where the ball will end up based on the velocity calculation
        Instantiate(Debugger, PredictedPosition, Quaternion.identity);
        //time formula
        float TimeToDistance = (Vector2.Distance(Ball.transform.position, PredictedPosition)) / Ball.CurrentBallSpeed;  //this equates to the AI knowing how long it has to reach target destination

        float PaddleToPosition = (Vector2.Distance(transform.position, PredictedPosition)) / PaddleSpeed;

        if (transform.position.y > PredictedPosition.y)
        {
            velocity.y = -PaddleToPosition / TimeToDistance;
        }
        else if (transform.position.y < PredictedPosition.y)
        {
            velocity.y = PaddleToPosition / TimeToDistance;
        }


        if (AIDifficulty != EnemyDifficulty.EASY)
        {
            velocity.y /= PredictedPosition.y > ManagerInstance.WorldHeight - 0.75f ? 1.5f : PredictedPosition.y < -ManagerInstance.WorldHeight + 0.75f ? 1.5f : 1;
        }

        velocity.y = Mathf.Clamp(velocity.y, velMin, velMax);

        TargetPosition = PredictedPosition;
    }

    void AdjustPath(sc_BallLogic Ball)
    {
        Vector2 PredictedPosition = new Vector2(transform.position.x, Ball.Velocity.y * Mathf.Abs(Ball.transform.position.x - transform.position.x) + Ball.transform.position.y);    //predicted position is where the ball will end up based on the velocity calculation
        Instantiate(Debugger, PredictedPosition, Quaternion.identity);
        //time formula
        float TimeToDistance = (Vector2.Distance(Ball.transform.position, PredictedPosition)) / Ball.CurrentBallSpeed;  //this equates to the AI knowing how long it has to reach target destination

        float PaddleToPosition = (Vector2.Distance(transform.position, PredictedPosition)) / PaddleSpeed;

        if (transform.position.y > PredictedPosition.y)
        {
            velocity.y = -PaddleToPosition / TimeToDistance;
        }
        else if (transform.position.y < PredictedPosition.y)
        {
            velocity.y = PaddleToPosition / TimeToDistance;
        }

        if (AIDifficulty != EnemyDifficulty.EASY)
        {
            velocity.y /= PredictedPosition.y > ManagerInstance.WorldHeight - 0.75f ? 2 : PredictedPosition.y < -ManagerInstance.WorldHeight + 0.75f ? 2 : 1;
        }

        velocity.y = Mathf.Clamp(velocity.y, velMin, velMax);

        TargetPosition = PredictedPosition;
    }

    #endregion

    void CheckPosition()
    {
        if (transform.position.y > ManagerInstance.WorldHeight + (transform.lossyScale.y / 2) - 0.25f)
        {
            velocity.y = 0f;
            transform.position = new Vector3(transform.position.x, ManagerInstance.WorldHeight + (transform.lossyScale.y / 2) - 0.25f);
        }
        else if (transform.position.y < -ManagerInstance.WorldHeight + (transform.lossyScale.y / 2) + 0.25f)
        {
            velocity.y = 0f;
            transform.position = new Vector3(transform.position.x, -ManagerInstance.WorldHeight + (transform.lossyScale.y / 2) + 0.25f);
        }

        if (Vector2.Distance(transform.position, TargetPosition) <= 0.1f)
        {
            velocity.y = velocity.y / 2;
        }
        if (Ball.Velocity.x < 0 && AIDifficulty == EnemyDifficulty.HARD)
        {
            //go back to center
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, Ball.transform.position.y), (PaddleSpeed / 2) * Time.deltaTime);
        }
    }
}
