using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sc_ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Player1, Player2;
    private int PlayerID;

    [SerializeField] sc_GameManager ManagerInstance = sc_GameManager.instance;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }
        InitializeScores();
        sc_BallLogic.ScoreIncrease += UpdateScore;
    }

    private void InitializeScores()
    {
        Player1.text = ManagerInstance.P1Score.ToString();
        Player2.text = ManagerInstance.P2Score.ToString();
    }

    void UpdateScore(int ID)
    {
        PlayerID = ID;
        if (PlayerID == ManagerInstance.P1ID)
        {
            Player1.text = ManagerInstance.P1Score.ToString();
        }
        else if (PlayerID == ManagerInstance.P2ID)
        {
            Player2.text = ManagerInstance.P2Score.ToString();
        }
    }
}
