using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_ColourSelection : MonoBehaviour
{
    [SerializeField] private bool SinglePlayer;
    [SerializeField] private bool PlayerLeft;
    [SerializeField] private List<Button> ColourButtons;
    [SerializeField] private sc_GameManager ManagerInstance = sc_GameManager.instance;

    void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<sc_GameManager>();
        }

        foreach (Button Colours in ColourButtons)
        {
            Colours.onClick.AddListener(() =>
            {
                UpdatePaddleColour(Colours.GetComponent<Image>());
            });
        }
    }

    public void UpdatePaddleColour(Image IMG)
    {
        if (IMG != null)
        {
            if (!SinglePlayer)
            {
                if (PlayerLeft)
                {
                    ManagerInstance.PlayerLeft.GetComponent<SpriteRenderer>().color = IMG.color;
                }
                else
                {
                    ManagerInstance.PlayerRight.GetComponent<SpriteRenderer>().color = IMG.color;
                }
            }
            else
            {
                ManagerInstance.SinglePlayerController.GetComponent<SpriteRenderer>().color = IMG.color;
            }
        }
        else
        {
            Debug.Log("No Image");
        }
    }
}
