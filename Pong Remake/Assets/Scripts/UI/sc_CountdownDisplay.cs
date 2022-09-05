using System.Collections;
using UnityEngine;
using TMPro;

public class sc_CountdownDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CountdownText;

    private Vector2 StartPos;
    private Vector2 EndPos;

    private void Awake()
    {
        StartPos = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        EndPos = Camera.main.WorldToScreenPoint(new Vector3(0, 1, 0));
    }

    public IEnumerator DisplayCountdown(int Countdown)
    {
        CountdownText.color = Color.white;
        CountdownText.text = Countdown.ToString();
        InvokeRepeating("StartColourFlash", 0, 0.2f);

        float Timer = 0;
        float MaxTimer = 1.0f;

        while (Timer < MaxTimer)
        {
            CountdownText.transform.position = Vector3.Lerp(StartPos, EndPos, Timer / MaxTimer);
            Timer += Time.deltaTime;
            yield return null;
        }
        Countdown--;
        CancelInvoke("StartColourFlash");
        CountdownText.text = "";
        if (Countdown > 0)
        {
            CountdownText.transform.position = StartPos;
            StartCoroutine(DisplayCountdown(Countdown));
        }
    }

    private void StartColourFlash()
    {
        if (CountdownText.color == Color.white)
        {
            CountdownText.color = new Color(0, 0, 0, 0);
        }
        else
        {
            CountdownText.color = new Color(1, 1, 1, 1);
        }
    }
}
