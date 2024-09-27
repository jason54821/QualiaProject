using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// カード配置するエリアを管理するマネージャークラス
/// </summary>
public class FieldManager : MonoBehaviour
{
    //オブジェクト・コンポーネント
    private BattleManager battleManager;                     // バトル画面マネージャー
    public RectTransform canvasRectTransform;                // CanvasのRectTransform
    public Camera mainCamera;                                // メインカメラ
    [SerializeField] private DammyHandUI dammyHandUI = null; // ダミー手札制御クラス

    //プレイエリア参照
    [SerializeField] 
    public GameObject[] lane1 = null;
    [SerializeField]
    public GameObject[] lane2 = null;
    [SerializeField]
    public GameObject[] lane3 = null;

    //カード関連参照
    [SerializeField] private GameObject cardPrefab = null; // カードプレハブ
    [SerializeField] private Transform cardsParent = null; // 生成するカードオブジェクトの親Transform
    [SerializeField] private Transform deckIconTrs = null; // デッキオブジェクトTransform

    [SerializeField] public Transform trashCardsParent = null;
    
    //各種変数・参照
    private float draggingCardPosZ;
    //public Card card;

    public Card draggingCard; // ドラッグ操作中カード
    public List<Card> cardInstances; // 生成したプレイヤー操作カードリスト
    private bool reserveHandAlign; // 手札整列フラグ
    private bool isDrawing; // true:手札補充中である

    public float moveTime = 5.0f;
    //public bool moveTrigger = false;

    public bool turnTrigger = false;

    public bool isAnimating = false;

    // 表示テスト用：カードデータ
    [SerializeField] private CardDataSO testCardData;

    // 初期化処理
    public void Init(BattleManager _battleManager)
    {
        // 参照取得
        battleManager = _battleManager;
        //card.Init(this);

        // 変数初期化
        cardInstances = new List<Card>();

        // デバッグ用ドロー処理（遅延実行）
        DOVirtual.DelayedCall(
            1.0f, // 1.0秒遅延
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

    // OnGUI(Updateのように繰り返し実行・UI制御用)
    private void OnGUI()
    {
        //手札整列フラグが立っているなら整列
        if (reserveHandAlign)
        {
            AlignHandCards();
            reserveHandAlign = false;
        }
    }

    #region プレイヤー側手札・デッキ処理
    /// <summary>
    /// デッキからカードを１枚引き手札に加える
    /// </summary>
    /// <param name="handID">対象手札番号</param>
    private void DrawCard(int handID)
    {
        // オブジェクト作成
        var obj = Instantiate(cardPrefab, cardsParent);
        // カード処理クラスを取得・リストに格納
        Card objCard = obj.GetComponent<Card>();
        cardInstances.Add(objCard);

        // カード初期設定
        objCard.Init(this, deckIconTrs.position);
        objCard.PutToZone(CardZone.ZoneType.Hand, dammyHandUI.GetHandPos(handID));
        objCard.SetInitialCardData(testCardData);
    }

    /// <summary>
    /// 手札が指定枚数になるまでカードを引く
    /// </summary>
    /// <param name="num">指定枚数</param>
    private void DrawCardsUntilNum(int num)
    {
        // 現在の手札枚数を取得
        int nowHandNum = 0;
        foreach(var card in cardInstances)
        {
            if(card.nowZone == CardZone.ZoneType.Hand)
                nowHandNum++;
        }
        // 新たに引くべき枚数を取得
        int drawNum = num - nowHandNum;
        if (drawNum <= 0)
            return;

        // 手札UIに枚数を指定
        dammyHandUI.SetHandNum(nowHandNum + drawNum);

        // 連続でカードを引く(Sequence)
        const float DrawIntervalTime = 0.1f; //ドロー間の時間間隔
        var drawSequence = DOTween.Sequence();
        isDrawing = true;
        for (int i = 0; i < drawNum; i++)
        {
            // 1枚引く処理
            drawSequence.AppendCallback(() =>
            {
                DrawCard(nowHandNum);
                nowHandNum++;
            });
            // 時間間隔を設定
            drawSequence.AppendInterval(DrawIntervalTime);
        }
        drawSequence.OnComplete(() => isDrawing = false);
    }

    /// <summary>
    /// 手札のカードを整列させる
    /// </summary>
    private void AlignHandCards()
    {
        // 手札整理処理
        int index = 0; // 手札内番号
        // ダミー手札を整列
        dammyHandUI.ApplyLayout();
        // 各カードをダミー手札に合わせて移動
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
    /// 現在の手札の枚数を手札UI処理クラスに反映させて整列する
    /// </summary>
    private void CheckHandCardsNum()
    {
        //　現在の手札枚数を取得
        int nowHandNum = 0;
        foreach(var item in cardInstances)
        {
            if (item.nowZone == CardZone.ZoneType.Hand)
                nowHandNum++;
        }
        //　ダミー手札に枚数を指定
        dammyHandUI.SetHandNum(nowHandNum);
        // 手札枚数に合わせて手札を指定
        //（手札枚数を変更した同フレームではダミー手札オブジェクトが動いていないため一瞬だけ遅延実行）
        reserveHandAlign = true;


    }
    #endregion

    #region カードドラッグ処理
    /// <summary>
    /// カードのドラッグ操作を開始する
    /// </summary>
    /// <param name="dragCard">操作対象カード</param>
    public void StartDragging(Card dragCard)
    {
        // 手札補充演出中なら終了
        if (isDrawing)
            return;

        // 操作対象カードを記憶
        draggingCard = dragCard;

        draggingCard.isMoving = true;
        // 他のカードオブジェクトより兄弟間で一番後ろにする(最前面表示にする)
        draggingCard.transform.SetAsLastSibling();

        draggingCardPosZ = draggingCard.transform.position.z;
        draggingCard.rectTransform = draggingCard.GetComponent<RectTransform>();
    }

    
   

    /// <summary>
    /// ドラッグ操作更新処理
    /// </summary>
    public void UpdateDragging()
    {
        // タップ位置を取得
        Vector2 tapPos = Input.mousePosition;
        // タップ位置を変換する（スクリーン座標->Canvasのローカル座標）
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            tapPos,
            mainCamera,
            out tapPos);

        // 座標を適用
        draggingCard.rectTransform.anchoredPosition = tapPos;


        //ドラック中のカードがあれば処理行う
        //if (draggingCard != null)
        //{
        //    Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //    mousePos.z = draggingCardPosZ;
        //    //mousePos.z = 0;
        //    draggingCard.transform.position = mousePos;
        //}
    } 
    
    /// <summary>
    /// カードのドラッグ操作を終了する
    /// </summary>
    public void EndDragging()
    {
        // 重なっているオブジェクトの情報をすべて取得する
        // (判定が必要なオブジェクトにはすべてBoxCollider2Dが付与されているのでそれを利用して判定）
        // このオブジェクトのスクリーン座標を取得する
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(mainCamera, draggingCard.transform.position);
        // メインカメラから上記で取得した座標に向けてRayを飛ばす
        Ray ray = mainCamera.ScreenPointToRay(pos);

        // ドラッグ先のオブジェクト取得処理
        CardZone targetZone = null;
        Card targetCard = null;
        GameObject targetZoneObj = null;
        //Rayが当たった全オブジェクトに対しての処理
        foreach(RaycastHit2D hit in Physics2D.RaycastAll(ray.origin, ray.direction, 100.0f))
        {
            // 当たったオブジェクトが存在しないなら終了
            if (!hit.collider)
            {
                //debug
                Debug.Log("オブジェクト存在しない");
                break;
            }

            // 当たったオブジェクトがドラッグ中のカードと同一なら次へ
            var hitObj = hit.collider.gameObject;
            if (hitObj == draggingCard.gameObject)
            {
                Debug.Log("同一カード");
                continue;
            }

            // オブジェクトがカードエリアなら取得して次へ
            var hitArea = hitObj.GetComponent<CardZone>();
            if(hitArea != null)
            {
                Debug.Log("カードエリア" + hitArea);
                targetZone = hitArea;
                
                targetZoneObj = hitObj;
                continue;
            }

            // オブジェクトがカードなら取得して次へ
            var hitCard = hitObj.GetComponent<Card>();
            if(hitCard != null)
            {
                Debug.Log("カード" + hitCard);
                targetCard = hitCard;
                continue;
            }
        }
        
        // 重なった対象ごとによる処理
        //if(targetCard != null &&
        //    (targetCard.nowZone >= CardZone.ZoneType.PlayBoard0 && targetCard.nowZone <= CardZone.ZoneType.PlayBoard4))
        //{// プレイボードにあるカードと重なった場合
        //    // 合成処理(未実装)
        //}
        if(targetZone != null)
        {// カードと重ならずカードエリアと重なった場合
            // 設置処理
            draggingCard.PutToZone(targetZone.zoneType, targetZone.zoneNum,targetZone.GetComponent<RectTransform>().position);
            draggingCard.TrackingToZone(targetZoneObj);
            CheckHandCardsNum();
            // 手札以外->手札への移動の場合、カードをリスト内で一番後ろにする
            if(draggingCard.nowZone == CardZone.ZoneType.Hand)
            {
                cardInstances.Remove(draggingCard);
                cardInstances.Add(draggingCard);
            }
        }
        else
        {// いずれとも重ならなかった場合
            // 元の位置に戻す
            Debug.Log("元の位置に");
            draggingCard.BackToBasePos();
        }

        draggingCard.isMoving = false;
        draggingCard = null;
        

        //Debug.Log("ドラッグ処理終了");
        //draggingCard.BackToBasePos();
        //Debug.Log("元の場所に戻る処理");

        //draggingCard = null;
    }
    #endregion

}
