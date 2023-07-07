using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class Node : MonoBehaviour
{
    public static int NodeSize { get; set; } = 32;
    public Node Parent { get; set; } = null;
    public Vector2 Position { get; set; } = Vector2.zero;
    public Vector2 Center { get; set; } = Vector2.zero;

    private enum NodeType
    {
        None,
        Open,
        Close,
    }
}
