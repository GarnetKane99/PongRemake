using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sc_PostGameDisplay : MonoBehaviour
{
    [SerializeField] sc_GameManager ManagerInstance = sc_GameManager.instance;
    [SerializeField] private GameObject PostGameObjects;
    [SerializeField] private TextMeshProUGUI PlayerXWinText;
    [SerializeField] private Button Play, Back;

    private void Awake()
    {
        if(ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }

        Play.onClick.AddListener(ReplayGame);
        Back.onClick.AddListener(LeaveGame);
    }

    public void DisplayWinner(int PlayerID)
    {
        PostGameObjects.SetActive(true);
        PlayerXWinText.text = "Player " + (PlayerID+1) + " wins!";
        InvokeRepeating("TextFlash", 0, 0.75f);
    }

    public void TextFlash()
    {
        if (PlayerXWinText.color == Color.white)
        {
            PlayerXWinText.color = new Color(0, 0, 0, 0);
        }
        else
        {
            PlayerXWinText.color = new Color(1, 1, 1, 1);
        }
    }

    private void ReplayGame()
    {
        CancelInvoke("TextFlash");
        PostGameObjects.SetActive(false);
        ManagerInstance.GameStart();
    }

    private void LeaveGame()
    {
        ManagerInstance.LeaveGame();
        //SceneManager.LoadScene("sce_MainMenu");
        Invoke("ReturnToMenu", 0.5f);
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("sce_MainMenu");
    }
}
