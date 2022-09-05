using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sc_PaddleSpeedController : MonoBehaviour
{
    [SerializeField] private bool SinglePlayer;
    [SerializeField] private bool PlayerLeft;
    [SerializeField] private Slider PaddleSpeedSlider;
    [SerializeField] private TextMeshProUGUI SliderText;
    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }

        PaddleSpeedSlider.onValueChanged.AddListener((x) =>
        {
            if (!SinglePlayer)
            {
                if (PlayerLeft)
                {
                    ManagerInstance.PlayerLeft.speed = (Mathf.Round(x * 10.0f) * 0.1f);
                }
                else
                {
                    ManagerInstance.PlayerRight.speed = (Mathf.Round(x * 10.0f) * 0.1f);
                }
            }
            else
            {
                ManagerInstance.SinglePlayerController.speed = (Mathf.Round(x * 10.0f) * 0.1f);
            }

            SliderText.text = (Mathf.Round(x * 10.0f) * 0.1f).ToString();
        });
    }
}
