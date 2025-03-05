using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class StageSelectManager : MonoBehaviour
{
    #region  変数宣言
    [Tooltip("シーン上のステージ名をセットする親オブジェクト")]
    [SerializeField] GameObject stageParent;

    [Tooltip("ステージ名のプレハブをセット")]
    [SerializeField] GameObject stageNamePrefab;
    
    [Tooltip("ステージ上に配置するヴァンパイアをセット")]
    [SerializeField] GameObject vampire;

    [Tooltip("ステージ(赤丸)のプレハブをセット")]
    [SerializeField] GameObject stagePrefab;

    [Tooltip("UIの中心座標をセット")]
    [SerializeField] GameObject CenterUI;
    
    [Tooltip("ステージの横に表示する矢印をセット")]
    [SerializeField] GameObject[] arrowImages;
    
    [Tooltip("ステージの間に置く丸のプレハブをセット")]
    [SerializeField] GameObject circle;

    [Tooltip("ステージ(赤丸)の実体をセット")]
    GameObject[] stageObject;

    [Tooltip("ステージ名オブジェクトをセット")]
    GameObject[] stageNameObject;

    [Tooltip("カメラのオブジェクトをセット")]
    Camera mainCameraObject;

    [Tooltip("タップされた座標をセット")]
    Vector3 startPos;

    [Tooltip("押している間の座標をセット")]
    Vector3 endPos;

    [Tooltip("タップしているかどうかをセット")]
    bool isTap;

    [Tooltip("動けるかどうかをセット")]
    bool isMove;

    [Tooltip("ステージ名オブジェクトを動かすスピードをセット")]
    float speed;

    [Tooltip("startPosTimeのコピーを保存をセット")]
    float startPosTime;

    [Tooltip("タップされた座標から押された座標を引いた値をセット")]
    float direction;

    [Tooltip("ステージの座標をセット")]
    float[] stageNamePos;

    [Tooltip("ステージ名オブジェクトどうしの間をセット")]
    float[] stageNameSpace;

    [Tooltip("カウント用")]
    int count = 0;

    [Tooltip("現在のステージの中心のインデックスをセット")]
    int stageIndex = 0;

    [Tooltip("現在のステージの中心のインデックスをセット")]
    int stageSpaceIndex = 0;

    [Tooltip("ヴァンパイアのアニメーション")]
    Animator animator;
    #endregion

    #region Unityデフォルトイベント
    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // 画面が押されているかを判定する
        TapCheck();

        // ステージの移動処理
        StageMove();

        // ステージ名の中心座標を取得
        GetCenter();
    }
    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    void Init()
    {
        animator     = vampire.GetComponent<Animator>();    // ヴァンパイアのアニメーターを変数にセット
        startPos     = Vector3.zero;                        // タップされた始めの位置
        endPos       = Vector3.zero;                        // タップが終わったときの位置
        speed        = Common.SS_INIT_SPEED;                // 設定した初期スピードで初期化
        startPosTime = Common.SS_START_POS_TIME;            // startPosの値を更新する更新頻度
        direction    = 0;                                   // タップされた点の距離

        // ステージとキャラ、カメラの座標に使う変数
        var pos = stageObject[0].transform.position;  // ここのstageObjectのインデックスを変えればステージの初期座標が変わる

        // ステージの初期値をセット
        var stagePos = stageParent.transform.position;
        stagePos.x = -pos.x;
        stageParent.transform.position = stagePos;

        stageNameObject = new GameObject[Common.SS_MAX_STAGE];
        stageNamePos = new float[Common.SS_MAX_STAGE];
        stageNameSpace = new float[Common.SS_MAX_STAGE - 1];

        isTap = false;
        isMove = false;

        // 全ステージを生成し、ヒエラルキーの名前も変更
        for (int i = 0; i < Common.SS_MAX_STAGE; i++)
        {
            GameObject instance = Instantiate(stageNamePrefab, stageParent.transform);
            stageNameObject[i] = instance;
        }

        // 表示するステージ数分表示
        //for (int i = 0; i < clearCnt + 1; i++)
        //{
        //    stageNameObject[i].GetComponentInChildren<Text>().text = stageName[i];
        //}

        // 最初に選択されているステージのα値を変える
        stageNameObject[0].GetComponent<Image>().color = Common.MAX_ALPHA;

        // 解放されてないステージ名オブジェクトを非表示にする
        //for (int i = clearCnt + 1; i < Common.SS_MAX_STAGE; i++)
        //{
        //    stageNameObject[i].SetActive(false);
        //    stageObject[i].SetActive(false);
        //}

        // キャラの初期座標をセット
        var collegeStudentPos = vampire.transform.position;
        collegeStudentPos.x = pos.x;
        vampire.transform.position = collegeStudentPos;

        // カメラオブジェクト取得
        mainCameraObject = Camera.main;

        // カメラの初期座標をセット
        var cameraPos = mainCameraObject.transform.position;
        cameraPos.x = pos.x;
        mainCameraObject.transform.position = cameraPos;

        // ステージの間の丸を生成
        CreateCircle();
        //Invoke("CreateCircle", 0.5f);
    }

    /// <summary>
    /// circleを生成
    /// </summary>
    void CreateCircle()
    {
        // ステージオブジェクトの間に間隔を持ってcircleを生成
        //for (int i = 0; i < clearCnt; i++)
        //{
        //    var pos = stageObject[i].transform.position;
        //    var nextPos = stageObject[i + 1].transform.position;
        //    var distance = pos - nextPos;

        //    var spaceX = Mathf.Abs(distance.x / Common.SS_CIRCLE_SPACE);
        //    var spaceY = Mathf.Abs(distance.y / Common.SS_CIRCLE_SPACE);

        //    if (spaceX > spaceY)
        //    {
        //        spaceY = Mathf.Abs(distance.y / spaceX);
        //        spaceX = Common.SS_CIRCLE_SPACE;

        //        if (distance.y == 0)
        //        {
        //            spaceY = Common.SS_CIRCLE_SPACE;
        //        }
        //    }
        //    else if (spaceX < spaceY)
        //    {
        //        spaceX = Mathf.Abs(distance.x / spaceY);
        //        spaceY = Common.SS_CIRCLE_SPACE;

        //        if (distance.x == 0)
        //        {
        //            spaceX = Common.SS_CIRCLE_SPACE;
        //        }
        //    }
        //    else
        //    {
        //        spaceX = Common.SS_CIRCLE_SPACE;
        //        spaceY = Common.SS_CIRCLE_SPACE;
        //    }

        //    int cnt = 0;
        //    var minDistance = Common.SS_CIRCLE_SPACE;
        //    while (Mathf.Abs(distance.x) > minDistance || Mathf.Abs(distance.y) > minDistance)
        //    {
        //        if (cnt > 100)
        //        {
        //            break;
        //        }

        //        if (distance.x > 0)
        //        {
        //            pos.x += -spaceX;
        //            distance.x += -spaceX;
        //        }
        //        else if (distance.x < 0)
        //        {
        //            pos.x += spaceX;
        //            distance.x += spaceX;
        //        }
        //        if (distance.y > 0)
        //        {
        //            pos.y += -spaceY;
        //            distance.y += -spaceY;
        //        }
        //        else if (distance.y < 0)
        //        {
        //            pos.y += spaceY;
        //            distance.y += spaceY;
        //        }

        //        Instantiate(circle, pos, Quaternion.identity);
        //        cnt++;
        //    }
        //}
    }

    /// <summary>
    /// タップチェック処理
    /// </summary>
    void TapCheck()
    {
        // 画面がタップされたとき
        if (Input.GetMouseButtonDown(0))
        {
            // startPosに画面が押された座標を代入する
            startPos = Input.mousePosition;

            // 画面が押されているのでtrueにする
            isTap = true;

            // 動けるようになるのでtrue
            isMove = true;
        }

        // 画面がタップされている間
        if (Input.GetMouseButton(0))
        {
            // endPosには毎フレーム画面が押されている座標を代入する
            endPos = Input.mousePosition;

            // 更新時間がくるまでdeltaTimeで計算
            startPosTime -= Time.deltaTime;

            // スピードは押されるたびに初期化する
            speed = Common.SS_INIT_SPEED;

            // startPosTimeが0になるたびにstartPosを現在のendPosで初期化
            if (startPosTime <= 0)
            {
                // startPosTimeの初期化
                startPosTime = Common.SS_START_POS_TIME;
                startPos = endPos;
            }
        }

        // 画面から指が離されたとき
        if (Input.GetMouseButtonUp(0))
        {
            // 指が離されたのでfalse
            isTap = false;
        }

        // 画面が押されていない間は減速し続ける
        if (!isTap)
        {
            speed *= Common.SS_SLOW_DOWN_SPEED;
            //direction *= Common.SS_SLOW_DOWN_SPEED;

            if (speed < 0.05f || Mathf.Abs(direction) < 50)
            {
                // 通常の移動を消したいのでfalse
                isMove = false;

                // 追尾する中心点を確定させる
                stageIndex = CheckCenter();

                // ステージ名の中心に寄らせる
                if (stageParent.GetComponent<RectTransform>().localPosition.x < -(stageNamePos[stageIndex] + 20))
                {
                    stageParent.GetComponent<RectTransform>().localPosition += new Vector3(Common.SS_DIRECTION_SPEED, 0, 0) * Time.deltaTime;
                }
                else if (stageParent.GetComponent<RectTransform>().localPosition.x > -(stageNamePos[stageIndex] - 20))
                {
                    stageParent.GetComponent<RectTransform>().localPosition += new Vector3(-Common.SS_DIRECTION_SPEED, 0, 0) * Time.deltaTime;
                }
                else
                {
                    if (speed < Common.SS_MIN_SPEED)
                    {
                        // 中心の座標を代入
                        stageParent.GetComponent<RectTransform>().localPosition = new Vector3(-stageNamePos[stageIndex],
                                                                                        stageParent.GetComponent<RectTransform>().localPosition.y,
                                                                                        stageParent.GetComponent<RectTransform>().localPosition.z);
                    }

                    // 止める
                    speed = 0;
                }
            }
        }
    }

    /// <summary>
    /// ステージ名の移動処理
    /// </summary>
    void StageMove()
    {
        // startPosとendPosを計算して距離を求める
        direction = (endPos.x - startPos.x) * speed;

        if (isMove)
        {
            // 計算結果がプラスの場合(0を含まない)
            if (direction > 0)
            {
                // ステージ名オブジェクトが0未満(値がマイナス)の時だけ移動させる
                if (stageParent.transform.position.x < CenterUI.transform.position.x)
                {
                    stageParent.transform.position += new Vector3((direction * speed), 0, 0) * Time.deltaTime;
                }
            }
            else
            {
                // ステージ名オブジェクトが0以上の時だけ移動する
                //if (stageNameObject[stageRelease - 1].transform.position.x >= CenterUI.transform.position.x)
                //{
                //    stageParent.transform.position += new Vector3((direction * speed), 0, 0) * Time.deltaTime;
                //}
            }
        }

        // 一番近い中心座標のインデックスを取得
        stageSpaceIndex = CheckCenter();

        if (speed < 0.075f)
        {
            isMove = false;
            speed = 0;
        }

        // 中心座標に一番近いオブジェクトの透明度をなしにする
        stageNameObject[stageSpaceIndex].GetComponent<Image>().color = Common.MAX_ALPHA;
        stageNameObject[stageSpaceIndex].GetComponentInChildren<Text>().color = new Color(1f, 0.5f, 0, 1f);

        // 中心座標に一番近いオブジェクトのサイズを変更する
        var size = 1.25f;
        stageNameObject[stageSpaceIndex].GetComponent<RectTransform>().localScale = new Vector3(size, size, size);

        // キャラがステージを移動する
        CharaMove();

        // カメラがステージを移動する
        CameraMove();

        // １ステージのみ表示されていた場合
        //if (stageRelease == 1)
        //{
        //    print("矢印off");
        //    var tmp1 = arrowImages[0].GetComponent<Image>().color;
        //    tmp1.a = 0f;
        //    arrowImages[0].GetComponent<Image>().color = tmp1;

        //    var tmp2 = arrowImages[0].GetComponent<Image>().color;
        //    tmp2.a = 0f;
        //    arrowImages[1].GetComponent<Image>().color = tmp2;

        //    return;
        //}

        if (stageSpaceIndex == 0)
        {
            var tmp1 = arrowImages[0].GetComponent<Image>().color;
            tmp1.a = 0;
            arrowImages[0].GetComponent<Image>().color = tmp1;
            var tmp2 = arrowImages[1].GetComponent<Image>().color;
            tmp2.a = 0.5f;
            arrowImages[1].GetComponent<Image>().color = tmp2;//errorTexts[0].SetActive(false);
        }
        //else if (stageSpaceIndex == stageRelease - 1)
        //{
        //    var tmp1 = arrowImages[0].GetComponent<Image>().color;
        //    tmp1.a = 0.5f;
        //    arrowImages[0].GetComponent<Image>().color = tmp1;
        //    var tmp2 = arrowImages[1].GetComponent<Image>().color;
        //    tmp2.a = 0;
        //    arrowImages[1].GetComponent<Image>().color = tmp2;
        //}
        else
        {
            var tmp1 = arrowImages[0].GetComponent<Image>().color;
            tmp1.a = 0.5f;
            arrowImages[0].GetComponent<Image>().color = tmp1;

            var tmp2 = arrowImages[1].GetComponent<Image>().color;
            tmp2.a = 0.5f;
            arrowImages[1].GetComponent<Image>().color = tmp2;
        }
    }

    /// <summary>
    /// ステージを移動するキャラの制御
    /// </summary>
    void CharaMove()
    {
        Vector2 direction = stageObject[stageSpaceIndex].transform.position - vampire.transform.position;

        if (direction.x > 0)
        {
            if (vampire.transform.position.x < stageObject[stageSpaceIndex].transform.position.x)
            {
                vampire.transform.position += new Vector3(direction.x, 0, 0) * Common.SS_CHARA_SPEED * Time.deltaTime;
                var lScale = vampire.transform.localScale;
                if (lScale.x <= 0)
                {
                    lScale.x *= -1;
                }
                vampire.transform.localScale = lScale;
            }
        }
        else
        {
            if (vampire.transform.position.x > stageObject[stageSpaceIndex].transform.position.x)
            {
                vampire.transform.position += new Vector3(direction.x, 0, 0) * Common.SS_CHARA_SPEED * Time.deltaTime;

                var lScale = vampire.transform.localScale;
                if (lScale.x >= 0)
                {
                    lScale.x *= -1;
                }
                vampire.transform.localScale = lScale;
            }
        }

        if (direction.y > 0)
        {
            if (vampire.transform.position.y < stageObject[stageSpaceIndex].transform.position.y)
            {
                vampire.transform.position += new Vector3(0, direction.y, 0) * Common.SS_CHARA_SPEED * Time.deltaTime;
            }
        }
        else
        {
            if (vampire.transform.position.y > stageObject[stageSpaceIndex].transform.position.y)
            {
                vampire.transform.position += new Vector3(0, direction.y, 0) * Common.SS_CHARA_SPEED * Time.deltaTime;
            }
        }

        float limit = 0.15f;

        if (MathF.Abs(direction.x) > limit || MathF.Abs(direction.y) > limit)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }
    }

    /// <summary>
    /// カメラ移動の制御
    /// </summary>
    void CameraMove()
    {
        Vector2 direction = stageObject[stageSpaceIndex].transform.position - mainCameraObject.transform.position;

        if (direction.x > 0)
        {
            if (mainCameraObject.transform.position.x < stageObject[stageSpaceIndex].transform.position.x)
            {
                mainCameraObject.transform.position += new Vector3(direction.x, 0, 0) * Common.SS_CAMERA_SPEED * Time.deltaTime;
            }
        }
        else
        {
            if (mainCameraObject.transform.position.x > stageObject[stageSpaceIndex].transform.position.x)
            {
                mainCameraObject.transform.position += new Vector3(direction.x, 0, 0) * Common.SS_CAMERA_SPEED * Time.deltaTime;
            }
        }

        if (direction.y > 0)
        {
            if (mainCameraObject.transform.position.y < stageObject[stageSpaceIndex].transform.position.y)
            {
                mainCameraObject.transform.position += new Vector3(0, direction.y, 0) * Common.SS_CAMERA_SPEED * Time.deltaTime;
            }
        }
        else
        {
            if (mainCameraObject.transform.position.y > stageObject[stageSpaceIndex].transform.position.y)
            {
                mainCameraObject.transform.position += new Vector3(0, direction.y, 0) * Common.SS_CAMERA_SPEED * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// ステージ名の中心座標を取得(1フレームおいて計算する)
    /// </summary>
    void GetCenter()
    {
        if (count == 1)
        {
            for (int i = 0; i < stageNamePos.Length; i++)
            {
                // 中心座標を取得
                stageNamePos[i] = stageNameObject[i].GetComponent<RectTransform>().localPosition.x;

                if (i != 0 && i < stageNamePos.Length - 1)
                {
                    // ステージ名の間の座標を取得
                    stageNameSpace[i - 1] = (stageNamePos[i]) - 250;
                }
            }

            CheckCenter();
        }

        count++;
    }

    /// <summary>
    /// 毎フレーム画面中心に一番近いステージの中心座標を計算する
    /// </summary>
    /// <returns>中心に一番近かった中心座標のインデックスを返す</returns>
    int CheckCenter()
    {
        float sum = Common.SS_INIT_SUM;
        int index = 0;

        for (int i = 0; i < stageNamePos.Length; i++)
        {
            // ステージ名オブジェクトのα値を初期化する
            stageNameObject[i].GetComponent<Image>().color = Common.HALF_ALPHA;
            stageNameObject[i].GetComponentInChildren<Text>().color = new Color(1f, 0.5f, 0, 0.5f);

            // ステージ名オブジェクトのサイズを初期化
            var size = 1f;
            stageNameObject[stageSpaceIndex].GetComponent<RectTransform>().localScale = new Vector3(size, size, size);

            // ステージの座標と各ステージの中心点の座標の差を計算
            var stageDirection = Mathf.Abs(Mathf.Abs(stageParent.GetComponent<RectTransform>().localPosition.x) - stageNamePos[i]);

            // 各ステージ名オブジェクトの距離を判定、sumより小さければsumを書き換える
            if (Mathf.Abs(sum) > stageDirection)
            {
                sum = stageDirection;
                index = i;
            }
        }

        return index;
    }

    /// <summary>
    /// 出撃ボタンを押したときの処理
    /// </summary>
    public void TapReadyButton()
    {
        // BGMをストップする
        //SoundManager.smInstance.StopBGM();

        // SEを再生
        //AudioClip clip = LoadSE("se_battle_start");
        //audioSource.volume = 0.25f;
        //audioSource.PlayOneShot(clip);
    }
}
