using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    [SerializeField] public float mFirstSpeed;
    [SerializeField] private float mMaxSpeed;
    [SerializeField] private float mSpeedD;//減速量
    [SerializeField] private float mSpeedA;
    /// <summary>
    /// 減速する　0になると減速しないよ
    /// </summary>
    /// <param name="_speed">減速したい速さ</param>
    /// <returns></returns>
    public float Deceleration(float _speed,float _acceleration)
    {
        if (_acceleration > 0)
        {
            _acceleration *= mSpeedD;//減速
        }
        else
        {
            _acceleration = 0;
        }
        if (_speed > 0)
        {
            _speed = _acceleration;
        }
        else if (_speed < 0)
        {
            _speed = -_acceleration;
        }
        return _speed;
    }
    private float acceleration( float _acceleration)
    {
        if (_acceleration < mMaxSpeed)//最高速度？
        {
            _acceleration *= mSpeedA;
        }
        else
        {
            _acceleration = mMaxSpeed;
        }
        return _acceleration;
    }

}
