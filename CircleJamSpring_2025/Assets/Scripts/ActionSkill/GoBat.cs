using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoBat", menuName = "SkillAction/GoBat")]
public class GoBat : SkillAction
{
    public override void Skill()
    {
        // ���ʗ�
        float moovAmount = 10f;

        //�R�E�����ɕϐg���ĉ��ړ�
        var player = GameObject.Find("ActionPlayer").GetComponent<ActionPlayer>();

        if (player == null)
        {
            return;
        }


        // �ł��Ă�C�����Ȃ�
        Rigidbody2D rb = player.GetRigidBody();

        rb.velocity = new Vector2(moovAmount, rb.velocity.y);


    }
}
