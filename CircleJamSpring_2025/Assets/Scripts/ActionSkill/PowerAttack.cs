using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerAttack ", menuName = "SkillAction/PowerAttack ")]
public class PowerAttack : SkillAction
{
    public override void Skill()
    {
        //�߂����ይ���U��
        var player = GameObject.Find("ActionPlayer").GetComponent<ActionPlayer>();

        if (player == null)
        {
            return;
        }

        // �v���C���[�̍U���͈͂��L����
        // �X�P�[����0.4�ɕύX






    }
}