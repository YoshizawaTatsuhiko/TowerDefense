using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;

    public event System.Action<EnemyController> EnemyAction 
    { 
        add => _enemyAction += value;
        remove => _enemyAction -= value; 
    }

    private static GameManager _instance;
    private event System.Action<EnemyController> _enemyAction = null;

    private void Awake()
    {
        if (!_instance) _instance = this;
        else Destroy(gameObject);
    }
}
