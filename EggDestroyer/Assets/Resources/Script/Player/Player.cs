using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float mFirstRiseSpeed;//初期上昇速度
    [SerializeField, Range(0.6f, 0.99f)] private float mRiseD;//上昇減速量

    [SerializeField] private float mFirstMoveSpeed;//初期移動速度
    [SerializeField] private float mMaxMoveSpeed;//最高移動速度
    [SerializeField, Range(1.0f, 1.99f)] private float mMoveA;//移動加速度
    [SerializeField,Range(0.6f,0.99f)] private float mMoveD;//移動減速量

    private float mNowRiseA;//上昇加速度
    private float mNowMoveA;//移動加速度
    private float mMoveX;
    private float mMoveZ;
    private bool mIsKeepRise;//上昇し続ける？
    private bool mIsInput;//動いてる？
    private Rigidbody mRigidbody;
    private PlayerStateEnum.PlayerState mPlayerState;
    /// 上昇
    private float Rise()
    {
        mPlayerState = PlayerStateEnum.PlayerState.rise;
        mNowRiseA *= mRiseD;//適当減速
        return mNowRiseA;
    }
    private float Deceleration(float _speed)
    {
        if (mNowMoveA > 0)//最初の速さより大きい？
        {
            mNowMoveA *= mMoveD;//減速
        }
        else
        {
            mNowMoveA = 0;
        }
        if (_speed > 0)
        {
            _speed = mNowMoveA;
        }
        else if (_speed < 0)
        {
            _speed = -mNowMoveA;
        }
        return _speed;
    }
    private void acceleration()
    {
        if (mNowMoveA < mMaxMoveSpeed)//最高速度？
        {
            mNowMoveA *= mMoveA;
        }
        else
        {
            mNowMoveA = mMaxMoveSpeed;
        }
    }
    //移動
    private void Move()
    {

        if (mPlayerState == PlayerStateEnum.PlayerState.rise)
        {
            if (Input.GetKey(KeyCode.DownArrow) ||
                Input.GetKey(KeyCode.UpArrow) ||
                Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.RightArrow))
            {
                mIsInput = true;
            }
            else//何も入力してないなら減速
            {

                mIsInput = false;
            }
        }
       
        float y = 0;
        Vector3 mMoveVel;
        if (mPlayerState == PlayerStateEnum.PlayerState.rise) y = Rise();
        if (mIsInput)
        {
            acceleration();
            mMoveX = Input.GetAxis("Horizontal") * mNowMoveA;
            mMoveZ = Input.GetAxis("Vertical") * mNowMoveA;
            Debug.Log(mMoveX);
            Debug.Log(mMoveZ);
        }
        else
        {
            mMoveX = Deceleration(mMoveX);
            mMoveZ = Deceleration(mMoveZ);

        }
        mMoveVel = new Vector3(mMoveX, y, mMoveZ);

        transform.position += mMoveVel * Time.deltaTime;
    }
    //発射準備
    private void PrepareRise()
    {
        mPlayerState = PlayerStateEnum.PlayerState.Idol;//発射準備状態
        mNowRiseA = mFirstRiseSpeed;//加速度を元に戻す
        mNowMoveA = mFirstMoveSpeed;
        mMoveX = 0;
        mMoveZ = 0;

        mIsInput = false;
    }
    private void Start()
    {
        PrepareRise();
        mRigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {

        if (!(mPlayerState == PlayerStateEnum.PlayerState.Descent)) //下降以外なら
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Move();
                mPlayerState = PlayerStateEnum.PlayerState.rise;
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
        else
        {
            //下降中でも減速しながら移動
            Move();
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
