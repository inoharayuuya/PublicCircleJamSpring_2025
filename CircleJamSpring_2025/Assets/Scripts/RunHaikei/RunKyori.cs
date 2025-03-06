using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Run_kyori : MonoBehaviour
{
    public GameObject scortext = null;
    public float score = 0;
    public RunHaikei run_haikei;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Score();
    }
    void Score()
    {
        //Run_Haikei run_Haikei = GetComponent<Run_Haikei>();
        Text scoretext = GetComponent<Text>();
        score = run_haikei.distance;
        scoretext.text = score.ToString("0000") + " M";
    }
}