using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    #region シングルトン維持用処理(変更不要)

    // シングルトン維持用
    public static Data instance;

    //Awake(Startより前に1度だけ実行
    private void Awake()
    {
        //シングルトン用処理
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // ゲーム起動時処理
        InitialProcess();
    }
    #endregion

    // 各種コンポーネント
    public PlayerDeckData playerDeckData;

    private void InitialProcess()
    {

    }
}
