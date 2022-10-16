using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;
    public int distance;
    public bool walkable;
    public bool Visited;
    public bool OnQueue;
    public Node father;
    public List<Node> neighbours;

    public Node (Vector3 _position, bool _walkable)
    {
        position = _position;
        father = null;
        distance = int.MaxValue;
        walkable = _walkable;
        neighbours = new List<Node>();
    }

    public void AddNeighbour(Node n)
    {
        neighbours.Add(n);
    }

    public bool IsNeighbour(Node n)
    {
        return neighbours.Contains(n);
    }
}
