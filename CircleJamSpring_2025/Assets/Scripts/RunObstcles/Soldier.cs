using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public ObstaclesControl obsControl;
    public bool soldierMoved = false;
    public int remainder = 2;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("衝突検知: " + gameObject.name); // 確認用ログ

        if (collider.gameObject.CompareTag("Player") && !soldierMoved)
        {
            soldierMoved = true;
            obsControl.moveCount += 2;
            Debug.Log(obsControl.moveCount);
            remainder = obsControl.moveCount % 2;
            //Debug.Log("MoveSun()実行");
            if (remainder == 0)
            {
                obsControl.moveDistance = 2;
            }
            obsControl.MoveSun();
        }
        /*suncontrol.hit = true;
        suncontrol.SunManager();*/
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            soldierMoved = false; //離れたらフラグをリセット
            obsControl.moveDistance = 1;
        }
    }
}
