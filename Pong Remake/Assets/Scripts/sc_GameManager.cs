using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_GameManager : MonoBehaviour
{
    public static sc_GameManager instance { get; private set; }

    private Camera mainCamera;
    public float WorldHeight => mainCamera.orthographicSize;
    public float WorldWidth => WorldHeight * ((float)Screen.width / (float)Screen.height);

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
