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
    private string mInputName;
    /// 上昇
    private float Rise()
    {
        mPlayerState = PlayerStateEnum.PlayerState.rise;
        mNowRiseA *= mRiseD;//適当減速
        return mNowRiseA;
    }
    /// <summary>
    /// 減速する
    /// </summary>
    /// <param name="_speed"></param>
    /// <returns></returns>
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
    private string InputKey(string _InputName)
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _InputName = KeyCode.DownArrow.ToString();
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            _InputName = KeyCode.UpArrow.ToString();
            
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            _InputName = KeyCode.LeftArrow.ToString();
            
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _InputName = KeyCode.RightArrow.ToString();
            
        }
        else
        {
            _InputName = null;
        }
        return _InputName;
    }
    //移動
    private void Move()
    {
        string NowInputName = null;
        NowInputName = InputKey(NowInputName);
        float y = 0;
        Vector3 mMoveVel;
        if (mNowMoveA <= mFirstMoveSpeed  && NowInputName != null)
        {

            mInputName = NowInputName;
            
        }
        if (NowInputName == mInputName && mPlayerState == PlayerStateEnum.PlayerState.rise)
        {
            acceleration();
            mMoveX = Input.GetAxis("Horizontal") * mNowMoveA;
            mMoveZ = Input.GetAxis("Vertical") * mNowMoveA;
        }
        else
        {
            mMoveX = Deceleration(mMoveX);
            mMoveZ = Deceleration(mMoveZ);
        }

        if (mPlayerState == PlayerStateEnum.PlayerState.rise)
        {
            y = Rise();
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
        mInputName = null;
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
