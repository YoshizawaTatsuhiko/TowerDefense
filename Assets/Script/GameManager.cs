using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;

    private static GameManager _instance;

    private void Awake()
    {
        if (!_instance) _instance = this;
        else Destroy(gameObject);
    }
}
