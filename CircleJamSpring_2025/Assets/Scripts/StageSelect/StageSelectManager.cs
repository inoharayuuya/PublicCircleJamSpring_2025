using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class StageSelectManager : MonoBehaviour
{
    #region  �ϐ��錾
    [Header("-�X�e�[�W�I�u�W�F�N�g�֘A-")]
    [Tooltip("�X�e�[�W(�Ԋ�)�̃v���n�u���Z�b�g")]
    [SerializeField] GameObject stagePrefab;

    [Tooltip("�X�e�[�W�̊Ԃɒu���ۂ̃v���n�u���Z�b�g")]
    [SerializeField] GameObject circle;

    [Tooltip("�X�e�[�W(�Ԋ�)�̎��̂��Z�b�g")]
    GameObject[] stageObject;

    [Tooltip("�X�e�[�W�̍��W���Z�b�g")]
    float[] stageNamePos;

    [Header("-�X�e�[�W���֘A-")]
    [Tooltip("�V�[����̃X�e�[�W�����Z�b�g����e�I�u�W�F�N�g")]
    [SerializeField] GameObject stageParent;

    [Tooltip("�X�e�[�W���̃v���n�u���Z�b�g")]
    [SerializeField] GameObject stageNamePrefab;

    [Tooltip("UI�̒��S���W���Z�b�g")]
    [SerializeField] GameObject CenterUI;

    [Tooltip("�X�e�[�W�̉��ɕ\����������Z�b�g")]
    [SerializeField] GameObject[] arrowImages;

    [Tooltip("�X�e�[�W���I�u�W�F�N�g���Z�b�g")]
    GameObject[] stageNameObject;

    [Tooltip("�X�e�[�W���I�u�W�F�N�g�ǂ����̊Ԃ��Z�b�g")]
    float[] stageNameSpace;

    [Header("-�L�����N�^�[�֘A-")]
    [Tooltip("�X�e�[�W��ɔz�u���郔�@���p�C�A���Z�b�g")]
    [SerializeField] GameObject vampire;

    [Tooltip("���@���p�C�A�̃A�j���[�V����")]
    Animator animator;

    [Header("-�J�����֘A-")]
    [Tooltip("�J�����̃I�u�W�F�N�g���Z�b�g")]
    Camera mainCameraObject;

    [Tooltip("�^�b�v���ꂽ���W���Z�b�g")]
    Vector3 startPos;

    [Tooltip("�����Ă���Ԃ̍��W���Z�b�g")]
    Vector3 endPos;

    [Tooltip("�^�b�v���Ă��邩�ǂ������Z�b�g")]
    bool isTap;

    [Tooltip("�����邩�ǂ������Z�b�g")]
    bool isMove;

    [Tooltip("�X�e�[�W���I�u�W�F�N�g�𓮂����X�s�[�h���Z�b�g")]
    float speed;

    [Tooltip("startPosTime�̃R�s�[��ۑ����Z�b�g")]
    float startPosTime;

    [Tooltip("�^�b�v���ꂽ���W���牟���ꂽ���W���������l���Z�b�g")]
    float direction;

    [Tooltip("�J�E���g�p")]
    int count = 0;

    [Tooltip("���݂̃X�e�[�W�̒��S�̃C���f�b�N�X���Z�b�g")]
    int stageIndex = 0;

    [Tooltip("���݂̃X�e�[�W�̒��S�̃C���f�b�N�X���Z�b�g")]
    int stageSpaceIndex = 0;

    [Tooltip("�X�e�[�W�N���A����Ă���X�e�[�W���J�E���g����p")]
    int clearCnt;

    [Tooltip("�X�e�[�W�f�[�^���Z�b�g")]
    StageData stageData;
    #endregion

    #region Unity�f�t�H���g�C�x���g
    // Start is called before the first frame update
    void Start()
    {
        // ������
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // ��ʂ�������Ă��邩�𔻒肷��
        TapCheck();

        // �X�e�[�W�̈ړ�����
        StageMove();

        // �X�e�[�W���̒��S���W���擾
        GetCenter();
    }
    #endregion

    /// <summary>
    /// ������
    /// </summary>
    void Init()
    {
        animator     = vampire.GetComponent<Animator>();    // ���@���p�C�A�̃A�j���[�^�[��ϐ��ɃZ�b�g
        startPos     = Vector3.zero;                        // �^�b�v���ꂽ�n�߂̈ʒu
        endPos       = Vector3.zero;                        // �^�b�v���I������Ƃ��̈ʒu
        speed        = Common.SS_INIT_SPEED;                // �ݒ肵�������X�s�[�h�ŏ�����
        startPosTime = Common.SS_START_POS_TIME;            // startPos�̒l���X�V����X�V�p�x
        direction    = 0;                                   // �^�b�v���ꂽ�_�̋���
        clearCnt     = 0;                                   // �N���A���Ă���X�e�[�W�̐�

        var jsonStage = Resources.Load<TextAsset>(Common.SS_STAGE_DATA_FILE); // Stage.Json����ǂݍ���
        if (jsonStage != null)
        {
            stageData = JsonUtility.FromJson<StageData>(jsonStage.text);

            foreach (var stage in stageData.list_stage)
            {
                if (stage.is_stage_clear)
                {
                    // �N���A�ς݂̏ꍇ�̂݃J�E���g�A�b�v
                    clearCnt++;
                }

                // �f�o�b�O�p
                //print($"stage_id: {stage.stage_id}");
                //print($"stage_name: {stage.stage_name}");
                //print($"is_stage_clear: {stage.is_stage_clear}");
                //print($"position: {stage.position}");
            }
        }
        else
        {
            // �t�@�C����������Ȃ������ꍇ�ɃG���[���O���o��
            Debug.LogError("JSON file not found!");
            return;
        }

        // ���ׂẴX�e�[�W���N���A���Ă�����N���A�J�E���g��-1����
        if (stageData.list_stage.Count == clearCnt)
        {
            clearCnt--;
        }

        // �N���A���Ă��鐔�ŏ���������
        stageObject     = new GameObject[clearCnt + 1];
        stageNameObject = new GameObject[clearCnt + 1];
        stageNamePos    = new float[clearCnt + 1];
        stageNameSpace  = new float[clearCnt + 1];

        // �X�e�[�W�̓N���A����Ă��镪�����\�����Ȃ��悤�ɂ���
        for (int i = 0; i < clearCnt + 1; i++)
        {
            // �X�e�[�W�𐶐�
            var stage      = Instantiate<GameObject>(stagePrefab, stageData.list_stage[i].position, Quaternion.identity);   // ��������
            stageObject[i] = stage;                                                                                         // �Ǘ����₷�����邽�߂ɔz��ɃZ�b�g

            // �X�e�[�W���𐶐�
            var instance       = Instantiate<GameObject>(stageNamePrefab, stageParent.transform);           // ��������
            stageNameObject[i] = instance;                                                                  // �Ǘ����₷�����邽�߂ɔz��ɃZ�b�g

            // �X�e�[�W����ύX
            stageNameObject[i].name = stageData.list_stage[i].stage_name;                                   // �q�G�����L�[��̖��O��ύX
            stageObject[i].name     = stageData.list_stage[i].stage_name;                                   // �q�G�����L�[��̖��O��ύX
            stageNameObject[i].GetComponentInChildren<Text>().text = stageData.list_stage[i].stage_name;    // �\������e�L�X�g��ύX

            // �X�e�[�W���̈ʒu���Z�b�g
            stageNamePos[i] = stageData.list_stage[i].position.x;
        }

        // �X�e�[�W�ƃL�����A�J�����̍��W�Ɏg���ϐ�
        var pos = stageObject[0].transform.position;  // ������stageObject�̃C���f�b�N�X��ς���΃X�e�[�W�̏������W���ς��

        // �X�e�[�W�̏����l���Z�b�g
        var stagePos = stageParent.transform.position;
        stagePos.x   = -pos.x;
        stageParent.transform.position = stagePos;

        isTap  = false;
        isMove = false;

        // �ŏ��ɑI������Ă���X�e�[�W�̃��l��ς���
        stageNameObject[0].GetComponent<Image>().color = Common.MAX_ALPHA;

        // �L�����̏������W���Z�b�g
        var collegeStudentPos      = vampire.transform.position;
        collegeStudentPos.x        = pos.x;
        vampire.transform.position = collegeStudentPos;

        // �J�����I�u�W�F�N�g�擾
        mainCameraObject = Camera.main;

        // �J�����̏������W���Z�b�g
        var cameraPos = mainCameraObject.transform.position;
        cameraPos.x   = pos.x;
        mainCameraObject.transform.position = cameraPos;

        // �X�e�[�W�̊Ԃ̊ۂ𐶐�
        CreateCircle();
    }

    /// <summary>
    /// circle�𐶐�
    /// </summary>
    void CreateCircle()
    {
        // �X�e�[�W�I�u�W�F�N�g�̊ԂɊԊu��������circle�𐶐�
        for (int i = 0; i < clearCnt; i++)
        {
            // �������v�Z
            var pos      = stageObject[i].transform.position;
            var nextPos  = stageObject[i + 1].transform.position;
            var distance = pos - nextPos;

            // �������X�y�[�X�Ŋ���
            var spaceX = Mathf.Abs(distance.x / Common.SS_CIRCLE_SPACE);
            var spaceY = Mathf.Abs(distance.y / Common.SS_CIRCLE_SPACE);

            // x��y�łǂ����̕�������Ă��邩����
            if (spaceX > spaceY)
            {
                // x����Ōv�Z����
                spaceY = Mathf.Abs(distance.y / spaceX);
                spaceX = Common.SS_CIRCLE_SPACE;

                // y���X�y�[�X�Ŋ���؂��ꍇ
                if (distance.y == 0)
                {
                    // �X�y�[�X�ŏ㏑��
                    spaceY = Common.SS_CIRCLE_SPACE;
                }
            }
            else if (spaceX < spaceY)
            {
                // y����Ōv�Z����
                spaceX = Mathf.Abs(distance.x / spaceY);
                spaceY = Common.SS_CIRCLE_SPACE;

                // x���X�y�[�X�Ŋ���؂��ꍇ
                if (distance.x == 0)
                {
                    // �X�y�[�X�ŏ㏑��
                    spaceX = Common.SS_CIRCLE_SPACE;
                }
            }
            else
            {
                // �����������ꍇ
                spaceX = Common.SS_CIRCLE_SPACE;
                spaceY = Common.SS_CIRCLE_SPACE;
            }

            int cnt = 0;
            var minDistance = Common.SS_CIRCLE_SPACE;

            // �_����������
            while (Mathf.Abs(distance.x) > minDistance || Mathf.Abs(distance.y) > minDistance)
            {
                if (cnt > 100)
                {
                    // 100�u���Ă��܂����[�v����ꍇ�͗��ꂷ���Ă���Ɣ��肷��
                    Debug.LogError("�X�e�[�W�Ԃ����ꂷ���Ă��܂�");
                    break;
                }

                if (distance.x > 0)
                {
                    // �������v���X�̏ꍇ
                    pos.x      += -spaceX;
                    distance.x += -spaceX;
                }
                else if (distance.x < 0)
                {
                    // �������}�C�i�X�̏ꍇ
                    pos.x      += spaceX;
                    distance.x += spaceX;
                }

                if (distance.y > 0)
                {
                    // �������v���X�̏ꍇ
                    pos.y      += -spaceY;
                    distance.y += -spaceY;
                }
                else if (distance.y < 0)
                {
                    // �������}�C�i�X�̏ꍇ
                    pos.y      += spaceY;
                    distance.y += spaceY;
                }

                // �_���𐶐�
                Instantiate(circle, pos, Quaternion.identity);
                cnt++;
            }
        }
    }

    /// <summary>
    /// �^�b�v�`�F�b�N����
    /// </summary>
    void TapCheck()
    {
        // ��ʂ��^�b�v���ꂽ�Ƃ�
        if (Input.GetMouseButtonDown(0))
        {
            // startPos�ɉ�ʂ������ꂽ���W��������
            startPos = Input.mousePosition;

            // ��ʂ�������Ă���̂�true�ɂ���
            isTap = true;

            // ������悤�ɂȂ�̂�true
            isMove = true;
        }

        // ��ʂ��^�b�v����Ă����
        if (Input.GetMouseButton(0))
        {
            // endPos�ɂ͖��t���[����ʂ�������Ă�����W��������
            endPos = Input.mousePosition;

            // �X�V���Ԃ�����܂�deltaTime�Ōv�Z
            startPosTime -= Time.deltaTime;

            // �X�s�[�h�͉�����邽�тɏ���������
            speed = Common.SS_INIT_SPEED;

            // startPosTime��0�ɂȂ邽�т�startPos�����݂�endPos�ŏ�����
            if (startPosTime <= 0)
            {
                // startPosTime�̏�����
                startPosTime = Common.SS_START_POS_TIME;
                startPos     = endPos;
            }
        }

        // ��ʂ���w�������ꂽ�Ƃ�
        if (Input.GetMouseButtonUp(0))
        {
            // �w�������ꂽ�̂�false
            isTap = false;
        }

        // ��ʂ�������Ă��Ȃ��Ԃ͌�����������
        if (!isTap)
        {
            // ���������|��������
            speed *= Common.SS_SLOW_DOWN_SPEED;

            if (speed < 0.05f || Mathf.Abs(direction) < 50)
            {
                // �ʏ�̈ړ������������̂�false
                isMove = false;

                // �ǔ����钆�S�_���m�肳����
                stageIndex = CheckCenter();

                // �X�e�[�W���̒��S�Ɋ�点��
                if (stageParent.GetComponent<RectTransform>().localPosition.x < -(stageNamePos[stageIndex] + 20))
                {
                    stageParent.GetComponent<RectTransform>().localPosition += new Vector3(Common.SS_DIRECTION_SPEED, 0, 0) * Time.deltaTime;
                }
                else if (stageParent.GetComponent<RectTransform>().localPosition.x > -(stageNamePos[stageIndex] - 20))
                {
                    stageParent.GetComponent<RectTransform>().localPosition += new Vector3(-Common.SS_DIRECTION_SPEED, 0, 0) * Time.deltaTime;
                }
                else
                {
                    if (speed < Common.SS_MIN_SPEED)
                    {
                        // ���S�̍��W����
                        stageParent.GetComponent<RectTransform>().localPosition = new Vector3(-stageNamePos[stageIndex],
                                                                                  stageParent.GetComponent<RectTransform>().localPosition.y,
                                                                                  stageParent.GetComponent<RectTransform>().localPosition.z);
                    }

                    // �~�߂�
                    speed = 0;
                }
            }
        }
    }

    /// <summary>
    /// �X�e�[�W���̈ړ�����
    /// </summary>
    void StageMove()
    {
        // startPos��endPos���v�Z���ċ��������߂�
        direction = (endPos.x - startPos.x) * speed;

        if (isMove)
        {
            // �v�Z���ʂ��v���X�̏ꍇ(0���܂܂Ȃ�)
            if (direction > 0)
            {
                // �X�e�[�W���I�u�W�F�N�g��0����(�l���}�C�i�X)�̎������ړ�������
                if (stageParent.transform.position.x < CenterUI.transform.position.x)
                {
                    stageParent.transform.position += new Vector3((direction * speed), 0, 0) * Time.deltaTime;
                }
            }
            else
            {
                // �X�e�[�W���I�u�W�F�N�g��0�ȏ�̎������ړ�����
                if (stageNameObject[stageNameObject.Length - 1].transform.position.x >= CenterUI.transform.position.x)
                {
                    stageParent.transform.position += new Vector3((direction * speed), 0, 0) * Time.deltaTime;
                }
            }
        }

        // ��ԋ߂����S���W�̃C���f�b�N�X���擾
        stageSpaceIndex = CheckCenter();

        if (speed < Common.SS_MIN_SPEED)
        {
            isMove = false;
            speed  = 0;
        }

        // ���S���W�Ɉ�ԋ߂��I�u�W�F�N�g�̓����x���Ȃ��ɂ���
        stageNameObject[stageSpaceIndex].GetComponent<Image>().color          = Common.MAX_ALPHA;
        stageNameObject[stageSpaceIndex].GetComponentInChildren<Text>().color = new Color(1f, 0.5f, 0, 1f);

        // ���S���W�Ɉ�ԋ߂��I�u�W�F�N�g�̃T�C�Y��ύX����
        var size = 1.25f;
        stageNameObject[stageSpaceIndex].GetComponent<RectTransform>().localScale = new Vector3(size, size, size);

        // �L�������X�e�[�W���ړ�����
        CharaMove();

        // �J�������X�e�[�W���ړ�����
        CameraMove();

        // �P�X�e�[�W�̂ݕ\������Ă����ꍇ
        if (stageNameObject.Length == 1)
        {
            print("���off");
            var tmp1 = arrowImages[0].GetComponent<Image>().color;
            tmp1.a   = Common.MIN_ALPHA.a;
            arrowImages[0].GetComponent<Image>().color = tmp1;

            var tmp2 = arrowImages[0].GetComponent<Image>().color;
            tmp2.a   = Common.MIN_ALPHA.a;
            arrowImages[1].GetComponent<Image>().color = tmp2;

            return;
        }

        if (stageSpaceIndex == 0)
        {
            // ��ԍ��ɂ���ꍇ
            // �����̖����\��
            var tmp1 = arrowImages[0].GetComponent<Image>().color;
            tmp1.a   = Common.MIN_ALPHA.a;
            arrowImages[0].GetComponent<Image>().color = tmp1;
            var tmp2 = arrowImages[1].GetComponent<Image>().color;
            tmp2.a   = Common.HALF_ALPHA.a;
            arrowImages[1].GetComponent<Image>().color = tmp2;
        }
        else if (stageSpaceIndex == stageNameObject.Length - 1)
        {
            // ��ԉE�ɂ���ꍇ
            // �E���̖����\��
            var tmp1 = arrowImages[0].GetComponent<Image>().color;
            tmp1.a   = Common.HALF_ALPHA.a;
            arrowImages[0].GetComponent<Image>().color = tmp1;
            var tmp2 = arrowImages[1].GetComponent<Image>().color;
            tmp2.a   = Common.MIN_ALPHA.a;
            arrowImages[1].GetComponent<Image>().color = tmp2;
        }
        else
        {
            // ����ȊO�̏ꍇ
            var tmp1 = arrowImages[0].GetComponent<Image>().color;
            tmp1.a = Common.HALF_ALPHA.a;
            arrowImages[0].GetComponent<Image>().color = tmp1;
            var tmp2 = arrowImages[1].GetComponent<Image>().color;
            tmp2.a = Common.HALF_ALPHA.a;
            arrowImages[1].GetComponent<Image>().color = tmp2;
        }
    }

    /// <summary>
    /// �X�e�[�W���ړ�����L�����̐���
    /// </summary>
    void CharaMove()
    {
        Vector2 direction = stageObject[stageSpaceIndex].transform.position - vampire.transform.position;

        if (direction.x > 0)
        {
            if (vampire.transform.position.x < stageObject[stageSpaceIndex].transform.position.x)
            {
                vampire.transform.position += new Vector3(direction.x, 0, 0) * Common.SS_CHARA_SPEED * Time.deltaTime;
                var lScale = vampire.transform.localScale;
                if (lScale.x <= 0)
                {
                    lScale.x *= -1;
                }
                vampire.transform.localScale = lScale;
            }
        }
        else
        {
            if (vampire.transform.position.x > stageObject[stageSpaceIndex].transform.position.x)
            {
                vampire.transform.position += new Vector3(direction.x, 0, 0) * Common.SS_CHARA_SPEED * Time.deltaTime;

                var lScale = vampire.transform.localScale;
                if (lScale.x >= 0)
                {
                    lScale.x *= -1;
                }
                vampire.transform.localScale = lScale;
            }
        }

        if (direction.y > 0)
        {
            if (vampire.transform.position.y < stageObject[stageSpaceIndex].transform.position.y)
            {
                vampire.transform.position += new Vector3(0, direction.y, 0) * Common.SS_CHARA_SPEED * Time.deltaTime;
            }
        }
        else
        {
            if (vampire.transform.position.y > stageObject[stageSpaceIndex].transform.position.y)
            {
                vampire.transform.position += new Vector3(0, direction.y, 0) * Common.SS_CHARA_SPEED * Time.deltaTime;
            }
        }

        float limit = 0.15f;

        if (MathF.Abs(direction.x) > limit || MathF.Abs(direction.y) > limit)
        {
            // �ړ����Ȃ̂ŃA�j���[�V�������Đ�����
            //animator.SetBool("isRun", true);
        }
        else
        {
            // �ړ����Ă��Ȃ��̂ŃA�j���[�V�������I������
            //animator.SetBool("isRun", false);
        }
    }

    /// <summary>
    /// �J�����ړ��̐���
    /// </summary>
    void CameraMove()
    {
        Vector2 direction = stageObject[stageSpaceIndex].transform.position - mainCameraObject.transform.position;

        if (direction.x > 0)
        {
            if (mainCameraObject.transform.position.x < stageObject[stageSpaceIndex].transform.position.x)
            {
                mainCameraObject.transform.position += new Vector3(direction.x, 0, 0) * Common.SS_CAMERA_SPEED * Time.deltaTime;
            }
        }
        else
        {
            if (mainCameraObject.transform.position.x > stageObject[stageSpaceIndex].transform.position.x)
            {
                mainCameraObject.transform.position += new Vector3(direction.x, 0, 0) * Common.SS_CAMERA_SPEED * Time.deltaTime;
            }
        }

        if (direction.y > 0)
        {
            if (mainCameraObject.transform.position.y < stageObject[stageSpaceIndex].transform.position.y)
            {
                mainCameraObject.transform.position += new Vector3(0, direction.y, 0) * Common.SS_CAMERA_SPEED * Time.deltaTime;
            }
        }
        else
        {
            if (mainCameraObject.transform.position.y > stageObject[stageSpaceIndex].transform.position.y)
            {
                mainCameraObject.transform.position += new Vector3(0, direction.y, 0) * Common.SS_CAMERA_SPEED * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// �X�e�[�W���̒��S���W���擾(1�t���[�������Čv�Z����)
    /// </summary>
    void GetCenter()
    {
        if (count == 1)
        {
            for (int i = 0; i < stageNamePos.Length; i++)
            {
                // ���S���W���擾
                stageNamePos[i] = stageNameObject[i].GetComponent<RectTransform>().localPosition.x;

                if (i != 0 && i < stageNamePos.Length - 1)
                {
                    // �X�e�[�W���̊Ԃ̍��W���擾
                    stageNameSpace[i - 1] = (stageNamePos[i]) - 250;
                }
            }

            CheckCenter();
        }

        count++;
    }

    /// <summary>
    /// ���t���[����ʒ��S�Ɉ�ԋ߂��X�e�[�W�̒��S���W���v�Z����
    /// </summary>
    /// <returns>���S�Ɉ�ԋ߂��������S���W�̃C���f�b�N�X��Ԃ�</returns>
    int CheckCenter()
    {
        float sum = Common.SS_INIT_SUM;
        int index = 0;

        for (int i = 0; i < stageNamePos.Length; i++)
        {
            // �X�e�[�W���I�u�W�F�N�g�̃��l������������
            stageNameObject[i].GetComponent<Image>().color = Common.HALF_ALPHA;
            stageNameObject[i].GetComponentInChildren<Text>().color = new Color(1f, 0.5f, 0, 0.5f);

            // �X�e�[�W���I�u�W�F�N�g�̃T�C�Y��������
            var size = 1f;
            stageNameObject[stageSpaceIndex].GetComponent<RectTransform>().localScale = new Vector3(size, size, size);

            // �X�e�[�W�̍��W�Ɗe�X�e�[�W�̒��S�_�̍��W�̍����v�Z
            var stageDirection = Mathf.Abs(Mathf.Abs(stageParent.GetComponent<RectTransform>().localPosition.x) - stageNamePos[i]);

            // �e�X�e�[�W���I�u�W�F�N�g�̋����𔻒�Asum��菬�������sum������������
            if (Mathf.Abs(sum) > stageDirection)
            {
                sum   = stageDirection;
                index = i;
            }
        }

        return index;
    }

    /// <summary>
    /// �o���{�^�����������Ƃ��̏���
    /// </summary>
    public void TapReadyButton()
    {
        // BGM���X�g�b�v����

        // SE���Đ�

        // ���ݑI�𒆂̃X�e�[�W���擾����
        print($"�I�������X�e�[�W��: {stageData.list_stage[stageIndex].stage_name}");
        print($"�I�������X�e�[�W�̃N���A��: {stageData.list_stage[stageIndex].is_stage_clear}");

        switch (stageData.list_stage[stageIndex].stage_type)
        {
            case StageType.Action:
                print("���̃X�e�[�W�̃^�C�v��Action�ł�");
                Common.LoadScene("ActionPlayer");
                break;

            case StageType.Run:
                print("���̃X�e�[�W�̃^�C�v��Run�ł�");
                Common.LoadScene("Run_Player");
                break;
        }
    }
}
