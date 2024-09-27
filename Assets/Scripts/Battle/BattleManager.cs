using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �J�[�h�o�g�����Ǘ�����}�l�[�W���[�N���X
/// </summary>
public class BattleManager : MonoBehaviour
{
    //�I�u�W�F�N�g�E�R���|�[�l���g�Q��
    [SerializeField]
    private FieldManager fieldManager;
    [SerializeField]
    private TurnManager turnManager;

    //UI�Q��
    [SerializeField] public Button turnStart;

    //�v���C���[�L�����N�^�[�Q��
    [SerializeField] public CharacterDataSO[] playerCharacters;

    //�G�L�����N�^�[�Q��
    //[SerializeField] public Enemy[] enemies;

    public CardDataSO testCardData;

    //�����������E���ׂĂ̏���������������X�^�[�g
    void Start()
    {
        fieldManager.Init(this);
        turnManager.Init(this, fieldManager);

        Debug.Log("BattleManager.cs : ����������");

        // �J�[�h���ʖ��\���e�X�g
        foreach (var cardEffect in testCardData.effectList)
        {
            // ���ʖ���������擾
            string nameText = CardEffectDefine.Dic_EffectName_JP[cardEffect.cardEffect];
            // ���ʒl�ϐ��𕶎���ɖ��ߍ���
            nameText = string.Format(nameText, cardEffect.value);
           
            Debug.Log(nameText);
        }

    }

}
