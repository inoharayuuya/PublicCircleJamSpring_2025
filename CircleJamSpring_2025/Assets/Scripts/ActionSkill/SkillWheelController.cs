using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillWheelController : MonoBehaviour
{
    public Image[] skillImages;  // スキルアイコンのImage
    public Text skillInfoText;    // 中央のスキル名を表示するText
    public Text[] cooldownTexts;  // 各スキルのクールタイム残り時間を表示するText
    public float animationDuration = 0.2f;  // アニメーションの時間

    private int currentIndex = 2;  // 初期位置（中央のスキル）
    public SkillData[] skillData;  // スキルデータ（ScriptableObject）
    private float[] skillCooldowns;  // 各スキルのクールタイム

        float moveUI = 1920f;
    float moveSpeed = 10f;
    int nowGame = 0;
    int maxGame = 9;

    void Start()
    {
        skillCooldowns = new float[skillData.Length];  // クールタイムの初期化
        SetSkillsInstantly();  // 初回のみ即座にセット
    }

    void Update()
    {
        for (int i = 0; i < skillCooldowns.Length; i++)
        {
            if (skillCooldowns[i] > 0)
            {
                skillCooldowns[i] -= Time.deltaTime;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            SwitchSkills(1);  // 右にスライド
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            SwitchSkills(-1);  // 左にスライド
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateSkill();
        }

        UpdateCooldownText();
    }

    void SetSkillsInstantly()
    {
        for (int i = 0; i < skillImages.Length; i++)
        {
            skillImages[i].sprite = skillData[i].skillImage;
            UpdateSkillAppearance(i);
        }
    }

    void SwitchSkills(int direction)
    {
        int previousIndex = currentIndex;
        currentIndex = (currentIndex + direction + skillImages.Length) % skillImages.Length;
        StartCoroutine(AnimateSkillSwitch(previousIndex, currentIndex));
    }

    void UpdateSkillAppearance(int index)
    {
        int distance = Mathf.Abs(index - currentIndex);
        if (distance > skillImages.Length / 2) distance = skillImages.Length - distance;

        skillImages[index].transform.localScale = (index == currentIndex) ? new Vector3(1.5f, 1.5f, 1) : new Vector3(1f, 1f, 1);
        skillImages[index].rectTransform.pivot = (index == currentIndex) ? new Vector2(0.5f, 0.325f) : new Vector2(0.5f, 0.5f);

        Color color = skillImages[index].color;
        color.a = (distance == 0) ? 1.0f : (distance == 1) ? 0.5f : 0.0f;
        skillImages[index].color = color;
    }

    IEnumerator AnimateSkillSwitch(int previousIndex, int newIndex)
    {
        float elapsedTime = 0f;
        Vector3 previousScale = skillImages[previousIndex].transform.localScale;
        Vector3 newScale = new Vector3(1.5f, 1.5f, 1);
        Vector3 defaultScale = new Vector3(1f, 1f, 1);

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            skillImages[previousIndex].transform.localScale = Vector3.Lerp(previousScale, defaultScale, t);
            skillImages[newIndex].transform.localScale = Vector3.Lerp(defaultScale, newScale, t);

            yield return null;
        }

        skillImages[previousIndex].transform.localScale = defaultScale;
        skillImages[newIndex].transform.localScale = newScale;

        for (int i = 0; i < skillImages.Length; i++)
        {
            UpdateSkillAppearance(i);
        }
        UpdateSkillText(newIndex);
    }

    void UpdateSkillText(int index)
    {
        if (index == currentIndex && skillInfoText != null)
        {
            skillInfoText.text = skillData[index].skillName;
        }
    }

    void UpdateCooldownText()
    {
        for (int i = 0; i < cooldownTexts.Length; i++)
        {
            if (cooldownTexts[i] != null)
            {
                cooldownTexts[i].text = skillCooldowns[i] > 0 ? skillCooldowns[i].ToString("F1") + "s" : "";
            }
        }
    }

    public void ActivateSkill()
    {
        if (skillCooldowns[currentIndex] <= 0)
        {
            skillCooldowns[currentIndex] = skillData[currentIndex].skillCoolTime;
        }
    }
}