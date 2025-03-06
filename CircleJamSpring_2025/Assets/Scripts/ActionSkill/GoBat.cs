using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoBat", menuName = "SkillAction/GoBat")]
public class GoBat : SkillAction
{
    public override void Skill()
    {
        // 効果量
        float moovAmount = 10f;

        //コウモリに変身して横移動
        var player = GameObject.Find("ActionPlayer").GetComponent<ActionPlayer>();

        if (player == null)
        {
            return;
        }


        // できてる気がしない
        Rigidbody2D rb = player.GetRigidBody();

        rb.velocity = new Vector2(moovAmount, rb.velocity.y);


    }
}
