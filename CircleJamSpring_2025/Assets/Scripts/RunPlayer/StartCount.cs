using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StartCount : MonoBehaviour
{
    public RunPlayer runPlayer;
    public GameObject count = null;   // Textオブジェクト
    public float startCountdown = 3f; //スタートカウントダウン用
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeCount();
    }
    public void TimeCount()
    {
        Text start_text = count.GetComponent<Text>();       // オブジェクトからTextコンポーネントを取得
        int displayCount = Mathf.CeilToInt(startCountdown); // 小数点以下を切り上げて整数にする
        start_text.text = displayCount.ToString();          // テキストの表示

        if (startCountdown >= 0)
        {
            startCountdown -= Time.deltaTime;               // 321のカウントダウン
        }
                           
        if (startCountdown <= 0)
        {
            start_text.text = "Start!";
        }
    } 
}
