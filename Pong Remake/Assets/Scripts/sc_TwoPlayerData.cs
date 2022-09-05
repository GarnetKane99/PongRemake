using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_TwoPlayerData : MonoBehaviour
{
    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;
    [SerializeField] private sc_ScoreController ScoreController;
    [SerializeField] private sc_BallLogic BallLogic;
    [SerializeField] private sc_TwoPlayerController PlayerLeft;
    [SerializeField] private sc_TwoPlayerController PlayerRight;
    [SerializeField] private sc_CountdownDisplay Countdown;
    [SerializeField] private sc_PostGameDisplay PGDisplay;
    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }
        ManagerInstance.ScoreHandler = ScoreController;
        ManagerInstance.BallLogic = BallLogic;
        ManagerInstance.PlayerLeft = PlayerLeft;
        ManagerInstance.PlayerRight = PlayerRight;
        ManagerInstance.DisplayCountdown = Countdown;
        ManagerInstance.PGDisplay = PGDisplay;
    }
}
