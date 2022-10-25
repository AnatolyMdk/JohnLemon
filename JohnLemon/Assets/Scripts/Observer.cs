﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;
    public GameEnding gameEnding;
    public bool m_IsPlayerInRange;

    public float radiusNoise;
    public LayerMask ghostMask;

    void OnTriggerEnter (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update ()
    {
        if (m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;
            
            if (Physics.Raycast (ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    if(transform.parent.tag != "Boss") {
                        gameEnding.CaughtPlayer ();
                    } else {
                        Collider[] ghosts = Physics.OverlapSphere(transform.position, radiusNoise, ghostMask);
                        foreach (Collider ghost in ghosts) {
                            GhostStateMachine ghostState = ghost.gameObject.GetComponent<GhostStateMachine>();
                            //ghostState.wait();
                            ghostState.rage(player.transform);
                        }
                    }
                }
            }
        }
    }
    
    /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radiusNoise);
    }*/
}
