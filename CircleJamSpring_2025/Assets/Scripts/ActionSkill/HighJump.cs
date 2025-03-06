using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HighJump",menuName = "SkillAction/HighJump")]
public class HighJump : SkillAction
{
    public override void Skill()
    {
        //プレイヤーのジャンプ力を取得させる
        //取得したものにジャンプ力を追加する（この中で完結させる：プレイヤーの数値は変えない）
        //ジャンプ処理を呼ぶ
    }
}
