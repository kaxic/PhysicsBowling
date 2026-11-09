using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    //ボールを前方に投げる強さ
    [SerializeField] private float throwforce = 12f;

    //ボールの物理演算コンポーネント
    private Rigidbody rb;

    //すでに投球したかどうか
    private bool hasThrown = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //スペースキーが押されていて、まだ投げてない場合
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && !hasThrown)
        {
            ThrowBall();
        }
    }


    //ボールを投げる処理
    private void ThrowBall()
    {
        //連続投球を防ぐ
        hasThrown = true;

        rb.AddForce(Vector3.forward * throwforce, ForceMode.Impulse);

        //ゲームマネージャーに投球を通知
        BowlingGameManager.Instance.OnBallThrown();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //投球後にボールがピンに衝突したか確認
        if (hasThrown && collision.collider.GetComponent<PinController>() != null)
        {
            BowlingGameManager.Instance.OnFirstPinHit();
        }
    }
}
