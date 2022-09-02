using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sc_PaddleSpeedController : MonoBehaviour
{
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
            ManagerInstance.SinglePlayerController.speed = (Mathf.Round(x * 10.0f) * 0.1f);
            SliderText.text = (Mathf.Round(x * 10.0f) * 0.1f).ToString();
        });
    }
}
