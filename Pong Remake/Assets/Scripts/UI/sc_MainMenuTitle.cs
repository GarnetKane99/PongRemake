using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class sc_MainMenuTitle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private Button SinglePlayer, TwoPlayer, Quit;

    private Vector3 StartPos;

    private void Awake()
    {
        StartPos = Camera.main.WorldToScreenPoint(new Vector3(0, -6, 0));
        SinglePlayer.gameObject.SetActive(false);
        TwoPlayer.gameObject.SetActive(false);
        Quit.gameObject.SetActive(false);

        //SinglePlayer.onClick.AddListener
    }

    private void Start()
    {
        StartCoroutine(SetupTitle());
    }

    private IEnumerator SetupTitle()
    {
        float Timer = 0;
        float MaxTimer = 3.0f;
        while (Timer < MaxTimer)
        {
            TitleText.transform.position = Vector3.Lerp(StartPos, Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)), Timer / MaxTimer);
            Timer += Time.deltaTime;
            yield return null;
        }
        Invoke("SinglePlayerSetup", 1.0f);
    }

    private void SinglePlayerSetup()
    {
        SinglePlayer.gameObject.SetActive(true);
        Invoke("TwoPlayerSetup", 1.0f);
    }
    private void TwoPlayerSetup()
    {
        TwoPlayer.gameObject.SetActive(true);
        Invoke("QuitSetup", 1.0f);
    }

    private void QuitSetup()
    {
        Quit.gameObject.SetActive(true);
    }

    private void SinglePlayerGame()
    {

    }

    private void TwoPlayerGame()
    {

    }
}
