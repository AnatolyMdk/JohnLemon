using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePriorityQueue
{
    private NodeQueue FirstNode;
    public int Count { get; private set; }

    public NodePriorityQueue()
    {
        FirstNode = null;
        Count = 0;
    }

    public void Add(Node n)
    {
        if (FirstNode == null)
        {
            FirstNode = new NodeQueue(n, null);
            Count++;
            return;
        }

        NodeQueue current = FirstNode;
        NodeQueue previous = null;
        while (current != null && current.data.distance < n.distance)
        {
            previous = current;
            current = current.next;
        }

        if(current == FirstNode)
            FirstNode = new NodeQueue(n, current);
        else
            previous.next = new NodeQueue(n, current);
        Count++;
    }

    public bool IsEmpty()
    {
        return Count == 0;
    }

    public Node Pop()
    {
        if (FirstNode == null) return null;
        NodeQueue n = FirstNode;
        FirstNode = FirstNode.next;
        n.next = null;
        Count--;
        return n.data;
    }

}
