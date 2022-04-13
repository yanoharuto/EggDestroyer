using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    [SerializeField] public float mFirstSpeed;
    [SerializeField] private float mMaxSpeed;
    [SerializeField] private float mSpeedD;//������
    [SerializeField] private float mSpeedA;
    /// <summary>
    /// ��������@0�ɂȂ�ƌ������Ȃ���
    /// </summary>
    /// <param name="_speed">��������������</param>
    /// <returns></returns>
    public float Deceleration(float _speed,float _acceleration)
    {
        if (_acceleration > 0)
        {
            _acceleration *= mSpeedD;//����
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
        if (_acceleration < mMaxSpeed)//�ō����x�H
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
