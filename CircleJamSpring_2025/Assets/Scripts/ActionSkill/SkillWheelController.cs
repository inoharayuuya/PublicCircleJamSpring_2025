using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillWheelController : MonoBehaviour
{
    public Image[] skillImages;  // スキルアイコンのImage
    public Text skillInfoText;    // 中央のスキル名を表示するText
    // public Text[] cooldownTexts; // 各スキルのクールタイムを表示するText
    public SkillData[] skillData; // スキルデータ（ScriptableObject）
    private int currentIndex = 2; // 中央のスキル位置
    private bool isRotating = false; // 回転中かどうかのフラグ
    private float rotationDuration = 0.2f; // 回転にかける時間

    public Vector3[] initPos;

    void Start()
    {
        initPos = new Vector3[skillImages.Length];
        for(int i=0;i<skillImages.Length;i++)
        {
            initPos[i]=skillImages[i].gameObject.GetComponent<RectTransform>().anchoredPosition;
            skillImages[i].sprite = skillData[i].skillImage;
        }
        StartSkillDisplay();
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && !isRotating)
        {
            RotateWheel(1); // 右スクロール
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && !isRotating)
        {
            RotateWheel(-1); // 左スクロール
        }

        // Eキーでスキル発動
        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateSkill();
        }

        // クールタイムの更新処理（ここでは仮に時間が経過する例として1秒減少）
        for (int i = 0; i < skillData.Length; i++)
        {
            if (skillData[i].coolTime > 0)
            {
                skillData[i].coolTime -= Time.deltaTime;
                // cooldownTexts[i].text = skillData[i].coolTime.ToString("F1"); // クールタイム表示
            }
            else
            {
                // cooldownTexts[i].text = " "; // クールタイムが終了したら空白
            }
        }
    }
    void RotateWheel(int direction)
    {
        if (isRotating) return; // 回転中なら処理しない

        isRotating = true; // 回転開始
        StartCoroutine(RotateWheelCoroutine(direction)); // コルーチン開始
    }

    IEnumerator RotateWheelCoroutine(int direction)
    {
        float elapsedTime = 0f;

        // 初期位置を取得
        Vector2[] startPos = new Vector2[skillImages.Length];
        Vector3[] startScales = new Vector3[skillImages.Length];
        float[] startAlphas = new float[skillImages.Length];

        for (int i = 0; i < skillImages.Length; i++)
        {
            startPos[i] = skillImages[i].GetComponent<RectTransform>().anchoredPosition;
            startScales[i] = skillImages[i].transform.localScale;
            startAlphas[i] = skillImages[i].color.a;
        }

        // 補間しながら位置を更新
        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rotationDuration;

            for (int i = 0; i < skillImages.Length; i++)
            {
                int targetIndex = (i + direction + skillImages.Length) % skillImages.Length;
                float targetAlpha;
                Vector3 targetScale;

                skillImages[i].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos[i], initPos[targetIndex], t);

                if (i + direction == currentIndex)
                {
                    targetAlpha = 1.0f; // 完全表示
                    targetScale = new Vector3(1.5f, 1.5f, 1);
                }
                else if (i + direction== (currentIndex - 1 + skillImages.Length) % skillImages.Length ||
                        i + direction== (currentIndex + 1) % skillImages.Length)
                {
                    targetAlpha = 0.5f; // 半透明
                    targetScale = Vector3.one;
                }
                else
                {
                    targetAlpha = 0.0f; // 非表示
                    targetScale = Vector3.one;
                }

                // 透明度を滑らかに変化
                Color color = skillImages[i].color;
                color.a = Mathf.Lerp(startAlphas[i], targetAlpha, t);
                skillImages[i].color = color;

                // スケールを滑らかに変化
                skillImages[i].transform.localScale = Vector3.Lerp(startScales[i], targetScale, t);
            }

            yield return null;
        }

        // データの入れ替え
        if (direction == 1)
        {
            // 右スクロール: 最後の要素を先頭に移動
            Image lastImage = skillImages[skillImages.Length - 1];
            SkillData last = skillData[skillData.Length - 1];

            for (int i = skillImages.Length - 1; i > 0; i--)
            {
                skillImages[i] = skillImages[i - 1];
                skillData[i] = skillData[i - 1];
            }

            skillImages[0] = lastImage;
            skillData[0] = last;
        }
        else if (direction == -1)
        {
            // 左スクロール: 先頭の要素を最後に移動
            Image firstImage = skillImages[0];
            SkillData first = skillData[0];

            for (int i = 0; i < skillImages.Length - 1; i++)
            {
                skillImages[i] = skillImages[i + 1];
                skillData[i] = skillData[i + 1];
            }

            skillImages[skillImages.Length - 1] = firstImage;
            skillData[skillData.Length - 1] = first;
        }

        isRotating = false; // 回転終了

        UpdateSkillDisplay();
    }


    void StartSkillDisplay()
    {
        initPos = new Vector3[skillImages.Length];

    for (int i = 0; i < skillImages.Length; i++)
    {
        // 各スキルアイコンの初期位置を設定
        initPos[i] = skillImages[i].GetComponent<RectTransform>().anchoredPosition;

        // 中央のスキルアイコンのみ大きく、その他は通常サイズ
        if (i == currentIndex)
        {
            skillImages[i].transform.localScale = new Vector3(1.5f, 1.5f, 1);
            skillImages[i].color = new Color(skillImages[i].color.r, skillImages[i].color.g, skillImages[i].color.b, 1.0f);
        }
        else if (i == (currentIndex - 1 + skillImages.Length) % skillImages.Length ||
                 i == (currentIndex + 1) % skillImages.Length)
        {
            skillImages[i].transform.localScale = Vector3.one;
            skillImages[i].color = new Color(skillImages[i].color.r, skillImages[i].color.g, skillImages[i].color.b, 0.5f);
        }
        else
        {
            skillImages[i].transform.localScale = Vector3.one;
            skillImages[i].color = new Color(skillImages[i].color.r, skillImages[i].color.g, skillImages[i].color.b, 0.0f);
        }
    }

    UpdateSkillDisplay();
    }

    void UpdateSkillDisplay()
    {
        // スキル名の更新
        skillInfoText.text = skillData[currentIndex].skillName;
    }

    void ActivateSkill()
    {
        // 中央のスキルがクールタイム中でなければ発動
        if (skillData[currentIndex].coolTime <= 0)
        {
            skillData[currentIndex].coolTime = skillData[currentIndex].skillCoolTime; // クールタイム開始
            skillData[currentIndex].skillAction.Skill();
            // 発動処理（例: スキルのエフェクトや効果を実行）
            Debug.Log($"{skillData[currentIndex].skillName} 発動！");
        }
        else
        {
            Debug.Log($"{skillData[currentIndex].skillName} はまだクールタイム中です。");
        }

        // 発動直後にクールタイムを表示する
        // cooldownTexts[currentIndex].text = skillData[currentIndex].coolTime.ToString("F1");
    }
}