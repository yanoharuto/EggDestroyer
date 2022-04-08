using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField, Range(0.6f, 0.99f)] private float mRiseCoefficient;
    [SerializeField, Range(1.0f, 10.0f)] private float mFirstRiseA;//初期加速度
    [SerializeField] private float mSpeed;
    private float mRiseA;//上昇加速度
    private bool mIsKeepRise;//上昇し続ける？
    private Rigidbody mRigidbody;
    private PlayerStateEnum.PlayerState mPlayerState;
    /// 上昇
    private float Rise()
    {
        mPlayerState = PlayerStateEnum.PlayerState.rise;
        mRiseA *= mRiseCoefficient;//適当加速度
        return mRiseA;
    }
    //移動
    private void Move()
    {
        float x = Input.GetAxis("Horizontal") * mSpeed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * mSpeed * Time.deltaTime;
        float y = Rise();
        Vector3 v = new Vector3(x, y, z);
        transform.position += v;
    }    

    //発射準備
    private void PrepareRise()
    {
        Debug.Log("PrepareRise");
        mPlayerState = PlayerStateEnum.PlayerState.Idol;//発射準備状態
        mRiseA = mFirstRiseA;//加速度を元に戻す

    }
    private void Start()
    {
        PrepareRise();
        mRigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!(mPlayerState == PlayerStateEnum.PlayerState.Descent)) 
        {
            if (Input.GetKey(KeyCode.Space))
            {
                mIsKeepRise = true;
                mRigidbody.useGravity = false;
                Move();
            }
            else if (mIsKeepRise == true)//スペースキーを離したら落下
            {
                mPlayerState = PlayerStateEnum.PlayerState.Descent;
                mIsKeepRise = false;
                mRigidbody.useGravity = true;
            }
        }
    }
    private void OnCollisionEnter(Collision _collision)
    {
        if(_collision.gameObject.CompareTag("Ground"))
        {
            PrepareRise();
        }
    }
}
