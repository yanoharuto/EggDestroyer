using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    [SerializeField] public float mFirstSpeed;
    [SerializeField] private float mMaxSpeed;
    [SerializeField] private float mSpeedD;//å∏ë¨ó 
    [SerializeField] private float mSpeedA;
    /// <summary>
    /// å∏ë¨Ç∑ÇÈÅ@0Ç…Ç»ÇÈÇ∆å∏ë¨ÇµÇ»Ç¢ÇÊ
    /// </summary>
    /// <param name="_speed">å∏ë¨ÇµÇΩÇ¢ë¨Ç≥</param>
    /// <returns></returns>
    public float Deceleration(float _speed,float _acceleration)
    {
        if (_acceleration > 0)
        {
            _acceleration *= mSpeedD;//å∏ë¨
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
        if (_acceleration < mMaxSpeed)//ç≈çÇë¨ìxÅH
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
