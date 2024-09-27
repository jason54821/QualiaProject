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
        // バトル開始時にプレイヤーのターンから始める
        currentTurn = TurnState.PlayerTurn;
        StartPlayerTurn();
    }

    void Update()
    {
        // ターンの状態に応じた処理を行う
        switch (currentTurn)
        {
            case TurnState.PlayerTurn:
                // プレイヤーの行動を管理

                //配置したらコストを処理する

                //作戦展開関連

                break;

            case TurnState.Action:
                // 処理
                break;

            case TurnState.EnemyTurn:
                // 敵の行動を管理
                break;

            case TurnState.Waiting:
                // 次のターンへの準備
                break;
        }
    }

    public void StartBattle()
    {
        Debug.Log("バトル開始 - ステータス初期化");

        // プレイヤーキャラクターのステータスを初期化
        foreach (var character in battleManager.playerCharacters)
        {
            character.InitStatus();  // 各キャラクターに初期化処理を行う
        }

        StartPlayerTurn();
    }

    // プレイヤーのターン開始時の処理
    public void StartPlayerTurn()
    {
        Debug.Log("プレイヤーのターンです");
        currentTurn = TurnState.PlayerTurn;
        // プレイヤーがカードを出せるようにするなどの処理

        battleManager.turnStart.interactable = true;

        //StartZoneにカードできるようにする

        //作戦展開するときに追加で配置できる処理
    }

    public void StartAction()
    {
        Debug.Log("アクションを処理するターンです");
        currentTurn = TurnState.Action;

        // ボタンを無効化しておく
        battleManager.turnStart.interactable = false;

        // プレイヤーが配置したカードによって行動を処理する
        StartCoroutine(ProcessAllCardActions());
    }

    private IEnumerator ProcessAllCardActions()
    {
        // 全てのカードの処理を完了するまで待機する
        List<Coroutine> coroutines = new List<Coroutine>();

        foreach (Card card in fieldManager.cardInstances)
        {
            Debug.Log("card");
            // コルーチンを開始し、リストに追加
            coroutines.Add(StartCoroutine(card.TimeMoveCoroutine()));
        }

        // 全てのコルーチンが完了するまで待機
        foreach (Coroutine coroutine in coroutines)
        {
            yield return coroutine;  // 各コルーチンが終了するまで待つ
        }

        Debug.Log("全てのカード処理が完了しました");

        // 全ての処理が完了したらボタンを有効にする
        battleManager.turnStart.interactable = true;

        // ターンを終了
        EndTurn();
    }

    // 敵のターン開始時の処理
    public void StartEnemyTurn()
    {
        Debug.Log("敵のターンです");
        currentTurn = TurnState.EnemyTurn;
        // 敵のAI行動を実行するなどの処理
        EndTurn();
    }

    // ターンの切り替え処理
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
