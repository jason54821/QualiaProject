using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Linq;

/// <summary>
/// �J�[�h�����N���X
/// </summary>
public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // �I�u�W�F�N�g�E�R���|�[�l���g�Q��
    [SerializeField] private CardUI cardUI = null;
    private FieldManager fieldManager;
    public RectTransform rectTransform;

    //�J�[�h�f�[�^
    [HideInInspector] public CardDataSO baseCardData;
    [HideInInspector] public List<Sprite> iconSprites;
    [HideInInspector] public List<CardEffectDefine> effects;
    [HideInInspector] public int forcePoint;
    [HideInInspector] public int controllerCharaID;

    // �e��ϐ�
    private Vector2 basePos;
    private Tween moveTween;
    private Tween punchTween;
    public CardZone.ZoneType nowZone;
    public int nowLane;

    public bool isMoving = false;
    private GameObject trackingZone;

    //�@�L�����N�^�[ID�萔
    public const int CharaNum = 2;
    public const int CharaID_Racha = 0;
    public const int CharaID_Haru = 1;
    public const int CharaID_None = -1;
    //�@���̑��萔
    private const int MaxIcons = 6;
    private const int MaxEffects = 6;

    //����������
    public void Init(FieldManager _fieldManager, Vector2 deckPosition)
    {
        //�Q�Ǝ擾
        fieldManager = _fieldManager;

        //�z���R���|�[�l���g������
        cardUI.Init(this);

        //�ϐ�������
        rectTransform = GetComponent<RectTransform>();
        rectTransform.position = Vector3.zero;
        basePos = Vector2.zero;
        nowZone = CardZone.ZoneType.Hand;
        iconSprites = new List<Sprite>();
        effects = new List<CardEffectDefine>();
    }

    /// <summary>
    /// (�������ɌĂяo��)
    /// �R�[�h��`�f�[�^����e��p�����[�^���擾���ăZ�b�g����
    /// </summary>
    /// <param name="cardControllerCharaID">�g�p��ID</param>
    public void SetInitialCardData(CardDataSO cardData)
    {
        baseCardData = cardData;
        Debug.Log("SetInitialCardData" + cardData.cardName_JP);
        
        //�J�[�h��
        cardUI.SetCardNameText(cardData.cardName_JP);

        //�J�[�h�A�C�R��
        //AddCardIcon(cardData.iconSprite);
        
        //�J�[�h���ʃ��X�g
        //foreach(var item in cardData.effectList)
        //    AddCardEffect(item);

        // �J�[�h�g�p�҃f�[�^
        //controllerCharaID = cardControllerCharaID;
        //cardUI.SetCardBackSprite(cardControllerCharaID); // �J�[�h�w�iUI�ɓK�p
    }

    #region �I�u�W�F�N�g�ړ��E�\�����o

    void Update()
    {
        if (!isMoving && (trackingZone != null))
        {
           //rectTransform.position = trackingZone.transform.position;
        }

    }

    public IEnumerator TimeMoveCoroutine()
    {
        Debug.Log("Start TimeMoveCoroutine");

        // 3�̃��[���ɂ��ꂼ��7��playZone�������X�g
        List<List<GameObject>> allLanes = new()
        {
            fieldManager.lane1.ToList(),  // ���[��1
            fieldManager.lane2.ToList(),  // ���[��2
            fieldManager.lane3.ToList()   // ���[��3
        };

        // �e���[����playZone��7�ł��邱�Ƃ�O��Ƃ��܂�
        const int playZoneCount = 7;

        if (nowLane >= 0 && nowLane < allLanes.Count)
        {
            // �Ή����郌�[����playZone���擾
            var currentLane = allLanes[nowLane];

            // ���ʏ����Ƀ��[����playZone��n��
            yield return MoveCard(currentLane, playZoneCount, nowZone);
        }

        if (nowZone == CardZone.ZoneType.StartZone)
        {
            Debug.Log("���s");
            ExecuteStartZoneActions(); // StartZone�̏������֐���
        }

        // ���̏�����ҋ@
        yield return new WaitForSeconds(fieldManager.moveTime);
    }

    // ���[�����Ƃ̋��ʏ����֐�
    IEnumerator MoveCard(List<GameObject> lane, int zoneCount, CardZone.ZoneType nowZone)
    {
        int i = 0;
        foreach (GameObject obj in lane)
        {
            var objArea = obj.GetComponent<CardZone>();

            if (nowZone == CardZone.ZoneType.StartZone)
            {
                HandleStartZone(); // StartZone�̏������֐���
                break;
            }

            // ���݂̃]�[���ɑΉ�����I�u�W�F�N�g��������A���̃]�[�������݂���ꍇ
            if (nowZone == objArea.zoneType && i + 1 < zoneCount)
            {
                var nextZone = lane[i + 1].GetComponent<CardZone>();
                Debug.Log("now " + nowZone + " next " + nextZone);
                MoveToNextZone(nextZone); // �ړ��������֐���
                //nowZone = nextZone.zoneType;

                break;
            }

            i++;
        }

        yield return null;
    }

    void ExecuteStartZoneActions()
    {
        fieldManager.isAnimating = true;

        var startSequence = DOTween.Sequence();

        startSequence.AppendCallback(() =>
        {
            rectTransform.DOPunchScale(Vector3.one * 0.5f, 1f, 2, 0.4f);
        });

        startSequence.AppendInterval(1f);

        startSequence.AppendCallback(() =>
        {
            rectTransform.DOPunchPosition(Vector3.one * 4f, 2f, 5, 4f)
                .SetEase(Ease.InQuart);
        });

        startSequence.AppendInterval(2f);

        startSequence.AppendCallback(() =>
        {
            gameObject.transform.SetParent(fieldManager.trashCardsParent);
            gameObject.SetActive(false);
        });

        startSequence.OnComplete(() =>
        {
            fieldManager.isAnimating = false;
        });

        // �J�[�h���s�i�����E�G�j�̏����������Œǉ�
    }

    void MoveToNextZone(CardZone nextZone)
    {
        Debug.Log("MoveToNextZone " + nextZone);
        const float MoveTime = 0.4f; // �J�[�h�ړ��A�j���[�V��������
                                     // �w��n�_�܂ňړ�����A�j���[�V����(DOTween)

        fieldManager.isAnimating = true;

        moveTween = rectTransform
            .DOMove(nextZone.transform.position, MoveTime) // �ړ�Tween
            .SetEase(Ease.OutQuart) // �ω��̎d�����w��
            .OnComplete(() =>
            {
                fieldManager.isAnimating = false;
                // �A�j���[�V���������������^�C�~���O�Ń]�[�����X�V
                nowZone = nextZone.zoneType;
                Debug.Log("�]�[�����X�V����܂���: " + nowZone);
            });

        punchTween = rectTransform
            .DOPunchScale(Vector3.one * 0.2f, 1f, 2, 0.2f);
    }

    void HandleStartZone()
    {
        // StartZone�Ŏ��s�����������������ɏ����܂�
        Debug.Log("StartZone�̏���");
    }


    
    public void TrackingToZone(GameObject zone)
    {
        trackingZone = zone;
    }

    /// <summary>
    /// �J�[�h��z�u����
    /// </summary>
    public void PutToZone(CardZone.ZoneType targetZone, int targetLane, Vector2 targetPosition)
    {
        Debug.Log("1PutToZone: nowZone = " + nowZone + " -> " + targetZone);

        rectTransform.position = targetPosition;
        nowZone = targetZone;
        nowLane = targetLane;

        Debug.Log("1PutToZone Completed: nowZone = " + nowZone);
    }

    public void PutToZone(CardZone.ZoneType targetZone, Vector2 targetPosition)
    {
        Debug.Log("2PutToZone: nowZone = " + nowZone + " -> " + targetZone);

        rectTransform.position = targetPosition;
        nowZone = targetZone;

        Debug.Log("2PutToZone Completed: nowZone = " + nowZone);
    }

    /// <summary>
	/// ��{���W�܂ŃJ�[�h���ړ�������
	/// </summary>
    public void BackToBasePos()
    {
        const float MoveTime = 0.4f; // �J�[�h�ړ��A�j���[�V��������

        // ���Ɏ��s���̈ړ��A�j���[�V����������Β�~
        if (moveTween != null )
            moveTween.Kill();

        // �w��n�_�܂ňړ�����A�j���[�V����(DOTween)
        moveTween = rectTransform
            .DOMove(basePos, MoveTime) // �ړ�Tween
            .SetEase(Ease.OutQuart); // �ω��̎d�����w��
        
    }

    #endregion

    #region �p�����[�^�ύX�E�ǉ�����

    public void AddCardIcon (Sprite newIcon)
    {
        if (iconSprites.Count >= MaxIcons)
            return;

        iconSprites.Add(newIcon);
    }

    private void AddCardEffect(CardEffectDefine newEffect)
    {
        if (effects.Count >= MaxEffects)
            return;

        var effectData = new CardEffectDefine();
        effectData.cardEffect = newEffect.cardEffect;
        effectData.value = newEffect.value;

        effects.Add(effectData);

        // UI�\��
        //cardUI.AddCardEffectText(effectData);
    }

    #endregion

    #region �^�b�v�C�x���g����

    /// <summary>
    /// �^�b�v�J�n���Ɏ��s
    /// IPointerDownHandler���K�v
    /// </summary>
    /// /// <param name="eventData">�^�b�v���</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        fieldManager.StartDragging(this);
        basePos = rectTransform.position;
    }

    /// <summary>
	/// �^�b�v�I�����Ɏ��s
	/// IPointerUpHandler���K�v
	/// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        fieldManager.EndDragging();
    }

    #endregion
}
