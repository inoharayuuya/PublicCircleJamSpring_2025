using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StartCount : MonoBehaviour
{
    public RunPlayer runPlayer;
    public GameObject count = null;   // Text�I�u�W�F�N�g
    public float startCountdown = 3f; //�X�^�[�g�J�E���g�_�E���p
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeCount();
    }
    public void TimeCount()
    {
        Text start_text = count.GetComponent<Text>();       // �I�u�W�F�N�g����Text�R���|�[�l���g���擾
        int displayCount = Mathf.CeilToInt(startCountdown); // �����_�ȉ���؂�グ�Đ����ɂ���
        start_text.text = displayCount.ToString();          // �e�L�X�g�̕\��

        if (startCountdown >= 0)
        {
            startCountdown -= Time.deltaTime;               // 321�̃J�E���g�_�E��
        }
                           
        if (startCountdown <= 0)
        {
            start_text.text = "Start!";
        }
    } 
}
