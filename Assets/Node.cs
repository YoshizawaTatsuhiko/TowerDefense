using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class Node : MonoBehaviour
{
    public Node Parent { get; set; } = null;
    

    private enum NodeType
    {
        None,
        Open,
        Close,
    }
}
