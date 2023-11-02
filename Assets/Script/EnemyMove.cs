using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1.0f;

    private void Move(Vector2 dest)
    {
        Vector2 myPos = transform.position;

        if ((dest - myPos).magnitude <= 0.1f) return;

        transform.position = (dest - myPos).normalized * _moveSpeed;
    }
}
