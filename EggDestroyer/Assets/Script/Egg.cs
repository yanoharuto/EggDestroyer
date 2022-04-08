using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] private AudioClip[] mBeforeBreakAudio;
    [SerializeField, Range(0.1f, 1.0f)] private float mBreakAllowableLimit;
    private AudioSource mAudioSource;
    private AudioClip mBreakEggAudio;
    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        mBreakEggAudio = (AudioClip)Resources.Load("Assets/Audio/BreakEgg");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            float x = Mathf.Abs(transform.position.x - collision.transform.position.x);
            float z= Mathf.Abs(transform.position.z - collision.transform.position.z);
            if (x < mBreakAllowableLimit && z < mBreakAllowableLimit) 
            {
                
            }
        }
    }
}
