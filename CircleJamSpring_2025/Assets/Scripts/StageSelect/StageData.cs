using System;
using System.Collections.Generic;

[Serializable]
public class StageData
{
    public List<Stage> list_stage;  // �S�X�e�[�W�f�[�^
    public StageData()
    {
        list_stage = new List<Stage>();
    }
}

[Serializable]
public class Stage
{
    public string stage_id;         // �X�e�[�WID
    public string stage_name;       // �X�e�[�W��
    public bool   is_stage_clear;   // �X�e�[�W���N���A���Ă��邩�ǂ���
}
