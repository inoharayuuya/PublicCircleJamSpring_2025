using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public enum Skill
    {
        HightJump,      //いい感じに踏ん張って高く飛ぶ
        GoBat,          //コウモリに変身して横移動
        SecretRecovery, //秘密の血液瓶で回復
        Muteki,         //なんか無敵になる
        PowerAttack     //めっちゃ強い攻撃
    }
    class Common
    {
        public const int    SS_MAX_STAGE       = 48;     // マックスのステージ数
        public const float  SS_CAMERA_SPEED    = 10f;    // カメラの移動スピード
        public const float  SS_CHARA_SPEED     = 2.5f;   // キャラクターの移動スピード
        public const float  SS_INIT_SUM        = 10000f; // sumの初期値
        public const float  SS_SLOW_DOWN_SPEED = 0.97f;  // 減速率
        public const float  SS_MIN_SPEED       = 0.05f;  // スピードの最小値
        public const float  SS_DIRECTION_SPEED = 1000f;  // ステージ名オブジェクトをステージ名の中心まで動かす時のスピード
        public const float  SS_START_POS_TIME  = 0.1f;   // startPosの更新頻度
        public const float  SS_INIT_SPEED      = 0.5f;   // スピードの初期値
        public const float  SS_CIRCLE_SPACE    = 0.5f;   // 生成するcircleの間隔

        public static Color HALF_ALPHA         = new Color(1.0f, 1.0f, 1.0f, 0.5f);    // 半透明
        public static Color MAX_ALPHA          = new Color(1.0f, 1.0f, 1.0f, 1.0f);    // 不透明
    }
}