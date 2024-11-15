using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour, IPoolable
{
    public virtual void Respawn(Vector3 position)
    { 
    }
}

// pool�� ���� �ٽ� ���� �� Respawn �Լ� ����ǵ��� �ϱ� ���� �߰� (poolable �������� Ipoolable ��� �޾ƾ���)
public interface IPoolable
{
    public void Respawn(Vector3 position);
}