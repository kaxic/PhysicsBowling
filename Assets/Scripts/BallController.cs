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
        hasThrown = true;

        rb.AddForce(Vector3.forward * throwforce, ForceMode.Impulse);
    }
}
