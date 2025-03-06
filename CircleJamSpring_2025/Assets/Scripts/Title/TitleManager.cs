using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ƒNƒŠƒbƒNˆ—
    /// </summary>
    public void PushTitle()
    {
        Common.LoadScene("StageSelect");
    }
}
