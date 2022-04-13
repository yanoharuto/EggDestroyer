using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpeedController _mRiseSpeedController;

    [SerializeField]private SpeedController _mMoveSpeedController;

    private float mNowRiseSpeed;//上昇加速度
    private float mMoveSpeedX;
    private float mMoveSpeedZ;
    private string mInputName;
    private Rigidbody mRigidbody;
    public PlayerStateEnum.PlayerState mPlayerState;

    /// 上昇
    private void Rise()
    {

 
        mPlayerState = PlayerStateEnum.PlayerState.rise;
        mNowRiseSpeed = _mRiseSpeedController.DecreaseAcceleration(); //適当減速

    }
   
    private string InputKey()
    { 
        string _InputName;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _InputName = KeyCode.DownArrow.ToString();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _InputName = KeyCode.UpArrow.ToString();
            
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _InputName = KeyCode.LeftArrow.ToString();
            
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
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
        Vector3 mMoveVel;
        Debug.Log(NowInputName);
        if (mPlayerState == PlayerStateEnum.PlayerState.rise)
        {
            Rise();
            if (NowInputName == mInputName ||//上昇中で入力し続けているなら
                mInputName == null)//最初の一回
            {
                _mMoveSpeedController.AddAcceleration();
                mMoveSpeedX = _mMoveSpeedController.ReflectSpeed(Input.GetAxis("Horizontal"));
                mMoveSpeedZ = _mMoveSpeedController.ReflectSpeed(Input.GetAxis("Vertical"));
                mInputName = NowInputName;
            }
        }
        if (NowInputName == null || NowInputName != mInputName)
        {
            if (_mMoveSpeedController.IsSpeedInited())
            {
                
                _mMoveSpeedController.InitSpeed();
                mInputName = NowInputName;
            }
            else
            {
                _mMoveSpeedController.DecreaseAcceleration();
                mMoveSpeedX = _mMoveSpeedController.ReflectSpeed(mMoveSpeedX);
                mMoveSpeedZ = _mMoveSpeedController.ReflectSpeed(mMoveSpeedZ);
            }
        }
        mMoveVel = new Vector3(mMoveSpeedX, mNowRiseSpeed, mMoveSpeedZ);
        transform.position += mMoveVel * Time.deltaTime;
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
