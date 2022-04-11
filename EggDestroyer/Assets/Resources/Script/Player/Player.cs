using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float mFirstRiseSpeed;//初期上昇速度
    [SerializeField, Range(0.6f, 0.99f)] private float mRiseD;//上昇減速量

    [SerializeField] private float mFirstMoveSpeed;//初期移動速度
    [SerializeField] private float mMoveA;//移動加速度
    [SerializeField] private float mMoveD;//移動減速量

    private float mNowRiseA;//上昇加速度
    private float mNowMoveA;//移動加速度
    private Vector3 mNowMoveVel;//移動量
    private bool mIsKeepRise;//上昇し続ける？
    private Rigidbody mRigidbody;
    private PlayerStateEnum.PlayerState mPlayerState;
    /// 上昇
    private float Rise()
    {
        mPlayerState = PlayerStateEnum.PlayerState.rise;
        mNowRiseA *= mRiseD;//適当減速
        return mNowRiseA;
    }
    //移動
    private void Move()
    {
        Boost();
        float x = Input.GetAxis("Horizontal") * mNowMoveA * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * mNowMoveA * Time.deltaTime;
        float y = Rise();
        mNowMoveVel += new Vector3(x, y, z);
        transform.position += mNowMoveVel;
    }
    private void Boost()
    {
        if (mPlayerState == PlayerStateEnum.PlayerState.rise) 
        {
            if (Input.GetKeyUp("Horizontal") || Input.GetKeyUp("Horizontal"))
            {
                mNowMoveA *= mMoveA;
            }
            else//何も入力してないなら減速
            {
                mNowMoveA *= mMoveD;
            }
        }
    }
    //発射準備
    private void PrepareRise()
    {
        mPlayerState = PlayerStateEnum.PlayerState.Idol;//発射準備状態
        mNowRiseA = mFirstRiseSpeed;//加速度を元に戻す
        mNowMoveA = mFirstMoveSpeed;
    }
    private void Start()
    {
        PrepareRise();
        mRigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(!(mPlayerState == PlayerStateEnum.PlayerState.Idol))
        {
            Move();
        }
        if (!(mPlayerState == PlayerStateEnum.PlayerState.Descent)) //下降以外なら
        {
            if (Input.GetKey(KeyCode.Space))
            {
                mIsKeepRise = true;
                mRigidbody.useGravity = false;
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
