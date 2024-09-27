using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �_�~�[��D����N���X
/// </summary>
public class DammyHandUI : MonoBehaviour
{
    //�@�_�~�[��D����pHorizontalLayoutGroup
    [SerializeField] private HorizontalLayoutGroup layoutGroup = null;
    //�@�_�~�[��D�v���n�u
    [SerializeField] private GameObject dammyHandPrefab = null;

    // ���������_�~�[��D�̃��X�g
    private List<Transform> dammyHandList;

    /// <summary>
    /// �w��̖����ɂȂ�悤�_�~�[��D���쐬�܂��͍폜����
    /// </summary>
    /// <param name="value">�ݒ薇��</param>
    public void SetHandNum(int value)
    {
        if(dammyHandList == null)
        {//�@������s��
            //�@���X�g�V�K�쐬
            dammyHandList = new List<Transform>();
            AddHandObj(value);
        }
        else
        {
            //�@���݂���ω����閇�����v�Z
            int differenceNum = value - dammyHandList.Count;
            //�@�_�~�[��D�쐬�E�폜
            if (differenceNum > 0)
                AddHandObj(differenceNum);
            else if (differenceNum < 0)
                RemoveHandObj(differenceNum);
            
        }
    }

    /// <summary>
    /// �_�~�[��D���w�薇���ǉ�����
    /// </summary>
    public void AddHandObj(int value) {
        //�@�ǉ��������I�u�W�F�N�g�쐬
        for (int i = 0; i < value; i++)
        {
            //�@�I�u�W�F�N�g�쐬
            var obj = Instantiate(dammyHandPrefab, transform);
            //�@���X�g�ɒǉ�
            dammyHandList.Add(obj.transform);
        }
    }

    /// <summary>
    /// �_�~�[��D���w�薇���폜����
    /// </summary>
    public void RemoveHandObj(int value)
    {
        value = Mathf.Abs(value);
        for(int i = 0; i < value; i++)
        {
            if (dammyHandList.Count <= 0)
                break;

            //�@�I�u�W�F�N�g�폜
            Destroy(dammyHandList[0].gameObject);
            //�@���X�g�폜
            dammyHandList.RemoveAt(0);
        }
    }

    /// <summary>
    /// �Y���ԍ��̃_�~�[��D�̍��W��Ԃ�
    /// </summary>
    public Vector2 GetHandPos(int index)
    {
        //�@�G���[���
        if(index < 0 || index >= dammyHandList.Count)
        {
            return Vector2.zero;
        }
        // Debug
        Debug.Log("GetHandPos" + dammyHandList[index].position);
        // �_�~�[��D�̍��W��Ԃ�
        return dammyHandList[index].position;
    }

    /// <summary>
    /// ���C�A�E�g�̎�������@�\�𑦍��ɓK�p����
    /// </summary>
    public void ApplyLayout()
    {
        layoutGroup.CalculateLayoutInputHorizontal();
        layoutGroup.SetLayoutHorizontal();
    }
}
