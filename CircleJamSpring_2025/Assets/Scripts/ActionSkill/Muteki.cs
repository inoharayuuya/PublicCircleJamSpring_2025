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

        // 無敵状態を開始し、コルーチンで解除処理を行う
        player.mutekiFlag = true;
        player.StartCoroutine(MutekiTimer(player));
    }

    /// <summary>
    /// 一定時間後に無敵状態を解除するコルーチン
    /// </summary>
    private IEnumerator MutekiTimer(ActionPlayer player)
    {
        yield return new WaitForSeconds(1f); // 1秒待機

        // 無敵状態解除
        player.mutekiFlag = false;
    }
}