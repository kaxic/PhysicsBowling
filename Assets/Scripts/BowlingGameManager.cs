using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class BowlingGameManager : MonoBehaviour
{
    //他のスクリプトからアクセスするための参照
    public static BowlingGameManager Instance { get; private set; }

    //得点表示用テキスト
    [SerializeField] private TMP_Text scoreText;

    //最終結果表示用テキスト
    [SerializeField] private TMP_Text resultText;

    //最初のピン衝突後に待つ時間
    [SerializeField] private float settleDelay = 4f;

    //ピンに当たらなかった時の制限時間
    [SerializeField] private float maxApproachTime = 12f;

    //倒れたピンの本数
    private int fallenPinCount = 0;

    //投球済みかどうか
    private bool hasThrown = false;

    //ピンに当たったかどうか
    private bool firstPinHit = false;

    //ゲームが終了したかどうか
    private bool isGameOver = false;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScoreText();

        resultText.gameObject.SetActive(false);
    }

    private void Update()
    {
        //Rキーでシーン再読み込み
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            RestartGame();
        }
    }


    //ボールが投げられた時に呼び出す
    public void OnBallThrown()
    {
        if (hasThrown)
        {
            return;
        }

        hasThrown = true;


        //5秒後に結果を表示する処理を開始
        StartCoroutine(WaitForResult());
    }

    public void OnFirstPinHit()
    {
        if (hasThrown && !isGameOver)
        {
            firstPinHit = true;
        }
    }

    public void AddFallenPin()
    {
        if (!hasThrown || isGameOver)
        {
            return;
        }

        if (fallenPinCount >= 10)
        {
            return;
        }


        fallenPinCount++;

        //画面表示を更新
        UpdateScoreText();

        // 現在の得点をConsoleに表示
        Debug.Log("倒したピン：" + fallenPinCount + " / 10");
    }

    //現在の得点を更新
    private void UpdateScoreText()
    {
        scoreText.text = "Score : " + fallenPinCount + " / 10";
    }

    //衝突を待ちピンが倒れる時間を確保する
    private IEnumerator WaitForResult()
    {
        float elapsed = 0f;

        //最初のピンに当たるまで待つ
        while (!firstPinHit && elapsed < maxApproachTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        //衝突したら転倒が落ち着くまで待つ
        if (firstPinHit)
        {
            yield return new WaitForSeconds(settleDelay);
        }


        //ゲーム終了
        isGameOver = true;

        //最終結果を表示
        resultText.text = "Result : " + fallenPinCount + " / 10\n" + "Press R to Restart";

        resultText.gameObject.SetActive(true);
    }

    //ゲームをリスタート
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
