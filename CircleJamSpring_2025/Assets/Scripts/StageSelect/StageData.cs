using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

[Serializable]
public class StageData
{
    public List<Stage> list_stage;  // 全ステージデータ
}

[Serializable]
public class Stage
{
    public string    stage_id;          // ステージID
    public string    stage_name;        // ステージ名
    public bool      is_stage_clear;    // ステージをクリアしているかどうか
    public StageType stage_type;        // ステージのタイプ(ランとかアクションとか)
    public Vector3   position;          // ステージの実体を置く位置(World座標)
}
