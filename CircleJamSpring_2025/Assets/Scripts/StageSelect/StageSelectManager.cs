using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// todo stageRelease�ɃX�e�[�W�N���X����Q�Ƃ��Ď擾����
// todo ���̃A�j���[�V�����������̂��C������

public class StageSelectManager : MonoBehaviour
{
    //#region  �ϐ��錾

    //[SerializeField, Tooltip("�X�e�[�W�����i�[����Ă���I�u�W�F�N�g���i�[")]
    //private GameObject stage;
    //[SerializeField, Tooltip("�X�e�[�W���̃v���n�u���i�[")]
    //private GameObject stageNamePrefab;
    //[SerializeField, Tooltip("�\������X�e�[�W�����i�[")]
    //private string[] stageName;
    //[SerializeField, Tooltip("�X�e�[�W��̃L�������i�[")]
    //private GameObject collegeStudent;
    //[SerializeField, Tooltip("�X�e�[�W(�Ԋ�)���i�[")]
    //private GameObject[] stageObject;
    //[SerializeField, Tooltip("UI�̒��S���W���i�[")]
    //private GameObject CenterUI;
    //[SerializeField, Tooltip("�x���e�L�X�g���i�[")]
    //private GameObject[] errorTexts;
    //[SerializeField, Tooltip("�X�e�[�W�̉��ɕ\����������i�[")]
    //private GameObject[] arrowImages;
    //[SerializeField, Tooltip("�X�e�[�W�̊Ԃɒu���ۂ��i�[")]
    //private GameObject circle;
    //[SerializeField, Tooltip("�퓬�J�n�e�L�X�g�̐e�I�u�W�F�N�g���i�[")]
    //private GameObject battleTextObject;

    //public int stageRelease;                // ���݉������Ă���X�e�[�W��

    //private GameObject[] stageNameObject;   // �X�e�[�W���I�u�W�F�N�g���i�[
    //private Camera mainCameraObject;        // �J�����̃I�u�W�F�N�g
    //private Vector3 startPos;               // �^�b�v���ꂽ���W
    //private Vector3 endPos;                 // �����Ă���Ԃ̍��W
    //private bool isTap;                     // �^�b�v���Ă��邩�ǂ���
    //private bool isMove;                    // �����邩�ǂ���
    //private float speed;                    // �X�e�[�W���I�u�W�F�N�g�𓮂����X�s�[�h
    //private float startPosTime;             // startPosTime�̃R�s�[��ۑ�
    //private float direction;                // �^�b�v���ꂽ���W���牟���ꂽ���W���������l���i�[
    //private float[] stageNamePos;           // �X�e�[�W�̍��W���i�[
    //private float[] stageNameSpace;         // �X�e�[�W���I�u�W�F�N�g�ǂ����̊�
    //private int count = 0;
    //private int stageIndex = 0;
    //private int stageSpaceIndex = 0;
    //private int clearCnt;                   // �N���A���Ă���X�e�[�W�����i�[

    //private Color halfAlpha = new Color(1.0f, 1.0f, 1.0f, 0.5f);    // ���l�������̕ϐ�
    //private Color maxAlpha = new Color(1.0f, 1.0f, 1.0f, 1.0f);     // ���l���ő�̕ϐ�(��������Ȃ��Ȃ�)

    //private Animator animator;
    //private AudioSource audioSource;
    //private int battleTextIndex;
    //private UnityEngine.Object[] battleObjects = new UnityEngine.Object[5];

    //#endregion

    //// Start is called before the first frame update
    //void Start()
    //{
    //    // ������
    //    Init();
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    ///// <summary>
    ///// ������
    ///// </summary>
    //void Init()
    //{
        
    //}

    ///// <summary>
    ///// circle�𐶐�
    ///// </summary>
    //void CreateCircle()
    //{
    //    // �X�e�[�W�I�u�W�F�N�g�̊ԂɊԊu��������circle�𐶐�
    //    for (int i = 0; i < clearCnt; i++)
    //    {
    //        var pos = stageObject[i].transform.position;
    //        var nextPos = stageObject[i + 1].transform.position;
    //        var distance = pos - nextPos;

    //        var spaceX = Mathf.Abs(distance.x / Common.SS_CIRCLE_SPACE);
    //        var spaceY = Mathf.Abs(distance.y / Common.SS_CIRCLE_SPACE);

    //        if (spaceX > spaceY)
    //        {
    //            spaceY = Mathf.Abs(distance.y / spaceX);
    //            spaceX = Common.SS_CIRCLE_SPACE;

    //            if (distance.y == 0)
    //            {
    //                spaceY = Common.SS_CIRCLE_SPACE;
    //            }
    //        }
    //        else if (spaceX < spaceY)
    //        {
    //            spaceX = Mathf.Abs(distance.x / spaceY);
    //            spaceY = Common.SS_CIRCLE_SPACE;

    //            if (distance.x == 0)
    //            {
    //                spaceX = Common.SS_CIRCLE_SPACE;
    //            }
    //        }
    //        else
    //        {
    //            spaceX = Common.SS_CIRCLE_SPACE;
    //            spaceY = Common.SS_CIRCLE_SPACE;
    //        }

    //        int cnt = 0;
    //        var minDistance = 0.5f;
    //        while (Mathf.Abs(distance.x) > minDistance || Mathf.Abs(distance.y) > minDistance)
    //        {
    //            if (cnt > 100)
    //            {
    //                break;
    //            }

    //            if (distance.x > 0)
    //            {
    //                pos.x += -spaceX;
    //                distance.x += -spaceX;
    //            }
    //            else if (distance.x < 0)
    //            {
    //                pos.x += spaceX;
    //                distance.x += spaceX;
    //            }
    //            if (distance.y > 0)
    //            {
    //                pos.y += -spaceY;
    //                distance.y += -spaceY;
    //            }
    //            else if (distance.y < 0)
    //            {
    //                pos.y += spaceY;
    //                distance.y += spaceY;
    //            }

    //            Instantiate(circle, pos, Quaternion.identity);
    //            cnt++;
    //        }
    //    }
    //}
}
