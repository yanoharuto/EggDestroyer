using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpeedController _mRiseSpeedController;
    [SerializeField] private float mRotateYSpeed;
    [SerializeField]private SpeedController _mMoveSpeedController;

    private float mNowRiseSpeed;//上昇加速度
    private float mMoveSpeedX;
    private float mMoveSpeedZ;
    private string mInputName;
    private Rigidbody mRigidbody;
    public PlayerStateEnum.PlayerState mPlayerState;
    private float mDirection;
    private float mEulerAngleY;
    private bool mIsRotate;
    /// 上昇
    private void Rise()
    {
        mPlayerState = PlayerStateEnum.PlayerState.rise;
        mNowRiseSpeed = _mRiseSpeedController.DecreaseAcceleration(); //適当減速

    }
   private void Rotate(string _nowInputName)
    {
        Vector3 Rotate;
        float RotateY = mRotateYSpeed;
        if (_nowInputName == KeyCode.UpArrow.ToString())
        {
            mDirection = 0;
        }
        else if (_nowInputName == KeyCode.DownArrow.ToString())
        {
            mDirection = 180;
        }
        else if (_nowInputName == KeyCode.LeftArrow.ToString())
        {
            mDirection = 270;
            
        }
        else if(_nowInputName==KeyCode.RightArrow.ToString())
        {
            mDirection = 90;
        }
        if (mDirection + mRotateYSpeed >= transform.rotation.eulerAngles.y &&
            mDirection - mRotateYSpeed <= transform.rotation.eulerAngles.y)
        {
            mIsRotate = false;
        }
        else if (mDirection == 270 && mEulerAngleY < 45)
        {
            RotateY = -mRotateYSpeed;
        }
        else if(mDirection==0&&mEulerAngleY>225)
        {
            RotateY = mRotateYSpeed;
        }
        else if (mDirection < mEulerAngleY)
        {
            RotateY = -mRotateYSpeed;
        }
        Rotate = new Vector3(0, RotateY, 0);
        transform.Rotate(Rotate);
    }
    private string InputKey()
    { 
        string _InputName;
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
        string NowInputName = InputKey();
        Vector3 MoveVel;

        if (mIsRotate)
        {
            Rotate(mInputName);
        }
        if (mPlayerState == PlayerStateEnum.PlayerState.rise)
        {
            Rise();
            if (NowInputName == mInputName && mIsRotate == false)//上昇中で同じ方向に入力し続けているなら
            {
                _mMoveSpeedController.AddAcceleration();//加速
                //斜めには進まない
                if (mInputName == KeyCode.LeftArrow.ToString() || mInputName == KeyCode.RightArrow.ToString())
                {
                    mMoveSpeedX = _mMoveSpeedController.ReflectSpeed(Input.GetAxis("Horizontal"));
                }
                if (mInputName == KeyCode.UpArrow.ToString() || mInputName == KeyCode.DownArrow.ToString())
                {
                    mMoveSpeedZ = _mMoveSpeedController.ReflectSpeed(Input.GetAxis("Vertical"));
                }
            }
        }
        if (NowInputName == null || NowInputName != mInputName)//何も入力してなかったり急に方向転換したなら
        {
            if (_mMoveSpeedController.IsDecelerationComlieted() && !mIsRotate)//完全に減速し終えたら
            {
                _mMoveSpeedController.InitSpeed();//スピードを元に戻して
                mIsRotate = true;
                mInputName = NowInputName;//進みたい方向を更新
                mEulerAngleY = transform.eulerAngles.y;
            }
            else
            {
                _mMoveSpeedController.DecreaseAcceleration();//減速
                mMoveSpeedX = _mMoveSpeedController.ReflectSpeed(mMoveSpeedX);
                mMoveSpeedZ = _mMoveSpeedController.ReflectSpeed(mMoveSpeedZ);
            }
        }

        MoveVel = new Vector3(mMoveSpeedX, mNowRiseSpeed, mMoveSpeedZ);

        transform.position += MoveVel * Time.deltaTime;
    }
    //発射準備
    private void PrepareRise()
    {
        mPlayerState = PlayerStateEnum.PlayerState.Idol;//発射準備状態
        _mMoveSpeedController.InitSpeed();//加速度を元に戻す
        _mRiseSpeedController.InitSpeed();
        mMoveSpeedX = 0;
        mMoveSpeedZ = 0;
        mNowRiseSpeed = 0;
        mInputName = null;
    }
    private void Start()
    {
        PrepareRise();
        mDirection = 0;
        mRigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {

        if (!(mPlayerState == PlayerStateEnum.PlayerState.Descent)) //下降以外なら
        {
            if (Input.GetKey(KeyCode.Space))
            {            
                mPlayerState = PlayerStateEnum.PlayerState.rise;
                Move();
              
                mRigidbody.useGravity = false;
            }
            else if (mPlayerState==PlayerStateEnum.PlayerState.rise)//スペースキーを離したら落下
            {
                mPlayerState = PlayerStateEnum.PlayerState.Descent;
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
