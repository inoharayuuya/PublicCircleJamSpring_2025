using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType
    {
        [Tooltip("固定ダメージ")] Fix,        // 固定ダメージ
        [Tooltip("持続ダメージ")] Duration,   // 持続ダメージ
        [Tooltip("回復")]         Heal,       // 回復
    }

    [Tooltip("消失するか否か")] public bool isObstacleDisabled;    // 消えるかどうか

    [Tooltip("効果量")]         public float amount;               // 効果量

    [Tooltip("障害物のタイプ")] public ObstacleType obstacleType;  // 障害物のタイプ

    public bool isFirst;


    private void Start()
    {
        Init();
    }

    void Init()
    {
        isObstacleDisabled = false;
    }



    /// <summary>
    /// 効果発動
    /// </summary>
    public void Amount()
    {
        switch (obstacleType)
        {
            case ObstacleType.Fix:
                if (isFirst)
                {
                    isFirst = false;
                    // ダメージ処理
                    print("固定ダメージ");
                }

                break;
            case ObstacleType.Duration:
                if (isFirst)
                {
                    // 一秒タイマー
                    StartCoroutine(CountDown());
                }


                break;
            case ObstacleType.Heal:
                if (isFirst)
                {
                    isFirst = false;
                    // 回復処理
                    print("回復");
                }


                break;



        }


        print($"isFirst:{isFirst}");
    }

    
    IEnumerator CountDown()
    {
        isFirst = false;

        // ダメージ処理
        print("継続ダメージ");

        yield return new WaitForSeconds(1);
        isFirst = true;
        print("1秒経過");

    }


}
