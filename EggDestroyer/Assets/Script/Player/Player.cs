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
    private void Rise()
    {
        Vector3 RiseVel;//上昇速度
        mPlayerState = PlayerStateEnum.PlayerState.rise;
        mRiseA *= mRiseCoefficient;//適当加速度
        RiseVel = new Vector3(transform.position.x, mRiseA, transform.position.z);   
        transform.position += RiseVel;
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
                Rise();
                Vector3 vector3 = new Vector3(0, 0, 0);
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    vector3 = new Vector3(0.01f, 0, 0);
                }
                else if(Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    vector3 = new Vector3(-0.01f, 0, 0);
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    vector3=new Vector3(0,0,0.01f);
                }
                else if( Input.GetKeyDown(KeyCode.DownArrow))
                {
                    vector3 = new Vector3(0, 0, -0.01f);
                }
                transform.position += vector3;

            }
            else if (mIsKeepRise == true)//スペースキーを離したら落下
            {
                mPlayerState = PlayerStateEnum.PlayerState.Descent;
                mIsKeepRise = false;
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
