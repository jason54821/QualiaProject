using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Linq;

/// <summary>
/// カード処理クラス
/// </summary>
public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // オブジェクト・コンポーネント参照
    [SerializeField] private CardUI cardUI = null;
    private FieldManager fieldManager;
    public RectTransform rectTransform;

    //カードデータ
    [HideInInspector] public CardDataSO baseCardData;
    [HideInInspector] public List<Sprite> iconSprites;
    [HideInInspector] public List<CardEffectDefine> effects;
    [HideInInspector] public int forcePoint;
    [HideInInspector] public int controllerCharaID;

    // 各種変数
    private Vector2 basePos;
    private Tween moveTween;
    private Tween punchTween;
    public CardZone.ZoneType nowZone;
    public int nowLane;

    public bool isMoving = false;
    private GameObject trackingZone;

    //　キャラクターID定数
    public const int CharaNum = 2;
    public const int CharaID_Racha = 0;
    public const int CharaID_Haru = 1;
    public const int CharaID_None = -1;
    //　その他定数
    private const int MaxIcons = 6;
    private const int MaxEffects = 6;

    //初期化処理
    public void Init(FieldManager _fieldManager, Vector2 deckPosition)
    {
        //参照取得
        fieldManager = _fieldManager;

        //配下コンポーネント初期化
        cardUI.Init(this);

        //変数初期化
        rectTransform = GetComponent<RectTransform>();
        rectTransform.position = Vector3.zero;
        basePos = Vector2.zero;
        nowZone = CardZone.ZoneType.Hand;
        iconSprites = new List<Sprite>();
        effects = new List<CardEffectDefine>();
    }

    /// <summary>
    /// (初期化に呼び出し)
    /// コード定義データから各種パラメータを取得してセットする
    /// </summary>
    /// <param name="cardControllerCharaID">使用者ID</param>
    public void SetInitialCardData(CardDataSO cardData)
    {
        baseCardData = cardData;
        Debug.Log("SetInitialCardData" + cardData.cardName_JP);
        
        //カード名
        cardUI.SetCardNameText(cardData.cardName_JP);

        //カードアイコン
        //AddCardIcon(cardData.iconSprite);
        
        //カード効果リスト
        //foreach(var item in cardData.effectList)
        //    AddCardEffect(item);

        // カード使用者データ
        //controllerCharaID = cardControllerCharaID;
        //cardUI.SetCardBackSprite(cardControllerCharaID); // カード背景UIに適用
    }

    #region オブジェクト移動・表示演出

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

        // 3つのレーンにそれぞれ7つのplayZoneを持つリスト
        List<List<GameObject>> allLanes = new()
        {
            fieldManager.lane1.ToList(),  // レーン1
            fieldManager.lane2.ToList(),  // レーン2
            fieldManager.lane3.ToList()   // レーン3
        };

        // 各レーンのplayZoneが7つであることを前提とします
        const int playZoneCount = 7;

        if (nowLane >= 0 && nowLane < allLanes.Count)
        {
            // 対応するレーンのplayZoneを取得
            var currentLane = allLanes[nowLane];

            // 共通処理にレーンのplayZoneを渡す
            yield return MoveCard(currentLane, playZoneCount, nowZone);
        }

        if (nowZone == CardZone.ZoneType.StartZone)
        {
            Debug.Log("実行");
            ExecuteStartZoneActions(); // StartZoneの処理を関数化
        }

        // 次の処理を待機
        yield return new WaitForSeconds(fieldManager.moveTime);
    }

    // レーンごとの共通処理関数
    IEnumerator MoveCard(List<GameObject> lane, int zoneCount, CardZone.ZoneType nowZone)
    {
        int i = 0;
        foreach (GameObject obj in lane)
        {
            var objArea = obj.GetComponent<CardZone>();

            if (nowZone == CardZone.ZoneType.StartZone)
            {
                HandleStartZone(); // StartZoneの処理を関数化
                break;
            }

            // 現在のゾーンに対応するオブジェクトが見つかり、次のゾーンが存在する場合
            if (nowZone == objArea.zoneType && i + 1 < zoneCount)
            {
                var nextZone = lane[i + 1].GetComponent<CardZone>();
                Debug.Log("now " + nowZone + " next " + nextZone);
                MoveToNextZone(nextZone); // 移動処理を関数化
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

        // カード実行（味方・敵）の処理をここで追加
    }

    void MoveToNextZone(CardZone nextZone)
    {
        Debug.Log("MoveToNextZone " + nextZone);
        const float MoveTime = 0.4f; // カード移動アニメーション時間
                                     // 指定地点まで移動するアニメーション(DOTween)

        fieldManager.isAnimating = true;

        moveTween = rectTransform
            .DOMove(nextZone.transform.position, MoveTime) // 移動Tween
            .SetEase(Ease.OutQuart) // 変化の仕方を指定
            .OnComplete(() =>
            {
                fieldManager.isAnimating = false;
                // アニメーションが完了したタイミングでゾーンを更新
                nowZone = nextZone.zoneType;
                Debug.Log("ゾーンが更新されました: " + nowZone);
            });

        punchTween = rectTransform
            .DOPunchScale(Vector3.one * 0.2f, 1f, 2, 0.2f);
    }

    void HandleStartZone()
    {
        // StartZoneで実行したい処理をここに書きます
        Debug.Log("StartZoneの処理");
    }


    
    public void TrackingToZone(GameObject zone)
    {
        trackingZone = zone;
    }

    /// <summary>
    /// カードを配置する
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
	/// 基本座標までカードを移動させる
	/// </summary>
    public void BackToBasePos()
    {
        const float MoveTime = 0.4f; // カード移動アニメーション時間

        // 既に実行中の移動アニメーションがあれば停止
        if (moveTween != null )
            moveTween.Kill();

        // 指定地点まで移動するアニメーション(DOTween)
        moveTween = rectTransform
            .DOMove(basePos, MoveTime) // 移動Tween
            .SetEase(Ease.OutQuart); // 変化の仕方を指定
        
    }

    #endregion

    #region パラメータ変更・追加処理

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

        // UI表示
        //cardUI.AddCardEffectText(effectData);
    }

    #endregion

    #region タップイベント処理

    /// <summary>
    /// タップ開始時に実行
    /// IPointerDownHandlerが必要
    /// </summary>
    /// /// <param name="eventData">タップ情報</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        fieldManager.StartDragging(this);
        basePos = rectTransform.position;
    }

    /// <summary>
	/// タップ終了時に実行
	/// IPointerUpHandlerが必要
	/// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        fieldManager.EndDragging();
    }

    #endregion
}
