using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

[Serializable]
public class StageData
{
    public List<Stage> list_stage;  // �S�X�e�[�W�f�[�^
}

[Serializable]
public class Stage
{
    public string    stage_id;          // �X�e�[�WID
    public string    stage_name;        // �X�e�[�W��
    public bool      is_stage_clear;    // �X�e�[�W���N���A���Ă��邩�ǂ���
    public StageType stage_type;        // �X�e�[�W�̃^�C�v(�����Ƃ��A�N�V�����Ƃ�)
    public Vector3   position;          // �X�e�[�W�̎��̂�u���ʒu(World���W)
}
