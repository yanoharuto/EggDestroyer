using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] private List<AudioClip> mBeforeBreakAudio = new List<AudioClip>();
    [SerializeField] private float mBreakAllowableLimit;
    [SerializeField] private int mClackedLayer;//割れた後のレイヤーの番号

    private AudioSource mAudioSource;
    private AudioClip mBllowOffAudio;
    private Rigidbody mRigidbody;
    private bool mIsClacked;
    private SpeedController _mMoveSpeedController;
    private float mBlowOffSpeedX, mBlowOffSpeedZ;
    [SerializeField]private Vector3 mBlowOffForce;
    /// <summary>
    /// 音を鳴らします
    /// </summary>
    /// <param name="_audioClip">鳴らしたい音</param>
    private void AudioPlay(AudioClip _audioClip)
    {
        mAudioSource.PlayOneShot(_audioClip);   
    }
    /// <summary>
    /// 卵が割れ動かなくなる
    /// </summary>
    private void Clack()
    {
        mRigidbody.isKinematic = true;
        foreach (AudioClip audioClip in mBeforeBreakAudio)//割れた音
        {
            AudioPlay(audioClip);
        }
        mIsClacked = true;
        gameObject.layer = mClackedLayer;
    }
    //吹っ飛ぶ力を決める
    private void SetBlowOffForce()
    {
        mBlowOffSpeedX = _mMoveSpeedController.ReflectSpeed(mRigidbody.velocity.x) * Time.deltaTime;//速さをもらってくる
        mBlowOffSpeedZ = _mMoveSpeedController.ReflectSpeed(mRigidbody.velocity.z) * Time.deltaTime;
        Debug.Log(mBlowOffSpeedX);
        mBlowOffForce.Set(mBlowOffSpeedX, 0, mBlowOffSpeedZ);
        Debug.Log(mBlowOffForce);

    }
    //吹っ飛ぶ
    private void BlowOff()
    {
        mRigidbody.AddForce(mBlowOffForce, ForceMode.Impulse);//吹っ飛ぶ
        Debug.DrawRay(transform.position, mBlowOffForce, Color.white);
        _mMoveSpeedController.DecreaseAcceleration();//減速

        mBlowOffSpeedX = _mMoveSpeedController.ReflectSpeed(mBlowOffSpeedX);//減速した速さをもらってくる
        mBlowOffSpeedZ = _mMoveSpeedController.ReflectSpeed(mBlowOffSpeedZ);
        mBlowOffForce.Set(mBlowOffSpeedX, 0, mBlowOffSpeedZ) ;
        
    }
    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        mAudioSource = GetComponent<AudioSource>();
        mIsClacked = false;
        mBlowOffForce = new Vector3(0, 0, 0);
        mBllowOffAudio = (AudioClip)Resources.Load("Audio/Paper");
        _mMoveSpeedController = GetComponent<SpeedController>();
        _mMoveSpeedController.InitSpeed();//速さを初期化
        mBlowOffSpeedZ = 0;
        mBlowOffSpeedX = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float x = Mathf.Abs(transform.position.x - collision.transform.position.x);
            float z = Mathf.Abs(transform.position.z - collision.transform.position.z);
            if (x < mBreakAllowableLimit && z < mBreakAllowableLimit &&
                !mIsClacked) //ど真ん中にプレイヤーが落ちたら
            {
                Clack();
            }
            {
                PlayerStateEnum.PlayerState playerState = collision.gameObject.GetComponent<Player>().mPlayerState;
                if (playerState == PlayerStateEnum.PlayerState.Descent)//落下中なら
                {
                    AudioPlay(mBllowOffAudio);   //ぶつかった時の音
                    _mMoveSpeedController.InitSpeed();//速度を初期化
                }
            }
        }
    }
    private void Update()
    {
        SetBlowOffForce();//吹っ飛ぶ力を設定
        BlowOff();
    }
}
