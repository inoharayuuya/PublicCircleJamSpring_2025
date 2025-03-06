using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType
    {
        [Tooltip("�Œ�_���[�W")] Fix,        // �Œ�_���[�W
        [Tooltip("�����_���[�W")] Duration,   // �����_���[�W
        [Tooltip("��")]         Heal,       // ��
    }

    [Tooltip("�������邩�ۂ�")] public bool isObstacleDisabled;    // �����邩�ǂ���

    [Tooltip("���ʗ�")]         public float amount;               // ���ʗ�

    [Tooltip("��Q���̃^�C�v")] public ObstacleType obstacleType;  // ��Q���̃^�C�v

    public bool isFirst;


    private void Start()
    {
        Init();
    }

    void Init()
    {
        isFirst = false;
    }



    /// <summary>
    /// ���ʔ���
    /// </summary>
    public float Amount(float healthPoint)
    {
        switch (obstacleType)
        {
            case ObstacleType.Fix:
                if (isFirst)
                {
                    isFirst = false;
                    // �_���[�W����
                    print("�Œ�_���[�W");
                    healthPoint -= amount;

                }
                break;
            case ObstacleType.Duration:
                if (isFirst)
                {
                    // ��b�^�C�}�[
                    StartCoroutine(CountDown());

                    // �_���[�W����
                    print("�p���_���[�W");
                    healthPoint -= amount;
                }
                break;
            case ObstacleType.Heal:
                if (isFirst)
                {
                    isFirst = false;
                    // �񕜏���
                    print("��");
                    healthPoint += amount;
                }
                break;
        }


        print($"isFirst:{isFirst}");
        return healthPoint;
    }

    
    IEnumerator CountDown()
    {
        isFirst = false;

        yield return new WaitForSeconds(1);
        isFirst = true;
        print("1�b�o��");
    }


}
