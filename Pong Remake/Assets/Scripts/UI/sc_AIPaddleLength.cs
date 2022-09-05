using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sc_AIPaddleLength : MonoBehaviour
{
    [SerializeField] private Slider AILengthSlider;
    [SerializeField] private TextMeshProUGUI SliderText;
    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }

        AILengthSlider.onValueChanged.AddListener((x) =>
        {
            ManagerInstance.AIController.transform.localScale = new Vector3(ManagerInstance.AIController.transform.localScale.x, (Mathf.Round(x * 10.0f) * 0.1f), ManagerInstance.AIController.transform.localScale.z);
            SliderText.text = (Mathf.Round(x * 10.0f) * 0.1f).ToString();
        });
    }
}
