using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class StageSelectManager : MonoBehaviour
{
    #region  �ϐ��錾
    [Tooltip("�V�[����̃X�e�[�W�����Z�b�g����e�I�u�W�F�N�g")]
    [SerializeField] GameObject stageParent;

    [Tooltip("�X�e�[�W���̃v���n�u���Z�b�g")]
    [SerializeField] GameObject stageNamePrefab;
    
    [Tooltip("�X�e�[�W��ɔz�u���郔�@���p�C�A���Z�b�g")]
    [SerializeField] GameObject vampire;

    [Tooltip("�X�e�[�W(�Ԋ�)�̃v���n�u���Z�b�g")]
    [SerializeField] GameObject stagePrefab;

    [Tooltip("UI�̒��S���W���Z�b�g")]
    [SerializeField] GameObject CenterUI;
    
    [Tooltip("�X�e�[�W�̉��ɕ\����������Z�b�g")]
    [SerializeField] GameObject[] arrowImages;
    
    [Tooltip("�X�e�[�W�̊Ԃɒu���ۂ̃v���n�u���Z�b�g")]
    [SerializeField] GameObject circle;

    [Tooltip("�X�e�[�W(�Ԋ�)�̎��̂��Z�b�g")]
    GameObject[] stageObject;

    [Tooltip("�X�e�[�W���I�u�W�F�N�g���Z�b�g")]
    GameObject[] stageNameObject;

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

    [Tooltip("�X�e�[�W�̍��W���Z�b�g")]
    float[] stageNamePos;

    [Tooltip("�X�e�[�W���I�u�W�F�N�g�ǂ����̊Ԃ��Z�b�g")]
    float[] stageNameSpace;

    [Tooltip("�J�E���g�p")]
    int count = 0;

    [Tooltip("���݂̃X�e�[�W�̒��S�̃C���f�b�N�X���Z�b�g")]
    int stageIndex = 0;

    [Tooltip("���݂̃X�e�[�W�̒��S�̃C���f�b�N�X���Z�b�g")]
    int stageSpaceIndex = 0;

    [Tooltip("���@���p�C�A�̃A�j���[�V����")]
    Animator animator;
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

        // �X�e�[�W�ƃL�����A�J�����̍��W�Ɏg���ϐ�
        var pos = stageObject[0].transform.position;  // ������stageObject�̃C���f�b�N�X��ς���΃X�e�[�W�̏������W���ς��

        // �X�e�[�W�̏����l���Z�b�g
        var stagePos = stageParent.transform.position;
        stagePos.x = -pos.x;
        stageParent.transform.position = stagePos;

        stageNameObject = new GameObject[Common.SS_MAX_STAGE];
        stageNamePos = new float[Common.SS_MAX_STAGE];
        stageNameSpace = new float[Common.SS_MAX_STAGE - 1];

        isTap = false;
        isMove = false;

        // �S�X�e�[�W�𐶐����A�q�G�����L�[�̖��O���ύX
        for (int i = 0; i < Common.SS_MAX_STAGE; i++)
        {
            GameObject instance = Instantiate(stageNamePrefab, stageParent.transform);
            stageNameObject[i] = instance;
        }

        // �\������X�e�[�W�����\��
        //for (int i = 0; i < clearCnt + 1; i++)
        //{
        //    stageNameObject[i].GetComponentInChildren<Text>().text = stageName[i];
        //}

        // �ŏ��ɑI������Ă���X�e�[�W�̃��l��ς���
        stageNameObject[0].GetComponent<Image>().color = Common.MAX_ALPHA;

        // �������ĂȂ��X�e�[�W���I�u�W�F�N�g���\���ɂ���
        //for (int i = clearCnt + 1; i < Common.SS_MAX_STAGE; i++)
        //{
        //    stageNameObject[i].SetActive(false);
        //    stageObject[i].SetActive(false);
        //}

        // �L�����̏������W���Z�b�g
        var collegeStudentPos = vampire.transform.position;
        collegeStudentPos.x = pos.x;
        vampire.transform.position = collegeStudentPos;

        // �J�����I�u�W�F�N�g�擾
        mainCameraObject = Camera.main;

        // �J�����̏������W���Z�b�g
        var cameraPos = mainCameraObject.transform.position;
        cameraPos.x = pos.x;
        mainCameraObject.transform.position = cameraPos;

        // �X�e�[�W�̊Ԃ̊ۂ𐶐�
        CreateCircle();
        //Invoke("CreateCircle", 0.5f);
    }

    /// <summary>
    /// circle�𐶐�
    /// </summary>
    void CreateCircle()
    {
        // �X�e�[�W�I�u�W�F�N�g�̊ԂɊԊu��������circle�𐶐�
        //for (int i = 0; i < clearCnt; i++)
        //{
        //    var pos = stageObject[i].transform.position;
        //    var nextPos = stageObject[i + 1].transform.position;
        //    var distance = pos - nextPos;

        //    var spaceX = Mathf.Abs(distance.x / Common.SS_CIRCLE_SPACE);
        //    var spaceY = Mathf.Abs(distance.y / Common.SS_CIRCLE_SPACE);

        //    if (spaceX > spaceY)
        //    {
        //        spaceY = Mathf.Abs(distance.y / spaceX);
        //        spaceX = Common.SS_CIRCLE_SPACE;

        //        if (distance.y == 0)
        //        {
        //            spaceY = Common.SS_CIRCLE_SPACE;
        //        }
        //    }
        //    else if (spaceX < spaceY)
        //    {
        //        spaceX = Mathf.Abs(distance.x / spaceY);
        //        spaceY = Common.SS_CIRCLE_SPACE;

        //        if (distance.x == 0)
        //        {
        //            spaceX = Common.SS_CIRCLE_SPACE;
        //        }
        //    }
        //    else
        //    {
        //        spaceX = Common.SS_CIRCLE_SPACE;
        //        spaceY = Common.SS_CIRCLE_SPACE;
        //    }

        //    int cnt = 0;
        //    var minDistance = Common.SS_CIRCLE_SPACE;
        //    while (Mathf.Abs(distance.x) > minDistance || Mathf.Abs(distance.y) > minDistance)
        //    {
        //        if (cnt > 100)
        //        {
        //            break;
        //        }

        //        if (distance.x > 0)
        //        {
        //            pos.x += -spaceX;
        //            distance.x += -spaceX;
        //        }
        //        else if (distance.x < 0)
        //        {
        //            pos.x += spaceX;
        //            distance.x += spaceX;
        //        }
        //        if (distance.y > 0)
        //        {
        //            pos.y += -spaceY;
        //            distance.y += -spaceY;
        //        }
        //        else if (distance.y < 0)
        //        {
        //            pos.y += spaceY;
        //            distance.y += spaceY;
        //        }

        //        Instantiate(circle, pos, Quaternion.identity);
        //        cnt++;
        //    }
        //}
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
                startPos = endPos;
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
            speed *= Common.SS_SLOW_DOWN_SPEED;
            //direction *= Common.SS_SLOW_DOWN_SPEED;

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
                //if (stageNameObject[stageRelease - 1].transform.position.x >= CenterUI.transform.position.x)
                //{
                //    stageParent.transform.position += new Vector3((direction * speed), 0, 0) * Time.deltaTime;
                //}
            }
        }

        // ��ԋ߂����S���W�̃C���f�b�N�X���擾
        stageSpaceIndex = CheckCenter();

        if (speed < 0.075f)
        {
            isMove = false;
            speed = 0;
        }

        // ���S���W�Ɉ�ԋ߂��I�u�W�F�N�g�̓����x���Ȃ��ɂ���
        stageNameObject[stageSpaceIndex].GetComponent<Image>().color = Common.MAX_ALPHA;
        stageNameObject[stageSpaceIndex].GetComponentInChildren<Text>().color = new Color(1f, 0.5f, 0, 1f);

        // ���S���W�Ɉ�ԋ߂��I�u�W�F�N�g�̃T�C�Y��ύX����
        var size = 1.25f;
        stageNameObject[stageSpaceIndex].GetComponent<RectTransform>().localScale = new Vector3(size, size, size);

        // �L�������X�e�[�W���ړ�����
        CharaMove();

        // �J�������X�e�[�W���ړ�����
        CameraMove();

        // �P�X�e�[�W�̂ݕ\������Ă����ꍇ
        //if (stageRelease == 1)
        //{
        //    print("���off");
        //    var tmp1 = arrowImages[0].GetComponent<Image>().color;
        //    tmp1.a = 0f;
        //    arrowImages[0].GetComponent<Image>().color = tmp1;

        //    var tmp2 = arrowImages[0].GetComponent<Image>().color;
        //    tmp2.a = 0f;
        //    arrowImages[1].GetComponent<Image>().color = tmp2;

        //    return;
        //}

        if (stageSpaceIndex == 0)
        {
            var tmp1 = arrowImages[0].GetComponent<Image>().color;
            tmp1.a = 0;
            arrowImages[0].GetComponent<Image>().color = tmp1;
            var tmp2 = arrowImages[1].GetComponent<Image>().color;
            tmp2.a = 0.5f;
            arrowImages[1].GetComponent<Image>().color = tmp2;//errorTexts[0].SetActive(false);
        }
        //else if (stageSpaceIndex == stageRelease - 1)
        //{
        //    var tmp1 = arrowImages[0].GetComponent<Image>().color;
        //    tmp1.a = 0.5f;
        //    arrowImages[0].GetComponent<Image>().color = tmp1;
        //    var tmp2 = arrowImages[1].GetComponent<Image>().color;
        //    tmp2.a = 0;
        //    arrowImages[1].GetComponent<Image>().color = tmp2;
        //}
        else
        {
            var tmp1 = arrowImages[0].GetComponent<Image>().color;
            tmp1.a = 0.5f;
            arrowImages[0].GetComponent<Image>().color = tmp1;

            var tmp2 = arrowImages[1].GetComponent<Image>().color;
            tmp2.a = 0.5f;
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
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
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
                sum = stageDirection;
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
        //SoundManager.smInstance.StopBGM();

        // SE���Đ�
        //AudioClip clip = LoadSE("se_battle_start");
        //audioSource.volume = 0.25f;
        //audioSource.PlayOneShot(clip);
    }
}
