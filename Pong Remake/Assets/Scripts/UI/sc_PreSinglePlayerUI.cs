using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sc_PreSinglePlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI OnePlayerText;
    private Vector3 StartPos;
    private Vector3 EndPos;

    [SerializeField] private string[] PlayerOptions;
    [SerializeField] private TextMeshProUGUI PlayerPresetsText;
    [SerializeField] private Button PlayerOptLeft, PlayerOptRight;
    private int CurPlayerOptionPos = default;

    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;

    [SerializeField] private Button PlayButton;

    [SerializeField] private GameObject OptionsCanvas;

    [SerializeField] private GameObject[] OptionsList;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }

        StartPos = Camera.main.WorldToScreenPoint(new Vector3(0, 4f, 0));
        EndPos = Camera.main.WorldToScreenPoint(new Vector3(0, 4.25f, 0));
        SetPlayerText();
        PlayerOptLeft.onClick.AddListener(PlayerLeftClick);
        PlayerOptRight.onClick.AddListener(PlayerRightClick);
        OptionsList[CurPlayerOptionPos].SetActive(true);
        StartCoroutine(BounceTextUp());
        PlayButton.onClick.AddListener(PlayGame);
    }

    private IEnumerator BounceTextUp()
    {
        float Timer = 0;
        float MaxTimer = 0.75f;
        while (Timer < MaxTimer)
        {
            OnePlayerText.transform.position = Vector3.Lerp(StartPos, EndPos, Timer / MaxTimer);
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
            OnePlayerText.transform.position = Vector3.Lerp(EndPos, StartPos, Timer / MaxTimer);
            Timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(BounceTextUp());
    }

    private void PlayerLeftClick()
    {
        OptionsList[CurPlayerOptionPos].SetActive(false);
        if (CurPlayerOptionPos == 0)
        {
            CurPlayerOptionPos = PlayerOptions.Length - 1;
        }
        else
        {
            CurPlayerOptionPos -= 1;
        }

        OptionsList[CurPlayerOptionPos].SetActive(true);

        SetPlayerText();
    }

    private void PlayerRightClick()
    {
        OptionsList[CurPlayerOptionPos].SetActive(false);
        if (CurPlayerOptionPos == PlayerOptions.Length - 1)
        {
            CurPlayerOptionPos = 0;
        }
        else
        {
            CurPlayerOptionPos++;
        }
        OptionsList[CurPlayerOptionPos].SetActive(true);
        SetPlayerText();
    }

    private void SetPlayerText()
    {
        PlayerPresetsText.text = PlayerOptions[CurPlayerOptionPos];
    }

    private void PlayGame()
    {
        OptionsCanvas.SetActive(false);
        ManagerInstance.GameStart();
        //ManagerInstance.GameReset = true;
    }
}
