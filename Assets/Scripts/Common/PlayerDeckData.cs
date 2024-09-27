using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckData : MonoBehaviour
{
    //�e��ݒ�f�[�^(Inspector����Z�b�g)
    public List<CardDataSO> allPlayerCardsList; // �v���C���[���J�[�h�S���̃��X�g
    public List<CardDataSO> playerInitialDesk; // �v���C���[�������f�b�L�J�[�h���X�g

    //�v���C���[���S�J�[�h�f�[�^�ƒʂ��ԍ���R�Â���Dictionary
    public static Dictionary<int, CardDataSO> CardDatasBySerialNum;

    //���݂̃v���C���[�f�b�L�f�[�^(�ʂ��ԍ��ŊǗ�)
    public static List<int> deckCardList;

    //����������
    public void Init()
    {
        //�v���C���[���S�J�[�h�f�[�^�ƒʂ��ԍ���R�Â���
        CardDatasBySerialNum = new Dictionary<int, CardDataSO>();
        foreach(var item in allPlayerCardsList)
        {
            CardDatasBySerialNum.Add(item.serialNum, item);
        }
    }


    /// <summary>
    /// �Q�[������N�����̏����f�b�L��ݒ肷��
    /// </summary>
    public void DataInitialize()
    {
        //�v���C���[�̌��݃f�b�L�f�[�^�ɏ����f�b�L�ݒ�𔽉f
        deckCardList = new List<int>();
        foreach(var cardData in playerInitialDesk)
        {
            AddCardToDeck(cardData.serialNum);
        }
    }

    /// <summary>
    /// �f�b�L�ɃJ�[�h��1���ǉ�����
    /// </summary>
    /// <param name="cardSerialNum">�J�[�h�̒ʂ��ԍ�</param>
    public static void AddCardToDeck(int cardSerialNum)
    {
        deckCardList.Add(cardSerialNum);
        deckCardList.Sort();
    }
}
