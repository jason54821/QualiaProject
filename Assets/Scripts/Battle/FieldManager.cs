using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �J�[�h�z�u����G���A���Ǘ�����}�l�[�W���[�N���X
/// </summary>
public class FieldManager : MonoBehaviour
{
    //�I�u�W�F�N�g�E�R���|�[�l���g
    private BattleManager battleManager;                     // �o�g����ʃ}�l�[�W���[
    public RectTransform canvasRectTransform;                // Canvas��RectTransform
    public Camera mainCamera;                                // ���C���J����
    [SerializeField] private DammyHandUI dammyHandUI = null; // �_�~�[��D����N���X

    //�v���C�G���A�Q��
    [SerializeField] 
    public GameObject[] lane1 = null;
    [SerializeField]
    public GameObject[] lane2 = null;
    [SerializeField]
    public GameObject[] lane3 = null;

    //�J�[�h�֘A�Q��
    [SerializeField] private GameObject cardPrefab = null; // �J�[�h�v���n�u
    [SerializeField] private Transform cardsParent = null; // ��������J�[�h�I�u�W�F�N�g�̐eTransform
    [SerializeField] private Transform deckIconTrs = null; // �f�b�L�I�u�W�F�N�gTransform

    [SerializeField] public Transform trashCardsParent = null;
    
    //�e��ϐ��E�Q��
    private float draggingCardPosZ;
    //public Card card;

    public Card draggingCard; // �h���b�O���쒆�J�[�h
    public List<Card> cardInstances; // ���������v���C���[����J�[�h���X�g
    private bool reserveHandAlign; // ��D����t���O
    private bool isDrawing; // true:��D��[���ł���

    public float moveTime = 5.0f;
    //public bool moveTrigger = false;

    public bool turnTrigger = false;

    public bool isAnimating = false;

    // �\���e�X�g�p�F�J�[�h�f�[�^
    [SerializeField] private CardDataSO testCardData;

    // ����������
    public void Init(BattleManager _battleManager)
    {
        // �Q�Ǝ擾
        battleManager = _battleManager;
        //card.Init(this);

        // �ϐ�������
        cardInstances = new List<Card>();

        // �f�o�b�O�p�h���[�����i�x�����s�j
        DOVirtual.DelayedCall(
            1.0f, // 1.0�b�x��
            () => 
            { 
                DrawCardsUntilNum(5); 
            }
        );
    }

    //Update
    void Update()
    {
        if(draggingCard != null){
            UpdateDragging();
        }
    }

    // OnGUI(Update�̂悤�ɌJ��Ԃ����s�EUI����p)
    private void OnGUI()
    {
        //��D����t���O�������Ă���Ȃ琮��
        if (reserveHandAlign)
        {
            AlignHandCards();
            reserveHandAlign = false;
        }
    }

    #region �v���C���[����D�E�f�b�L����
    /// <summary>
    /// �f�b�L����J�[�h���P��������D�ɉ�����
    /// </summary>
    /// <param name="handID">�Ώێ�D�ԍ�</param>
    private void DrawCard(int handID)
    {
        // �I�u�W�F�N�g�쐬
        var obj = Instantiate(cardPrefab, cardsParent);
        // �J�[�h�����N���X���擾�E���X�g�Ɋi�[
        Card objCard = obj.GetComponent<Card>();
        cardInstances.Add(objCard);

        // �J�[�h�����ݒ�
        objCard.Init(this, deckIconTrs.position);
        objCard.PutToZone(CardZone.ZoneType.Hand, dammyHandUI.GetHandPos(handID));
        objCard.SetInitialCardData(testCardData);
    }

    /// <summary>
    /// ��D���w�薇���ɂȂ�܂ŃJ�[�h������
    /// </summary>
    /// <param name="num">�w�薇��</param>
    private void DrawCardsUntilNum(int num)
    {
        // ���݂̎�D�������擾
        int nowHandNum = 0;
        foreach(var card in cardInstances)
        {
            if(card.nowZone == CardZone.ZoneType.Hand)
                nowHandNum++;
        }
        // �V���Ɉ����ׂ��������擾
        int drawNum = num - nowHandNum;
        if (drawNum <= 0)
            return;

        // ��DUI�ɖ������w��
        dammyHandUI.SetHandNum(nowHandNum + drawNum);

        // �A���ŃJ�[�h������(Sequence)
        const float DrawIntervalTime = 0.1f; //�h���[�Ԃ̎��ԊԊu
        var drawSequence = DOTween.Sequence();
        isDrawing = true;
        for (int i = 0; i < drawNum; i++)
        {
            // 1����������
            drawSequence.AppendCallback(() =>
            {
                DrawCard(nowHandNum);
                nowHandNum++;
            });
            // ���ԊԊu��ݒ�
            drawSequence.AppendInterval(DrawIntervalTime);
        }
        drawSequence.OnComplete(() => isDrawing = false);
    }

    /// <summary>
    /// ��D�̃J�[�h�𐮗񂳂���
    /// </summary>
    private void AlignHandCards()
    {
        // ��D��������
        int index = 0; // ��D���ԍ�
        // �_�~�[��D�𐮗�
        dammyHandUI.ApplyLayout();
        // �e�J�[�h���_�~�[��D�ɍ��킹�Ĉړ�
        foreach(var card in cardInstances)
        {
            if(card.nowZone == CardZone.ZoneType.Hand)
            {
                card.PutToZone(CardZone.ZoneType.Hand, dammyHandUI.GetHandPos(index));
                index++;
            }
        }
    }

    /// <summary>
    /// ���݂̎�D�̖�������DUI�����N���X�ɔ��f�����Đ��񂷂�
    /// </summary>
    private void CheckHandCardsNum()
    {
        //�@���݂̎�D�������擾
        int nowHandNum = 0;
        foreach(var item in cardInstances)
        {
            if (item.nowZone == CardZone.ZoneType.Hand)
                nowHandNum++;
        }
        //�@�_�~�[��D�ɖ������w��
        dammyHandUI.SetHandNum(nowHandNum);
        // ��D�����ɍ��킹�Ď�D���w��
        //�i��D������ύX�������t���[���ł̓_�~�[��D�I�u�W�F�N�g�������Ă��Ȃ����߈�u�����x�����s�j
        reserveHandAlign = true;


    }
    #endregion

    #region �J�[�h�h���b�O����
    /// <summary>
    /// �J�[�h�̃h���b�O������J�n����
    /// </summary>
    /// <param name="dragCard">����ΏۃJ�[�h</param>
    public void StartDragging(Card dragCard)
    {
        // ��D��[���o���Ȃ�I��
        if (isDrawing)
            return;

        // ����ΏۃJ�[�h���L��
        draggingCard = dragCard;

        draggingCard.isMoving = true;
        // ���̃J�[�h�I�u�W�F�N�g���Z��Ԃň�Ԍ��ɂ���(�őO�ʕ\���ɂ���)
        draggingCard.transform.SetAsLastSibling();

        draggingCardPosZ = draggingCard.transform.position.z;
        draggingCard.rectTransform = draggingCard.GetComponent<RectTransform>();
    }

    
   

    /// <summary>
    /// �h���b�O����X�V����
    /// </summary>
    public void UpdateDragging()
    {
        // �^�b�v�ʒu���擾
        Vector2 tapPos = Input.mousePosition;
        // �^�b�v�ʒu��ϊ�����i�X�N���[�����W->Canvas�̃��[�J�����W�j
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            tapPos,
            mainCamera,
            out tapPos);

        // ���W��K�p
        draggingCard.rectTransform.anchoredPosition = tapPos;


        //�h���b�N���̃J�[�h������Ώ����s��
        //if (draggingCard != null)
        //{
        //    Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //    mousePos.z = draggingCardPosZ;
        //    //mousePos.z = 0;
        //    draggingCard.transform.position = mousePos;
        //}
    } 
    
    /// <summary>
    /// �J�[�h�̃h���b�O������I������
    /// </summary>
    public void EndDragging()
    {
        // �d�Ȃ��Ă���I�u�W�F�N�g�̏������ׂĎ擾����
        // (���肪�K�v�ȃI�u�W�F�N�g�ɂ͂��ׂ�BoxCollider2D���t�^����Ă���̂ł���𗘗p���Ĕ���j
        // ���̃I�u�W�F�N�g�̃X�N���[�����W���擾����
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(mainCamera, draggingCard.transform.position);
        // ���C���J���������L�Ŏ擾�������W�Ɍ�����Ray���΂�
        Ray ray = mainCamera.ScreenPointToRay(pos);

        // �h���b�O��̃I�u�W�F�N�g�擾����
        CardZone targetZone = null;
        Card targetCard = null;
        GameObject targetZoneObj = null;
        //Ray�����������S�I�u�W�F�N�g�ɑ΂��Ă̏���
        foreach(RaycastHit2D hit in Physics2D.RaycastAll(ray.origin, ray.direction, 100.0f))
        {
            // ���������I�u�W�F�N�g�����݂��Ȃ��Ȃ�I��
            if (!hit.collider)
            {
                //debug
                Debug.Log("�I�u�W�F�N�g���݂��Ȃ�");
                break;
            }

            // ���������I�u�W�F�N�g���h���b�O���̃J�[�h�Ɠ���Ȃ玟��
            var hitObj = hit.collider.gameObject;
            if (hitObj == draggingCard.gameObject)
            {
                Debug.Log("����J�[�h");
                continue;
            }

            // �I�u�W�F�N�g���J�[�h�G���A�Ȃ�擾���Ď���
            var hitArea = hitObj.GetComponent<CardZone>();
            if(hitArea != null)
            {
                Debug.Log("�J�[�h�G���A" + hitArea);
                targetZone = hitArea;
                
                targetZoneObj = hitObj;
                continue;
            }

            // �I�u�W�F�N�g���J�[�h�Ȃ�擾���Ď���
            var hitCard = hitObj.GetComponent<Card>();
            if(hitCard != null)
            {
                Debug.Log("�J�[�h" + hitCard);
                targetCard = hitCard;
                continue;
            }
        }
        
        // �d�Ȃ����Ώۂ��Ƃɂ�鏈��
        //if(targetCard != null &&
        //    (targetCard.nowZone >= CardZone.ZoneType.PlayBoard0 && targetCard.nowZone <= CardZone.ZoneType.PlayBoard4))
        //{// �v���C�{�[�h�ɂ���J�[�h�Əd�Ȃ����ꍇ
        //    // ��������(������)
        //}
        if(targetZone != null)
        {// �J�[�h�Əd�Ȃ炸�J�[�h�G���A�Əd�Ȃ����ꍇ
            // �ݒu����
            draggingCard.PutToZone(targetZone.zoneType, targetZone.zoneNum,targetZone.GetComponent<RectTransform>().position);
            draggingCard.TrackingToZone(targetZoneObj);
            CheckHandCardsNum();
            // ��D�ȊO->��D�ւ̈ړ��̏ꍇ�A�J�[�h�����X�g���ň�Ԍ��ɂ���
            if(draggingCard.nowZone == CardZone.ZoneType.Hand)
            {
                cardInstances.Remove(draggingCard);
                cardInstances.Add(draggingCard);
            }
        }
        else
        {// ������Ƃ��d�Ȃ�Ȃ������ꍇ
            // ���̈ʒu�ɖ߂�
            Debug.Log("���̈ʒu��");
            draggingCard.BackToBasePos();
        }

        draggingCard.isMoving = false;
        draggingCard = null;
        

        //Debug.Log("�h���b�O�����I��");
        //draggingCard.BackToBasePos();
        //Debug.Log("���̏ꏊ�ɖ߂鏈��");

        //draggingCard = null;
    }
    #endregion

}
