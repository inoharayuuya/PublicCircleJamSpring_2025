using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerAttack ", menuName = "SkillAction/PowerAttack ")]
public class PowerAttack : SkillAction
{
    public override void Skill()
    {
        //めっちゃ強い攻撃
        var player = GameObject.Find("ActionPlayer").GetComponent<ActionPlayer>();

        if (player == null)
        {
            return;
        }

        // プレイヤーの攻撃範囲を広げる
        // スケールを0.4に変更






    }
}