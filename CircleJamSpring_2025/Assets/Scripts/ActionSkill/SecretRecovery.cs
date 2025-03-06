using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SecretRecovery", menuName = "SkillAction/SecretRecovery")]
public class SecretRecovery : SkillAction
{
    public override void Skill()
    {
        // Œø‰Ê—Ê
        float healAmount = 20f;

        //”é–§‚ÌŒŒ‰t•r‚Å‰ñ•œ 
        var player = GameObject.Find("ActionPlayer").GetComponent<ActionPlayer>();

        if (player == null)
        {
            return;
        }

        player.healthPoint += healAmount;

    }
}