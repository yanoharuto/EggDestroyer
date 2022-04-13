using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] private List<AudioClip> mBeforeBreakAudio = new List<AudioClip>();
    [SerializeField] private float mBreakAllowableLimit;
    [SerializeField] private float mFistBlowOffSpeed;
    [SerializeField] private float mMaxBlowOffSpeed;
    [SerializeField,Range(0.6f,1.0f)] private float mBlowOffD;//減速量
    [SerializeField] private int mClackedLayer;

    private AudioSource mAudioSource;
    private AudioClip mBllowOffAudio;
    private Rigidbody mRigidbody;
    private bool mIsClacked;
    private float mBlowOffSpeed;
    [SerializeField]private Vector3 mBlowOffForce;
    /// <summary>
    /// 音を鳴らします
    /// </summary>
    /// <param name="_audioClip">鳴らしたい音</param>
    private void AudioPlay(AudioClip _audioClip)
    {
        mAudioSource.PlayOneShot(_audioClip);   
    }
    //割れるよ
    private void Clack()
    {
        mRigidbody.isKinematic = true;
        foreach (AudioClip audioClip in mBeforeBreakAudio)
        {
            AudioPlay(audioClip);
        }
        mIsClacked = true;
        gameObject.layer = mClackedLayer;
    }
    //吹っ飛ぶ力を決める
    private void RecieveBlowOffForce(Vector3 _leverage)
    {
        mBlowOffSpeed = mFistBlowOffSpeed;
        float x = transform.position.x - _leverage.x * mBlowOffSpeed * Time.deltaTime;
        float z = transform.position.z - _leverage.z * mBlowOffSpeed * Time.deltaTime;
        mBlowOffForce.Set(x, 0, z) ;
        
    }
    //吹っ飛ぶ
    private void BlowOff()
    {
        mRigidbody.AddForce(mBlowOffForce,ForceMode.Impulse);
        mBlowOffSpeed *= mBlowOffD;
        if (mBlowOffSpeed < 0) 
        {
            Debug.Log(mBlowOffSpeed);
            mBlowOffSpeed = 0;
        }
        if(mBlowOffSpeed>mMaxBlowOffSpeed)
        {
            mBlowOffSpeed = mMaxBlowOffSpeed;
        }
        float x = mBlowOffForce.x * mBlowOffSpeed * Time.deltaTime;
        float z = mBlowOffForce.z * mBlowOffSpeed * Time.deltaTime;
        Debug.DrawRay(transform.position, mBlowOffForce,Color.white);
        mBlowOffForce.Set(x, 0, z ) ;
        
    }
    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        mAudioSource = GetComponent<AudioSource>();
        mIsClacked = false;
        mBlowOffForce = new Vector3(0, 0, 0);
        mBllowOffAudio = (AudioClip)Resources.Load("Audio/Paper");
        mBlowOffSpeed = 0;
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
                    AudioPlay(mBllowOffAudio);   
                    RecieveBlowOffForce(collision.transform.position);
                }
            }
        }
    }
    private void Update()
    {

        BlowOff();
    }
}
