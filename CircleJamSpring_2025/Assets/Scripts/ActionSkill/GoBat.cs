using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoBat", menuName = "SkillAction/GoBat")]
public class GoBat : SkillAction
{
    public override void Skill()
    {
        // Œø‰Ê—Ê
        float moovAmount = 15f;

        //ƒRƒEƒ‚ƒŠ‚É•Ïg‚µ‚Ä‰¡ˆÚ“®
        var player = GameObject.Find("ActionPlayer").GetComponent<ActionPlayer>();

        if (player == null)
        {
            return;
        }


        player.SkillGoBad();





    }
}
