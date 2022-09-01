using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sc_TextShake : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerScoredText;
    private Vector3 StartingPos;

    private void Awake()
    {
        StartingPos = PlayerScoredText.transform.position;
    }

    public IEnumerator TextShaker(int playerID)
    {        
        PlayerScoredText.text = "Player " + (playerID + 1) + " Scored!";
        InvokeRepeating("StartColourFlash", 0, 0.2f);
        float Timer = 0;
        float MaxTimer = 1.6f;

        while (Timer < MaxTimer)
        {
            PlayerScoredText.transform.position = Vector3.Lerp(PlayerScoredText.transform.position, new Vector3(PlayerScoredText.transform.position.x, PlayerScoredText.transform.position.y + 0.1f), Timer / MaxTimer);
            Timer += Time.deltaTime;
            yield return null;
        }
        PlayerScoredText.transform.position = StartingPos;
        CancelInvoke("StartColourFlash");
        PlayerScoredText.color = Color.white;
        PlayerScoredText.text = "";
    }

    private void StartColourFlash()
    {
        if (PlayerScoredText.color == Color.white)
        {
            PlayerScoredText.color = new Color(0, 0, 0, 0);
        }
        else
        {
            PlayerScoredText.color = new Color(1, 1, 1, 1);
        }
    }
}
