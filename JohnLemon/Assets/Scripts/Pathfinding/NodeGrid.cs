using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public Vector3 centerGrid;
    public Vector3 sizeGrid;
    public Vector3 nodeSize;
    public LayerMask layer;
    public float sphereColliderSize;

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
                    Gizmos.color = Physics.OverlapSphere(nodeCenter, sphereColliderSize, layer).Length == 0 ? Color.white : Color.red;
                    Gizmos.DrawWireCube(nodeCenter, nodeSize - new Vector3(0.2f, 0.2f, 0.2f));
                }
            }
        }
    }
}
