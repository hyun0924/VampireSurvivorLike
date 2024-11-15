using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour, IPoolable
{
    public virtual void Respawn(Vector3 position)
    { 
    }
}

// pool에 들어가면 다시 나올 때 Respawn 함수 실행되도록 하기 위해 추가 (poolable 넣을꺼면 Ipoolable 상속 받아야함)
public interface IPoolable
{
    public void Respawn(Vector3 position);
}