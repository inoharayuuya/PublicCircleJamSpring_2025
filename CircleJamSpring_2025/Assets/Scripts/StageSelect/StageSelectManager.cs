using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// todo stageReleaseにステージクラスから参照して取得する
// todo 矢印のアニメーションがずれるのを修正する

public class StageSelectManager : MonoBehaviour
{
    //#region  変数宣言

    //[SerializeField, Tooltip("ステージ名が格納されているオブジェクトを格納")]
    //private GameObject stage;
    //[SerializeField, Tooltip("ステージ名のプレハブを格納")]
    //private GameObject stageNamePrefab;
    //[SerializeField, Tooltip("表示するステージ名を格納")]
    //private string[] stageName;
    //[SerializeField, Tooltip("ステージ上のキャラを格納")]
    //private GameObject collegeStudent;
    //[SerializeField, Tooltip("ステージ(赤丸)を格納")]
    //private GameObject[] stageObject;
    //[SerializeField, Tooltip("UIの中心座標を格納")]
    //private GameObject CenterUI;
    //[SerializeField, Tooltip("警告テキストを格納")]
    //private GameObject[] errorTexts;
    //[SerializeField, Tooltip("ステージの横に表示する矢印を格納")]
    //private GameObject[] arrowImages;
    //[SerializeField, Tooltip("ステージの間に置く丸を格納")]
    //private GameObject circle;
    //[SerializeField, Tooltip("戦闘開始テキストの親オブジェクトを格納")]
    //private GameObject battleTextObject;

    //public int stageRelease;                // 現在解放されているステージ数

    //private GameObject[] stageNameObject;   // ステージ名オブジェクトを格納
    //private Camera mainCameraObject;        // カメラのオブジェクト
    //private Vector3 startPos;               // タップされた座標
    //private Vector3 endPos;                 // 押している間の座標
    //private bool isTap;                     // タップしているかどうか
    //private bool isMove;                    // 動けるかどうか
    //private float speed;                    // ステージ名オブジェクトを動かすスピード
    //private float startPosTime;             // startPosTimeのコピーを保存
    //private float direction;                // タップされた座標から押された座標を引いた値を格納
    //private float[] stageNamePos;           // ステージの座標を格納
    //private float[] stageNameSpace;         // ステージ名オブジェクトどうしの間
    //private int count = 0;
    //private int stageIndex = 0;
    //private int stageSpaceIndex = 0;
    //private int clearCnt;                   // クリアしているステージ数を格納

    //private Color halfAlpha = new Color(1.0f, 1.0f, 1.0f, 0.5f);    // α値が半分の変数
    //private Color maxAlpha = new Color(1.0f, 1.0f, 1.0f, 1.0f);     // α値が最大の変数(透明じゃなくなる)

    //private Animator animator;
    //private AudioSource audioSource;
    //private int battleTextIndex;
    //private UnityEngine.Object[] battleObjects = new UnityEngine.Object[5];

    //#endregion

    //// Start is called before the first frame update
    //void Start()
    //{
    //    // 初期化
    //    Init();
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    ///// <summary>
    ///// 初期化
    ///// </summary>
    //void Init()
    //{
        
    //}

    ///// <summary>
    ///// circleを生成
    ///// </summary>
    //void CreateCircle()
    //{
    //    // ステージオブジェクトの間に間隔を持ってcircleを生成
    //    for (int i = 0; i < clearCnt; i++)
    //    {
    //        var pos = stageObject[i].transform.position;
    //        var nextPos = stageObject[i + 1].transform.position;
    //        var distance = pos - nextPos;

    //        var spaceX = Mathf.Abs(distance.x / Common.SS_CIRCLE_SPACE);
    //        var spaceY = Mathf.Abs(distance.y / Common.SS_CIRCLE_SPACE);

    //        if (spaceX > spaceY)
    //        {
    //            spaceY = Mathf.Abs(distance.y / spaceX);
    //            spaceX = Common.SS_CIRCLE_SPACE;

    //            if (distance.y == 0)
    //            {
    //                spaceY = Common.SS_CIRCLE_SPACE;
    //            }
    //        }
    //        else if (spaceX < spaceY)
    //        {
    //            spaceX = Mathf.Abs(distance.x / spaceY);
    //            spaceY = Common.SS_CIRCLE_SPACE;

    //            if (distance.x == 0)
    //            {
    //                spaceX = Common.SS_CIRCLE_SPACE;
    //            }
    //        }
    //        else
    //        {
    //            spaceX = Common.SS_CIRCLE_SPACE;
    //            spaceY = Common.SS_CIRCLE_SPACE;
    //        }

    //        int cnt = 0;
    //        var minDistance = 0.5f;
    //        while (Mathf.Abs(distance.x) > minDistance || Mathf.Abs(distance.y) > minDistance)
    //        {
    //            if (cnt > 100)
    //            {
    //                break;
    //            }

    //            if (distance.x > 0)
    //            {
    //                pos.x += -spaceX;
    //                distance.x += -spaceX;
    //            }
    //            else if (distance.x < 0)
    //            {
    //                pos.x += spaceX;
    //                distance.x += spaceX;
    //            }
    //            if (distance.y > 0)
    //            {
    //                pos.y += -spaceY;
    //                distance.y += -spaceY;
    //            }
    //            else if (distance.y < 0)
    //            {
    //                pos.y += spaceY;
    //                distance.y += spaceY;
    //            }

    //            Instantiate(circle, pos, Quaternion.identity);
    //            cnt++;
    //        }
    //    }
    //}
}
