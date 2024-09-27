using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    // ���̃J�[�h�̏����N���X
    private Card card;

    //// UI�v���n�u
    //[SerializeField] private GameObject cardIconPrefab = null;
    //[SerializeField] private GameObject cardEffectTextPrefab = null;
    //// �I�u�W�F�N�g��UI�Q��
    //[SerializeField] private Image cardBackImage = null;
    [SerializeField] private Text cardNameText = null;
    //[SerializeField] private Transform cardIconParent = null;
    //[SerializeField] private Transform cardEffectTextParent = null;
    //[SerializeField] private Text cardForceText = null;
    //[SerializeField] private Image cardForceBackImage = null;
    //[SerializeField] private Text quantityText = null;
    //[SerializeField] private Image quantityBackImage = null;
    //[SerializeField] private GameObject hilightImageObject = null;
    //// �e��X�v���C�g�f��
    //[SerializeField] private Sprite cardBackSprite_Racha = null;
    //[SerializeField] private Sprite cardBackSprite_Haru = null;

    // �쐬��������Text���X�g
    //private Dictionary<CardEffectDefine, Text> cardEffectTextDic;

    // �������֐�(Card.cs����ďo)
    public void Init(Card _card)
    {
        card = _card;
        //cardEffectTextDic = new Dictionary<CardEffectDefine, Text>();

        //UI������
        //quantityText.text = "";
        //quantityBackImage.color = Color.clear;
    } 

    ///// <summary>
    ///// �v���C���[���p�E�G���p�̃J�[�h�q�[�摜���Z�b�g
    ///// </summary>
    ///// <param name="cardControllerCharaID"></param>
    //public void SetCardBackSprite(int cardControllerCharaID)
    //{
    //    if (cardControllerCharaID == Card.CharaID_Racha)
    //        cardBackImage.sprite = cardBackSprite_Racha;
    //    else if (cardControllerCharaID == Card.CharaID_Haru)
    //        cardBackImage.sprite = cardBackSprite_Haru;
    //}

    /// <summary>
    /// �J�[�h���\�����o
    /// </summary>
    public void SetCardNameText(string name_JP)
    {
        Debug.Log("SetCardNameText" + name_JP);
        cardNameText.text = name_JP;
    }

    ///// <summary>
    ///// �J�[�h�A�C�R��UI��ǉ�
    ///// </summary>
    //public void AddCardIconImage(Sprite sprite)
    //{
    //    var obj = Instantiate(cardIconPrefab, cardIconParent);
    //    obj.GetComponent<Image>().sprite = sprite;
    //}

    ///// <summary>
    ///// �J�[�h����Text��ǉ�
    ///// </summary>
    //public void AddCardEffectText( CardEffectDefine effectData)
    //{
    //    var obj = Instantiate(cardEffectTextPrefab, cardEffectTextParent);
    //    cardEffectTextDic.Add(effectData, obj.GetComponent<Text>());
    //    ApplyCardEffectText(effectData);
    //}

    ///// <summary>
    ///// �J�[�h����Text�̕\�����e���X�V
    ///// </summary>
    //public void ApplyCardEffectText(CardEffectDefine effectData)
    //{
    //    var targetText = cardEffectTextDic[effectData];
    //    int effectValue = effectData.value;
    //    string effectValueMes = "";

    //    effectValueMes = effectValue.ToString();
    //}


    ///// <summary>
    ///// �J�[�h���xText�\��
    ///// </summary>
    //public void SetForcePointText(int value)
    //{
    //    if (value > 0)
    //    {
    //        cardForceText.text = value.ToString();
    //        cardForceBackImage.color = Color.white;
    //    }
    //    else
    //    {
    //        cardForceText.text = "";
    //        cardForceBackImage.color = Color.clear;
    //    }
    //}

    ///// <summary>
    ///// �J�[�h����Text�\��
    ///// </summary>
    //public void SetAmountText(int value)
    //{
    //    quantityText.text = "x" + value;
    //    quantityBackImage.color = Color.white;
    //}

    ///// <summary>
    ///// �J�[�h�����\���摜��\���E��\���ɂ���
    ///// </summary>
    //public void SetHilightImage(bool mode)
    //{
    //    hilightImageObject.SetActive(mode);
    //}

    //public void ClearIconsAndEffects()
    //{
    //    // �A�C�R��������
    //    int length = cardIconParent.childCount;
    //    for(int i = 0; i < length; i++)
    //    {
    //        Destroy(cardIconParent.GetChild(i).gameObject);
    //    }
    //    // ���ʏ�����
    //    length = cardEffectTextParent.childCount;
    //    for(int i = 0;i < length; i++)
    //    {
    //        Destroy(cardEffectTextParent.GetChild(i).gameObject);
    //    }
    //}

}
