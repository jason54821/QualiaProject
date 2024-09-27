using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// カードバトルを管理するマネージャークラス
/// </summary>
public class BattleManager : MonoBehaviour
{
    //オブジェクト・コンポーネント参照
    [SerializeField]
    private FieldManager fieldManager;
    [SerializeField]
    private TurnManager turnManager;

    //UI参照
    [SerializeField] public Button turnStart;

    //プレイヤーキャラクター参照
    [SerializeField] public CharacterDataSO[] playerCharacters;

    //敵キャラクター参照
    //[SerializeField] public Enemy[] enemies;

    public CardDataSO testCardData;

    //初期化処理・すべての初期化をここからスタート
    void Start()
    {
        fieldManager.Init(this);
        turnManager.Init(this, fieldManager);

        Debug.Log("BattleManager.cs : 初期化完了");

        // カード効果名表示テスト
        foreach (var cardEffect in testCardData.effectList)
        {
            // 効果名文字列を取得
            string nameText = CardEffectDefine.Dic_EffectName_JP[cardEffect.cardEffect];
            // 効果値変数を文字列に埋め込む
            nameText = string.Format(nameText, cardEffect.value);
           
            Debug.Log(nameText);
        }

    }

}
