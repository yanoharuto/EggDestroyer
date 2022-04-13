using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    [SerializeField] private float mFirstSpeed;
    [SerializeField] private float mMaxSpeed;
    [SerializeField,Range(0.0f,1.0f)] private float mSpeedD;//減速量
    [SerializeField,Range(1.0f,1000.0f)] private float mSpeedA;
    private float mSpeed;
    ///
    public float DecreaseAcceleration()
    {
        if (mSpeed > 1)
        {
            mSpeed *= mSpeedD;//減速
        }
        else
        {
            mSpeed = 0;
        }
        return mSpeed;
    }
    public float AddAcceleration()
    {
        if (mSpeed < mMaxSpeed)//最高速度？
        {
            mSpeed *= mSpeedA;
        }
        else
        {
            mSpeed = mMaxSpeed;
        }
        return mSpeed;
    }

    public void InitSpeed()
    {
        mSpeed = mFirstSpeed;
    }
    public float ReflectSpeed(float _speed)
    {
        if (_speed > 0)
        {
            _speed = mSpeed;
        }
        else if (_speed < 0)
        {
            _speed = -mSpeed;
            
        }
        return _speed;
    }
}
