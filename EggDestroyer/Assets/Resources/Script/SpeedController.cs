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
    /// <summary>
    /// 減速
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// 加速
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// 初期速度にする
    /// </summary>
    public void InitSpeed()
    {
        mSpeed = mFirstSpeed;
      
    }
    /// <summary>
    /// 引数を参照してスピードを返すよ
    /// </summary>
    /// <param name="_speed">これが0だとこの数が返ってくる</param>
    /// <returns>0より大きいと正数で逆だと負の数でスピードが返る</returns>
    public float ReflectSpeed(float _speed)
    {
        float speed = mSpeed;
        if (_speed < 0)
        {
            speed = -mSpeed;
        }
        else if (_speed == 0)
        {
            speed = _speed;
        }
        return speed;
    }
    /// <summary>
    /// 最初の速度より遅い？
    /// </summary>
    /// <returns></returns>
    public bool IsBlowFirstSpeed()
    {
        if (mSpeed <= mFirstSpeed) return true; 
        
        return false;
    }
    /// <summary>
    /// 減速完了
    /// </summary>
    /// <returns></returns>
    public bool IsDecelerationComlieted()
    {
        if (mSpeed == 0) return true;
        return false;
    }
}
