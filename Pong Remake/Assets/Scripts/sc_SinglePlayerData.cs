using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_SinglePlayerData : MonoBehaviour
{
    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;
    [SerializeField] private sc_ScoreController ScoreController;
    [SerializeField] private sc_BallLogic BallLogic;
    [SerializeField] private sc_SinglePlayerController SinglePlayerController;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }
        ManagerInstance.ScoreHandler = ScoreController;
        ManagerInstance.BallLogic = BallLogic;
        ManagerInstance.SinglePlayerController = SinglePlayerController;
    }
}
