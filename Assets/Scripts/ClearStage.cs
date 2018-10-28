using UnityEngine;
using System.Collections;

public class ClearStage : MonoBehaviour {

    private float timer;

    public float openingTime = 4.0f;
    // Use this for initialization
    void Start()
    {
        openingTime = 4.0f;

    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if (timer > openingTime)
        {
            Application.LoadLevel("Opening");

        }

    }


    void Awake()
    {
        openingTime = 1.0f;
    }
}
