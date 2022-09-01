using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_GameManager : MonoBehaviour
{
    public static sc_GameManager instance { get; private set; }

    private Camera mainCamera;
    public float WorldHeight => mainCamera.orthographicSize;
    public float WorldWidth => WorldHeight * ((float)Screen.width / (float)Screen.height);

    //The paddle and ball properties are controlled by the Game Manager so that they can be configurable in the game prior to starting a game
    [Range(0, 0.5f)]
    [Tooltip("This is for the offset for the centering of the paddle to control how much of the paddle is considered the 'middle' for a horizontal rebound")]
    public float PaddleOffset;

    public float MaxBallSpeed, MinBallSpeed;

    public bool GameReset = false;

    public int P1Score = default, P2Score = default;
    public int P1ID = default, P2ID = default;

    public bool SinglePlayerMode = false;

    public delegate void ScoreIncreaser(int playerID);
    public static event ScoreIncreaser ScoreToIncrease;

    public sc_ScoreController ScoreHandler;
    public sc_AIController AIController;
    public sc_SinglePlayerController SinglePlayerController;

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;
        SinglePlayerMode = true;
    }

    public void SetupDelegate()
    {
        sc_BallLogic.ScoreIncrease += IncreaseScore;
    }

    public void IncreaseScore(int PlayerID)
    {
        GameReset = true;
        if (PlayerID == P1ID)
        {
            P1Score++;
        }
        else if (PlayerID == P2ID)
        {
            P2Score++;
        }
        ScoreHandler.UpdateScore(PlayerID);
    }
}
