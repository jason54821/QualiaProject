using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    // このカードの処理クラス
    private Card card;

    //// UIプレハブ
    //[SerializeField] private GameObject cardIconPrefab = null;
    //[SerializeField] private GameObject cardEffectTextPrefab = null;
    //// オブジェクト内UI参照
    //[SerializeField] private Image cardBackImage = null;
    [SerializeField] private Text cardNameText = null;
    //[SerializeField] private Transform cardIconParent = null;
    //[SerializeField] private Transform cardEffectTextParent = null;
    //[SerializeField] private Text cardForceText = null;
    //[SerializeField] private Image cardForceBackImage = null;
    //[SerializeField] private Text quantityText = null;
    //[SerializeField] private Image quantityBackImage = null;
    //[SerializeField] private GameObject hilightImageObject = null;
    //// 各種スプライト素材
    //[SerializeField] private Sprite cardBackSprite_Racha = null;
    //[SerializeField] private Sprite cardBackSprite_Haru = null;

    // 作成した効果Textリスト
    //private Dictionary<CardEffectDefine, Text> cardEffectTextDic;

    // 初期化関数(Card.csから呼出)
    public void Init(Card _card)
    {
        card = _card;
        //cardEffectTextDic = new Dictionary<CardEffectDefine, Text>();

        //UI初期化
        //quantityText.text = "";
        //quantityBackImage.color = Color.clear;
    } 

    ///// <summary>
    ///// プレイヤー側用・敵側用のカード拝啓画像をセット
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
    /// カード名表示演出
    /// </summary>
    public void SetCardNameText(string name_JP)
    {
        Debug.Log("SetCardNameText" + name_JP);
        cardNameText.text = name_JP;
    }

    ///// <summary>
    ///// カードアイコンUIを追加
    ///// </summary>
    //public void AddCardIconImage(Sprite sprite)
    //{
    //    var obj = Instantiate(cardIconPrefab, cardIconParent);
    //    obj.GetComponent<Image>().sprite = sprite;
    //}

    ///// <summary>
    ///// カード効果Textを追加
    ///// </summary>
    //public void AddCardEffectText( CardEffectDefine effectData)
    //{
    //    var obj = Instantiate(cardEffectTextPrefab, cardEffectTextParent);
    //    cardEffectTextDic.Add(effectData, obj.GetComponent<Text>());
    //    ApplyCardEffectText(effectData);
    //}

    ///// <summary>
    ///// カード効果Textの表示内容を更新
    ///// </summary>
    //public void ApplyCardEffectText(CardEffectDefine effectData)
    //{
    //    var targetText = cardEffectTextDic[effectData];
    //    int effectValue = effectData.value;
    //    string effectValueMes = "";

    //    effectValueMes = effectValue.ToString();
    //}


    ///// <summary>
    ///// カード強度Text表示
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
    ///// カード数量Text表示
    ///// </summary>
    //public void SetAmountText(int value)
    //{
    //    quantityText.text = "x" + value;
    //    quantityBackImage.color = Color.white;
    //}

    ///// <summary>
    ///// カード強調表示画像を表示・非表示にする
    ///// </summary>
    //public void SetHilightImage(bool mode)
    //{
    //    hilightImageObject.SetActive(mode);
    //}

    //public void ClearIconsAndEffects()
    //{
    //    // アイコン初期化
    //    int length = cardIconParent.childCount;
    //    for(int i = 0; i < length; i++)
    //    {
    //        Destroy(cardIconParent.GetChild(i).gameObject);
    //    }
    //    // 効果初期化
    //    length = cardEffectTextParent.childCount;
    //    for(int i = 0;i < length; i++)
    //    {
    //        Destroy(cardEffectTextParent.GetChild(i).gameObject);
    //    }
    //}

}
