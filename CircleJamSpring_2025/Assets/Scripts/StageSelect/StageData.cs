using System;
using System.Collections.Generic;

[Serializable]
public class StageData
{
    public List<Stage> list_stage;  // 全ステージデータ
    public StageData()
    {
        list_stage = new List<Stage>();
    }
}

[Serializable]
public class Stage
{
    public string stage_id;         // ステージID
    public string stage_name;       // ステージ名
    public bool   is_stage_clear;   // ステージをクリアしているかどうか
}
