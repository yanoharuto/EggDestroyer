using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Range(1.0f, 3.0f)] private float mRiseVel;
    private float mRiseA = 0;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            Vector3 RiseVel = new Vector3(0, mRiseA + mRiseA, 0);
            transform.position += RiseVel;
            mRiseA += 0.1f;
        }
    }
}
