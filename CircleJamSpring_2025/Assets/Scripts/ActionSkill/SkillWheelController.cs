using UnityEngine;
using UnityEngine.UI;

public class SkillWheelController : MonoBehaviour
{
    public Image[] skillImages;  // スキルアイコンのImage
    public Text skillInfoText;    // 中央のスキル名を表示するText
    // public Text[] cooldownTexts; // 各スキルのクールタイムを表示するText
    public SkillData[] skillData; // スキルデータ（ScriptableObject）
    private int currentIndex = 2; // 中央のスキル位置

    public Vector3[] initPos;

    void Start()
    {
        initPos = new Vector3[skillImages.Length];
        for(int i=0;i<skillImages.Length;i++)
        {
            initPos[i]=skillImages[i].gameObject.GetComponent<RectTransform>().anchoredPosition;
        }
        UpdateSkillDisplay();
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            RotateWheel(1); // 右スクロール
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
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
        for(int i = 0; i <skillImages.Length;i++)
        {
            if(i+direction >= skillImages.Length)
            {
                skillImages[i].gameObject.GetComponent<RectTransform>().anchoredPosition = initPos[0];
                print($"{i} = {skillImages[i].gameObject.name}");
                continue;
            }
            if(i+direction < 0)
            {
                skillImages[i].gameObject.GetComponent<RectTransform>().anchoredPosition = initPos[skillImages.Length-1];
                continue;
            }
            skillImages[i].gameObject.GetComponent<RectTransform>().anchoredPosition = initPos[i+direction];
        }
        if (direction == 1)
        {
            // 右スクロール: 最後の要素を先頭に移動

            Image lastImage = skillImages[skillImages.Length - 1];
            for (int i = skillImages.Length - 1; i > 0; i--)
            {
                skillImages[i] = skillImages[i - 1];
            }
            skillImages[0] = lastImage;
            // 右スクロール: 最後の要素を先頭に移動

            SkillData last = skillData[skillData.Length - 1];
            for (int i = skillData.Length - 1; i > 0; i--)
            {
                skillData[i] = skillData[i - 1];
            }
            skillData[0] = last;
        }
        else if (direction == -1)
        {
            // 左スクロール: 先頭の要素を最後に移動
            Image firstImage = skillImages[0];
            for (int i = 0; i < skillImages.Length - 1; i++)
            {
                skillImages[i] = skillImages[i + 1];
            }
            skillImages[skillImages.Length - 1] = firstImage;
            // 左スクロール: 先頭の要素を最後に移動
            SkillData first = skillData[0];
            for (int i = 0; i < skillData.Length - 1; i++)
            {
                skillData[i] = skillData[i + 1];
            }
            skillData[skillData.Length - 1] = first;
        }

        UpdateSkillDisplay();
    }

    void UpdateSkillDisplay()
    {
        for (int i = 0; i < skillImages.Length; i++)
        {
            skillImages[i].sprite = skillData[i].skillImage;

            // 透明度とサイズの更新
            if (i == currentIndex)
            {
                // 中央: 完全に表示、サイズは1.5倍
                skillImages[i].color = new Color(1, 1, 1, 1); // 透明度1.0
                // cooldownTexts[i].color = new Color(1, 1, 1, 1); // 透明度1.0
                skillImages[i].transform.localScale = new Vector3(1.5f, 1.5f, 1);
            }
            else if (i == (currentIndex - 1 + skillImages.Length) % skillImages.Length || i == (currentIndex + 1) % skillImages.Length)
            {
                // 左右: 半透明、サイズは通常
                skillImages[i].color = new Color(1, 1, 1, 0.5f); // 透明度0.5
                // cooldownTexts[i].color = new Color(1, 1, 1, 0.5f); // 透明度0.5
                skillImages[i].transform.localScale = Vector3.one;
            }
            else
            {
                // 端: 非表示、サイズは通常
                skillImages[i].color = new Color(1, 1, 1, 0.0f); // 透明度0.0
                // cooldownTexts[i].color = new Color(1, 1, 1, 0.0f); // 透明度0.0
                skillImages[i].transform.localScale = Vector3.one;
            }
        }

        skillInfoText.text = skillData[currentIndex].skillName;
    }

    void ActivateSkill()
    {
        // 中央のスキルがクールタイム中でなければ発動
        if (skillData[currentIndex].coolTime <= 0)
        {
            skillData[currentIndex].coolTime = skillData[currentIndex].skillCoolTime; // クールタイム開始
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