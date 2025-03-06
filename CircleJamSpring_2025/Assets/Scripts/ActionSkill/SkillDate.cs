using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

[CreateAssetMenu(fileName = "Skill-0000",menuName = "SkillDate")]
public class SkillData : ScriptableObject
{
    [Header("スキルのID")]
    public string skillId = "Skill-0000";
    [Tooltip("スキルの名前")]
    public string skillName;
    [Tooltip("スキルの効果")]
    public Skill skillType;
    [Tooltip("スキルのクールタイム")]
    public float skillCoolTime = 2.0f;
    public float coolTime;
    [Header("スキルの画像")]
    public Sprite skillImage;
    [Header("スキルの処理")]
    public SkillAction skillAction;
}