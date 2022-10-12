using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector3 centerGrid;
    public Vector3 sizeGrid;
    public Vector3 nodeSize;
    public LayerMask layer;

    private void OnDrawGizmos()
    {
        int nodeX = Convert.ToInt32(sizeGrid.x / nodeSize.x);
        int nodeY = Convert.ToInt32(sizeGrid.y / nodeSize.y);
        int nodeZ = Convert.ToInt32(sizeGrid.z / nodeSize.z);

        Gizmos.DrawWireCube(centerGrid, sizeGrid);

        for (int i = 0; i < nodeX; i++)
        {
            for (int j = 0; j < nodeY; j++)
            {
                for (int k = 0; k < nodeZ; k++)
                {
                    Vector3 nodeCenter = new Vector3(centerGrid.x - (sizeGrid.x / 2) + nodeSize.x * i + nodeSize.x / 2,
                                                 centerGrid.y - (sizeGrid.y / 2) + nodeSize.y * j + nodeSize.y / 2,
                                                 centerGrid.z - (sizeGrid.z / 2) + nodeSize.z * k + nodeSize.z / 2);

                    Gizmos.color = Physics.OverlapSphere(nodeCenter, nodeSize.x / 2, layer) == null ? Color.red : Color.blue;
                    Gizmos.DrawWireCube(nodeCenter, nodeSize);
                }
            }
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
