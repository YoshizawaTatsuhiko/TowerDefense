using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1.0f;

    private IEnumerator AlongTheRoute(Vector3[] route)
    {
        foreach (var dest in route)
        {
            yield return Move(dest);
        }
    }

    private IEnumerator Move(Vector3 dest)
    {
        Vector3 movementDir = dest - transform.position;

        while (movementDir.magnitude > 0.1f)
        {
            transform.position += movementDir.normalized * _moveSpeed;
            movementDir = dest - transform.position;
            yield return null;
        }
    }

    //private void Move(Vector2 dest)
    //{
    //    Vector2 myPos = transform.position;

    //    if ((dest - myPos).magnitude <= 0.1f) return;

    //    transform.position = (dest - myPos).normalized * _moveSpeed;
    //}
}
