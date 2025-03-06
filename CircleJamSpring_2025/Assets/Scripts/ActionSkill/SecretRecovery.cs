using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SecretRecovery", menuName = "SkillAction/SecretRecovery")]
public class SecretRecovery : SkillAction
{
    public override void Skill()
    {
        // ���ʗ�
        float healAmount = 20f;

        //�閧�̌��t�r�ŉ� 
        var player = GameObject.Find("ActionPlayer").GetComponent<ActionPlayer>();

        if (player == null)
        {
            return;
        }

        player.healthPoint += healAmount;

    }
}