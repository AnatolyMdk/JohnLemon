using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeQueue
{
    public Node data;
    public NodeQueue next;
    public NodeQueue(Node _data, NodeQueue _next)
    {
        data = _data;
        next = _next;
    }
}
