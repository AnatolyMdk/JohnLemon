using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{

    public Vector3 CenterGrid;
    public Vector3 SizeGrid;
    public Vector3 SizeNode;
    public float SphereRadiusWallDetector;
    public LayerMask WallLayer;
    public Node[,] grid;
    private int nNodesX;
    private int nNodesZ;

    #region DEBUG
    public Transform initialPos;
    public Transform finalPos;
    public bool algorithm;
    private List<Node> path;
    #endregion

    private void Start()
    {
        nNodesX = Convert.ToInt32(SizeGrid.x / SizeNode.x);
        nNodesZ = Convert.ToInt32(SizeGrid.z / SizeNode.z);
        grid = new Node[nNodesX, nNodesZ];
        for (int i = 0; i < nNodesX; i++)
        {
            for (int j = 0; j < nNodesZ; j++)
            {
                float posX = CenterGrid.x - SizeGrid.x / 2 + i * SizeNode.x + SizeNode.x / 2;
                float posZ = CenterGrid.z - SizeGrid.z / 2 + j * SizeNode.z + SizeNode.z / 2;
                Vector3 pos = new Vector3(posX, CenterGrid.y, posZ);
                bool walkable = Physics.OverlapSphere(pos, SphereRadiusWallDetector, WallLayer).Length == 0;
                grid[i, j] = new Node(pos, walkable);
            }
        }

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int plusI = -1; plusI <= 1; plusI++)
                {
                    for (int plusJ = -1; plusJ <= 1; plusJ++)
                    {
                        if (Math.Abs(plusI) == Math.Abs(plusJ)) continue;

                        int newI = i + plusI;
                        int newJ = j + plusJ;
                        if (!OutOfBounds(newI, newJ))
                        {
                            grid[i, j].AddNeighbour(grid[newI, newJ]);
                        }
                    }
                }
                Debug.Log("Node [" + i + ", " + j + "] have " + grid[i, j].neighbours.Count + " neighbours.");
            }
        }
    }


    public List<Node> Algorithm(Vector3 initialPos, Vector3 finalPos)
    {
        ResetDistancesGrid();
        ResetVisitedGrid();
        ResetOnQueueGrid();
        NodePriorityQueue queue = new NodePriorityQueue();
        Node currentNode = NodeFromWorldPos(initialPos);
        Node targetNode = NodeFromWorldPos(finalPos);
        currentNode.distance = 0;
        currentNode.father = null;
        queue.Add(currentNode);

        while (!queue.IsEmpty())
        {
            currentNode = queue.Pop();
            if (currentNode == targetNode) break;
            currentNode.Visited = true;
            foreach (Node neighbour in currentNode.neighbours)
            {
                if (neighbour.Visited || !neighbour.walkable) continue;
                int newDistance = currentNode.distance + 1;
                if (newDistance < neighbour.distance)
                {
                    neighbour.distance = newDistance;
                    neighbour.father = currentNode;
                }
                if (!neighbour.OnQueue)
                {
                    queue.Add(neighbour);
                    neighbour.OnQueue = true;
                }
            }
        }
        path = RetracePath(currentNode);
        return path;
    }

    private List<Node> RetracePath(Node n)
    {
        List<Node> listNode = new List<Node>();
        while (n != null)
        {
            listNode.Add(n);
            n = n.father;
        }
        listNode.Reverse();
        return listNode;
    }

    private Node NodeFromWorldPos(Vector3 pos)
    {
        float percentX = Math.Abs(pos.x - (CenterGrid.x - SizeGrid.x / 2)) / SizeGrid.x;
        float percentZ = Math.Abs(pos.z - (CenterGrid.z - SizeGrid.z / 2)) / SizeGrid.z;

        int indexX = Convert.ToInt32(grid.GetLength(0) * percentX - .5f);
        int indexZ = Convert.ToInt32(grid.GetLength(1) * percentZ - .5f);

        return grid[indexX, indexZ];
    }

    private void ResetDistancesGrid()
    {
        foreach (Node n in grid) n.distance = int.MaxValue;
    }

    private void ResetVisitedGrid()
    {
        foreach (Node n in grid) n.Visited = false;
    }

    private void ResetOnQueueGrid()
    {
        foreach (Node n in grid) n.OnQueue = false;
    }

    private bool OutOfBounds(int i, int j)
    {
        return i < 0 || j < 0 || i >= grid.GetLength(0) || j >= grid.GetLength(1);
    }
    private void OnDrawGizmos()
    {

        if (algorithm)
        {
            path = Algorithm(initialPos.position, finalPos.position);
            algorithm = false;
        }


        if (grid != null)
        {
            foreach (Node n in grid)
            {
                if (n == NodeFromWorldPos(initialPos.position)) Gizmos.color = Color.blue;
                else if (n == NodeFromWorldPos(finalPos.position)) Gizmos.color = Color.green;
                else Gizmos.color = n.walkable ? Color.white : Color.red;
                Gizmos.DrawWireCube(n.position, SizeNode - new Vector3(0.05f, 0.05f, 0.05f));
            }
            if (path != null)
            {
                Gizmos.color = Color.red;
                foreach (Node n in path)
                {
                    Gizmos.DrawCube(n.position, SizeNode / 4);
                }
            }
        }
    }
}
