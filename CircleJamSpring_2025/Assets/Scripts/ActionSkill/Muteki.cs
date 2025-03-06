using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Muteki", menuName = "SkillAction/Muteki")]
public class Muteki : SkillAction
{
    public override void Skill()
    {


        // nankamuteki
        var player = GameObject.Find("ActionPlayer").GetComponent<ActionPlayer>();

        if (player == null)
        {
            return;
        }

        // ���G��Ԃ��J�n���A�R���[�`���ŉ����������s��
        player.mutekiFlag = true;
        player.StartCoroutine(MutekiTimer(player));
    }

    /// <summary>
    /// ��莞�Ԍ�ɖ��G��Ԃ���������R���[�`��
    /// </summary>
    private IEnumerator MutekiTimer(ActionPlayer player)
    {
        yield return new WaitForSeconds(1f); // 1�b�ҋ@

        // ���G��ԉ���
        player.mutekiFlag = false;
    }
}