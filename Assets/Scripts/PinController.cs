using UnityEngine;

public class PinController : MonoBehaviour
{
    //転倒と判定する角度
    [SerializeField] private float downAngle = 60f;

    //転倒によって中心が下がる距離
    [SerializeField] private float minHeightDrop = 0.2f;

    //転倒状態が継続すべき時間
    [SerializeField] private float confirmationTime = 0.2f;


    //ピンの初期状態
    private Vector3 initialUp;
    private float initialHeight;

    //転倒状態が続いた時間
    private float downTime = 0f;

    //転倒しているか判定
    private bool hasFallen = false;


    private void Start()
    {
        initialUp = transform.up;
        initialHeight = transform.position.y;
    }

    private void Update()
    {
        if (hasFallen)
        {
            return;
        }

        //初期姿勢からの傾きを計算
        float angle = Vector3.Angle(initialUp, transform.up);

        //初期状態から下がった距離
        float heightDrop = initialHeight - transform.position.y;

        bool isDown = angle >= downAngle || heightDrop >= minHeightDrop;

        //転倒と判定
        if (isDown)
        {
            downTime += Time.deltaTime;
        }
        else
        {
            downTime = 0f;
        }


        //一定時間倒れてたら加点
        if (downTime >= confirmationTime)
        {
            hasFallen = true;

            if (BowlingGameManager.Instance != null)
            {
                BowlingGameManager.Instance.AddFallenPin();
            }
        }
    }
}
