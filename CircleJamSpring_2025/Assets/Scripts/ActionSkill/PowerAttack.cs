using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
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
        // �X�P�[����0.6�ɕύX
        Debug.Log($"test;{player.transform.GetChild(1)}");
        player.transform.GetChild(1).localScale = new Vector3(0.6f, 0.6f, 0);
        player.SkillAttack();




    }
}