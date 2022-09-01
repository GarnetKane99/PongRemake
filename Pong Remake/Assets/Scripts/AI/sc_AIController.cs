using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_AIController : MonoBehaviour
{
    //[SerializeField] private sc_BallLogic Ball;
    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;

    [SerializeField] private float PaddleSpeed;

    [SerializeField] private Vector2 velocity;

    [SerializeField] private Vector3 TargetPosition;

    [SerializeField] private GameObject Debugger;


    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }
        sc_BallLogic.BallHit += PredictPath;
        sc_BallLogic.WallHit += AdjustPath;
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
        }
    }

    #region Ball Prediction Calculation
    void PredictPath(sc_BallLogic Ball)
    {
        Vector2 PredictedPosition = new Vector2(transform.position.x, Ball.Velocity.y * Random.Range(13.5f, 14.5f) + Ball.transform.position.y);    //predicted position is where the ball will end up based on the velocity calculation
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

        velocity.y = Mathf.Clamp(velocity.y, -2.0f, 2.0f);

        TargetPosition = PredictedPosition;
    }

    void AdjustPath(sc_BallLogic Ball)
    {
        Debug.Log("Wall Hit");

        Vector2 PredictedPosition = new Vector2(transform.position.x, Ball.Velocity.y * Mathf.Abs(Ball.transform.position.x - transform.position.x) + Ball.transform.position.y);    //predicted position is where the ball will end up based on the velocity calculation
        //time formula
        Instantiate(Debugger, PredictedPosition, Quaternion.identity);
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

        velocity.y /= PredictedPosition.y > ManagerInstance.WorldHeight - 0.75f ? 2 : PredictedPosition.y < -ManagerInstance.WorldHeight + 0.75f ? 2 : 1;

        velocity.y = Mathf.Clamp(velocity.y, -2.0f, 2.0f);

        TargetPosition = PredictedPosition;
    }

    #endregion

    void CheckPosition()
    {
        if (transform.position.y > ManagerInstance.WorldHeight - 0.75f)
        {
            velocity.y = 0f;
            //TargetPosition = new Vector3(0, 0, 0);
            transform.position = new Vector3(transform.position.x, ManagerInstance.WorldHeight - .75f);
        }
        else if (transform.position.y < -ManagerInstance.WorldHeight + .75f)
        {
            velocity.y = 0f;
            //TargetPosition = new Vector3(0, 0, 0);
            transform.position = new Vector3(transform.position.x, -ManagerInstance.WorldHeight + .75f);
        }

        if (Vector2.Distance(transform.position, TargetPosition) <= 0.1f)
        {
            velocity.y = velocity.y / 2;
        }
    }
}
