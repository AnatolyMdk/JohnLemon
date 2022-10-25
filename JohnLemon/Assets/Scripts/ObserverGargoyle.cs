using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverGargoyle : MonoBehaviour
{
    public Transform player;
    public Timer gameTime;
    public float radiusNoise;
    public LayerMask ghostMask;

    bool m_IsPlayerInRange;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Alarma.alarma_Activada)
        {
            m_IsPlayerInRange = true;

            Collider[] ghosts = Physics.OverlapSphere(transform.position, radiusNoise, ghostMask);
            foreach (Collider ghost in ghosts)
            {
                GhostStateMachine ghostState = ghost.gameObject.GetComponent<GhostStateMachine>();
                //ghostState.wait();
                ghostState.ChangeToInvestigation(transform.position);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player && !Alarma.alarma_Activada)
        {
            m_IsPlayerInRange = false;

            Collider[] ghosts = Physics.OverlapSphere(transform.position, radiusNoise, ghostMask);
            foreach (Collider ghost in ghosts)
            {
                GhostStateMachine ghostState = ghost.gameObject.GetComponent<GhostStateMachine>();
                //ghostState.wait();
                ghostState.normal();
            }
        }
    }

    void Update()
    {
        if (m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    gameTime.time -= 15f;
                    m_IsPlayerInRange = false;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radiusNoise);
    }
}
