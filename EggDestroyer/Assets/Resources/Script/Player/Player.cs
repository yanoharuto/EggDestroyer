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
   private void Rotate(string _nowInput)
    {
        Vector3 vector3 = transform.position;
        if(KeyCode.DownArrow.ToString()==_nowInput)
        {
            vector3 -= transform.forward;
            transform.LookAt(vector3);
        }
        else if (KeyCode.UpArrow.ToString() == _nowInput)
        {
            vector3 += transform.forward;
            transform.LookAt(vector3);
        }
        else if (KeyCode.RightArrow.ToString() == _nowInput)
        {
            vector3 += transform.right;
            transform.LookAt(vector3);
        }
        else if (KeyCode.LeftArrow.ToString() == _nowInput)
        {
            vector3 -= transform.right;
            transform.LookAt(vector3);
        }


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

        if (mPlayerState == PlayerStateEnum.PlayerState.rise)
        {
            Rise();
            if (NowInputName == mInputName )//上昇中で同じ方向に入力し続けているなら
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
            if (_mMoveSpeedController.IsDecelerationComlieted())//完全に減速し終えたら
            {
                _mMoveSpeedController.InitSpeed();//スピードを元に戻して
                
                mInputName = NowInputName;//進みたい方向を更新
            }
            else
            {
                Rotate(NowInputName);
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
