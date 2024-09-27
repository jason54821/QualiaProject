using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    #region �V���O���g���ێ��p����(�ύX�s�v)

    // �V���O���g���ێ��p
    public static Data instance;

    //Awake(Start���O��1�x�������s
    private void Awake()
    {
        //�V���O���g���p����
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // �Q�[���N��������
        InitialProcess();
    }
    #endregion

    // �e��R���|�[�l���g
    public PlayerDeckData playerDeckData;

    private void InitialProcess()
    {

    }
}
