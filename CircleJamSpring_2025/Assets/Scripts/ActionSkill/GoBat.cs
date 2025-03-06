using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoBat", menuName = "SkillAction/GoBat")]
public class GoBat : SkillAction
{
    public override void Skill()
    {
        //ƒRƒEƒ‚ƒŠ‚É•Ïg‚µ‚Ä‰¡ˆÚ“®
        var player = GameObject.Find("ActionPlayer").GetComponent<ActionPlayer>();

        if (player == null)
        {
            return;
        }

        //player.rb.velocity = new Vector2(speed, rb.velocity.y);


    }
}
