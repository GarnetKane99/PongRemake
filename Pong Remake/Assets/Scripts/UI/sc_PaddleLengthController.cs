using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sc_PaddleLengthController : MonoBehaviour
{
    [SerializeField] private bool SinglePlayer;
    [SerializeField] private bool PlayerLeft;
    [SerializeField] private Slider PaddleLengthSlider;
    [SerializeField] private TextMeshProUGUI SliderText;
    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }

        PaddleLengthSlider.onValueChanged.AddListener((y) =>
        {
            if (!SinglePlayer)
            {
                if (PlayerLeft)
                {
                    ManagerInstance.PlayerLeft.transform.localScale = new Vector3(ManagerInstance.PlayerLeft.transform.localScale.x, (Mathf.Round(y * 10.0f) * 0.1f), ManagerInstance.PlayerLeft.transform.localScale.z);
                }
                else
                {
                    ManagerInstance.PlayerRight.transform.localScale = new Vector3(ManagerInstance.PlayerRight.transform.localScale.x, (Mathf.Round(y * 10.0f) * 0.1f), ManagerInstance.PlayerRight.transform.localScale.z);
                }
            }
            else
            {
                ManagerInstance.SinglePlayerController.transform.localScale = new Vector3(ManagerInstance.SinglePlayerController.transform.localScale.x, (Mathf.Round(y * 10.0f) * 0.1f), ManagerInstance.SinglePlayerController.transform.localScale.z);
            }
            SliderText.text = (Mathf.Round(y * 10.0f) * 0.1f).ToString();
        });
    }
}
