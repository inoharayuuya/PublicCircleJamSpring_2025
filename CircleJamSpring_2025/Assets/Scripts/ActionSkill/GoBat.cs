using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoBat", menuName = "SkillAction/GoBat")]
public class GoBat : SkillAction
{
    public override void Skill()
    {
        //コウモリに変身して横移動
        var player = GameObject.Find("ActionPlayer").GetComponent<ActionPlayer>();

        if (player == null)
        {
            return;
        }

        //player.rb.velocity = new Vector2(speed, rb.velocity.y);


    }
}
