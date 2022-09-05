using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sc_AIDifficulty : MonoBehaviour
{
    [SerializeField] private Slider DifficultySlider;
    [SerializeField] private TextMeshProUGUI SliderText;
    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }

        DifficultySlider.onValueChanged.AddListener((x) =>
        {
            switch (x)
            {
                case 1:
                    ManagerInstance.AIController.UpdateBrain(EnemyDifficulty.EASY);
                    SliderText.text = "EASY";
                    break;
                case 2:
                    ManagerInstance.AIController.UpdateBrain(EnemyDifficulty.MEDIUM);
                    SliderText.text = "MEDIUM";
                    break;
                case 3:
                    ManagerInstance.AIController.UpdateBrain(EnemyDifficulty.HARD);
                    SliderText.text = "HARD";
                    break;
                default:
                    break;
            }
        });
    }
}
