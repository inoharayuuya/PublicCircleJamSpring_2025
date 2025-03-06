using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HighJump",menuName = "SkillAction/HighJump")]
public class HighJump : SkillAction
{
    public override void Skill()
    {
        //プレイヤーのジャンプ力を取得させる

        var player = GameObject.Find("ActionPlayer").GetComponent<ActionPlayer>();

        if (player == null)
        {
            return;
        }

        float jumpAdd = player.GetJumpAmount();
        //取得したものにジャンプ力を追加する（この中で完結させる：プレイヤーの数値は変えない）
        jumpAdd += 5f;
        //ジャンプ処理を呼ぶ
        player.StratJump(jumpAdd);

    }
}
