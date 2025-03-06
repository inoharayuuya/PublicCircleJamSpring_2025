using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Run_kyori : MonoBehaviour
{
    public GameObject scortext = null;
    public float scor = 0;
    public RunHaikei run_haikei;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Scor();
    }
    void Scor()
    {
        //Run_Haikei run_Haikei = GetComponent<Run_Haikei>();
        Text scortext = GetComponent<Text>();
        scor = run_haikei.distance * -1;
        scortext.text = scor.ToString("0000") + " M";
    }
}