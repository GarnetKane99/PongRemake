using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class sc_preGameSetup : MonoBehaviour
{
    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;
    [SerializeField] private TextMeshProUGUI PlayerLogo;
    private Vector3 StartPos;
    private Vector3 EndPos;

    [Header("Player 1 Settings")]
    [SerializeField] private string[] PlayerOptions;
    [SerializeField] private TextMeshProUGUI PlayerPresetsText;
    [SerializeField] private Button PlayerOptLeft;
    [SerializeField] private Button PlayerOptRight;
    [SerializeField] private GameObject[] PlayerOptionsList;
    private int CurPlayerOptionPos = default;

    [Header("Player 2 Settings")]
    [SerializeField] private string[] Player2Options;
    [SerializeField] private TextMeshProUGUI Player2PresetsText;
    [SerializeField] private Button Player2OptLeft;
    [SerializeField] private Button Player2OptRight;
    [SerializeField] private GameObject[] Player2OptionsList;
    private int CurPlayer2OptionPos = default;

    [SerializeField] private GameObject OptionsCanvas;
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button BackButton;


    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }

        StartPos = Camera.main.WorldToScreenPoint(new Vector3(0, 4f, 0));
        EndPos = Camera.main.WorldToScreenPoint(new Vector3(0, 4.25f, 0));

        //PlayerData Setup
        SetPlayerText();
        PlayerOptLeft.onClick.AddListener(PlayerLeftClick);
        PlayerOptRight.onClick.AddListener(PlayerRightClick);
        PlayerOptionsList[CurPlayerOptionPos].SetActive(true);

        //Player 2 Setup
        SetAIText();
        Player2OptLeft.onClick.AddListener(Player2LeftClick);
        Player2OptRight.onClick.AddListener(Player2RightClick);
        Player2OptionsList[CurPlayer2OptionPos].SetActive(true);

        PlayButton.onClick.AddListener(PlayGame);
        BackButton.onClick.AddListener(LeaveGame);
        StartCoroutine(BounceTextUp());
    }

    private IEnumerator BounceTextUp()
    {
        float Timer = 0;
        float MaxTimer = 0.75f;
        while (Timer < MaxTimer)
        {
            PlayerLogo.transform.position = Vector3.Lerp(StartPos, EndPos, Timer / MaxTimer);
            Timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(BounceTextDown());
    }

    private IEnumerator BounceTextDown()
    {
        float Timer = 0;
        float MaxTimer = 0.75f;
        while (Timer < MaxTimer)
        {
            PlayerLogo.transform.position = Vector3.Lerp(EndPos, StartPos, Timer / MaxTimer);
            Timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(BounceTextUp());
    }

    private void PlayerLeftClick()
    {
        PlayerOptionsList[CurPlayerOptionPos].SetActive(false);
        if (CurPlayerOptionPos == 0)
        {
            CurPlayerOptionPos = PlayerOptions.Length - 1;
        }
        else
        {
            CurPlayerOptionPos -= 1;
        }

        PlayerOptionsList[CurPlayerOptionPos].SetActive(true);

        SetPlayerText();
    }

    private void PlayerRightClick()
    {
        PlayerOptionsList[CurPlayerOptionPos].SetActive(false);
        if (CurPlayerOptionPos == PlayerOptions.Length - 1)
        {
            CurPlayerOptionPos = 0;
        }
        else
        {
            CurPlayerOptionPos++;
        }
        PlayerOptionsList[CurPlayerOptionPos].SetActive(true);
        SetPlayerText();
    }

    private void SetPlayerText()
    {
        PlayerPresetsText.text = PlayerOptions[CurPlayerOptionPos];
    }

    private void Player2LeftClick()
    {
        Player2OptionsList[CurPlayer2OptionPos].SetActive(false);
        if (CurPlayer2OptionPos == 0)
        {
            CurPlayer2OptionPos = Player2Options.Length - 1;
        }
        else
        {
            CurPlayer2OptionPos -= 1;
        }

        Player2OptionsList[CurPlayer2OptionPos].SetActive(true);

        SetAIText();
    }

    private void Player2RightClick()
    {
        Player2OptionsList[CurPlayer2OptionPos].SetActive(false);
        if (CurPlayer2OptionPos == Player2Options.Length - 1)
        {
            CurPlayer2OptionPos = 0;
        }
        else
        {
            CurPlayer2OptionPos++;
        }
        Player2OptionsList[CurPlayer2OptionPos].SetActive(true);
        SetAIText();
    }


    private void SetAIText()
    {
        Player2PresetsText.text = Player2Options[CurPlayer2OptionPos];
    }

    private void PlayGame()
    {
        OptionsCanvas.SetActive(false);
        ManagerInstance.GameStart();
    }

    private void LeaveGame()
    {
        ManagerInstance.LeaveGame();
        Invoke("ReturnToMenu", 0.5f);
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("sce_MainMenu");
    }

}
