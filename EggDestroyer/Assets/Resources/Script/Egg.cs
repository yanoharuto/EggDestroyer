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
    [SerializeField] private int mClackedLayer;//割れた後のレイヤーの番号

    private AudioSource mAudioSource;
    private AudioClip mBllowOffAudio;
    private Rigidbody mRigidbody;
    private bool mIsClacked;
    private float mBlowOffSpeed;
    private float mBlowOffX, mBlowOffZ;
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
        mBlowOffX = transform.position.x - _leverage.x * Time.deltaTime * mBlowOffSpeed;
        mBlowOffZ = transform.position.z - _leverage.z * Time.deltaTime * mBlowOffSpeed;

        mBlowOffForce.Set(mBlowOffX, 0, mBlowOffZ) ;
        
    }
    //吹っ飛ぶ
    private void BlowOff()
    {
        mRigidbody.AddForce(mBlowOffForce,ForceMode.Impulse);

            Debug.Log(mBlowOffX);
        if (Mathf.Abs(mBlowOffX) <= 1)
        {
            mBlowOffX = 0;   

        }
        if (Mathf.Abs(mBlowOffZ) <= 1)
        {
            mBlowOffZ = 0;
        }

        mBlowOffX *= mBlowOffD;
        mBlowOffZ *= mBlowOffD;

        mBlowOffForce.Set(mBlowOffX, 0, mBlowOffZ) ;
        
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
                //Clack();
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
