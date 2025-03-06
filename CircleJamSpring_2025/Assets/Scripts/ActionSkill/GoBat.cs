using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoBat", menuName = "SkillAction/GoBat")]
public class GoBat : SkillAction
{
    public override void Skill()
    {
        // 効果量
        float moovAmount = 15f;

        //コウモリに変身して横移動
        var player = GameObject.Find("ActionPlayer").GetComponent<ActionPlayer>();

        if (player == null)
        {
            return;
        }


        player.SkillGoBad();





    }
}
