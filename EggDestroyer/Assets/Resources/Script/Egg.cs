using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] private List<AudioClip> mBeforeBreakAudio = new List<AudioClip>();
    [SerializeField, Range(0.1f, 1.0f)] private float mBreakAllowableLimit;
    [SerializeField] private int mClackedLayer;
    private AudioSource mAudioSource;
    
    private Rigidbody mRigidbody;
    private bool mIsClacked;

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
    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        mAudioSource = GetComponent<AudioSource>();
        mIsClacked = false;
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
        }
    }
}
