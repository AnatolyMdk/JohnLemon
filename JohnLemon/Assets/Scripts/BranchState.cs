using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchState : MonoBehaviour
{
    public float radiusNoise;
    public LayerMask ghostsMask;
    public AudioSource hit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hit.Play();
            Collider[] ghosts = Physics.OverlapSphere(transform.position, radiusNoise, ghostsMask);
            foreach(Collider ghost in ghosts)
            {
                GhostStateMachine ghostState = ghost.gameObject.GetComponent<GhostStateMachine>();
                if(ghostState != null)
                    ghostState.ChangeToInvestigation(transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radiusNoise);
    }
}
