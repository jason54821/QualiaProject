using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private BattleManager battleManager;
    private FieldManager fieldManager;
    public enum TurnState { PlayerTurn, EnemyTurn, Action, Waiting }
    public TurnState currentTurn;

    public void Init(BattleManager _battleManager, FieldManager _fieldManager)
    {
        battleManager = _battleManager;
        fieldManager = _fieldManager;
        
        battleManager.turnStart.onClick.AddListener(StartAction);
        // �o�g���J�n���Ƀv���C���[�̃^�[������n�߂�
        currentTurn = TurnState.PlayerTurn;
        StartPlayerTurn();
    }

    void Update()
    {
        // �^�[���̏�Ԃɉ������������s��
        switch (currentTurn)
        {
            case TurnState.PlayerTurn:
                // �v���C���[�̍s�����Ǘ�

                //�z�u������R�X�g����������

                //���W�J�֘A

                break;

            case TurnState.Action:
                // ����
                break;

            case TurnState.EnemyTurn:
                // �G�̍s�����Ǘ�
                break;

            case TurnState.Waiting:
                // ���̃^�[���ւ̏���
                break;
        }
    }

    public void StartBattle()
    {
        Debug.Log("�o�g���J�n - �X�e�[�^�X������");

        // �v���C���[�L�����N�^�[�̃X�e�[�^�X��������
        foreach (var character in battleManager.playerCharacters)
        {
            character.InitStatus();  // �e�L�����N�^�[�ɏ������������s��
        }

        StartPlayerTurn();
    }

    // �v���C���[�̃^�[���J�n���̏���
    public void StartPlayerTurn()
    {
        Debug.Log("�v���C���[�̃^�[���ł�");
        currentTurn = TurnState.PlayerTurn;
        // �v���C���[���J�[�h���o����悤�ɂ���Ȃǂ̏���

        battleManager.turnStart.interactable = true;

        //StartZone�ɃJ�[�h�ł���悤�ɂ���

        //���W�J����Ƃ��ɒǉ��Ŕz�u�ł��鏈��
    }

    public void StartAction()
    {
        Debug.Log("�A�N�V��������������^�[���ł�");
        currentTurn = TurnState.Action;

        // �{�^���𖳌������Ă���
        battleManager.turnStart.interactable = false;

        // �v���C���[���z�u�����J�[�h�ɂ���čs������������
        StartCoroutine(ProcessAllCardActions());
    }

    private IEnumerator ProcessAllCardActions()
    {
        // �S�ẴJ�[�h�̏�������������܂őҋ@����
        List<Coroutine> coroutines = new List<Coroutine>();

        foreach (Card card in fieldManager.cardInstances)
        {
            Debug.Log("card");
            // �R���[�`�����J�n���A���X�g�ɒǉ�
            coroutines.Add(StartCoroutine(card.TimeMoveCoroutine()));
        }

        // �S�ẴR���[�`������������܂őҋ@
        foreach (Coroutine coroutine in coroutines)
        {
            yield return coroutine;  // �e�R���[�`�����I������܂ő҂�
        }

        Debug.Log("�S�ẴJ�[�h�������������܂���");

        // �S�Ă̏���������������{�^����L���ɂ���
        battleManager.turnStart.interactable = true;

        // �^�[�����I��
        EndTurn();
    }

    // �G�̃^�[���J�n���̏���
    public void StartEnemyTurn()
    {
        Debug.Log("�G�̃^�[���ł�");
        currentTurn = TurnState.EnemyTurn;
        // �G��AI�s�������s����Ȃǂ̏���
        EndTurn();
    }

    // �^�[���̐؂�ւ�����
    public void EndTurn()
    {
        if (currentTurn == TurnState.PlayerTurn)
        {
            battleManager.turnStart.interactable = true;
        }
        else if (currentTurn == TurnState.Action)
        {
            currentTurn = TurnState.Waiting;
            StartEnemyTurn();
        }
        else if (currentTurn == TurnState.EnemyTurn)
        {
            currentTurn = TurnState.Waiting;
            StartPlayerTurn();
        }
    }
}
