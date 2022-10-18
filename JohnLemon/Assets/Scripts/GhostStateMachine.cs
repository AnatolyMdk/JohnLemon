using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostStateMachine : MovingEntity
{
    enum state
    {
        Normal,
        Investigation,
        Chase
    }

    state currentState;
    Vector3 targetPosition;
    Vector3 towardsTarget;
    float moveRadius = 5f;

    public Dijkstra Pathfinder;


    Transform currentTarget;
    public float maxChaseDistance = 10f;
    float distanceToChange = 1f;
    

    void RecalculateTargetPosition()
    {
        targetPosition = transform.position + Random.insideUnitSphere * moveRadius;
        targetPosition.y = 0;
    }

    void Start()
    {
        RecalculateTargetPosition();
        StartCoroutine(FSM());
        Debug.Log(currentState);
    }

    IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    void ChangeState(state nextState)
    {
        Debug.Log(currentState + "->" + nextState);
        currentState = nextState;
    }

    IEnumerator Normal ()
    {
        while (currentState == state.Normal)
        {
            towardsTarget = targetPosition - transform.position;
            MoveTowards(towardsTarget.normalized);

            if (towardsTarget.magnitude < 0.25f)
                RecalculateTargetPosition();

            Debug.DrawLine(transform.position, targetPosition, Color.green);
            yield return 0;
        }
    }

    IEnumerator Investigation()
    {
        while (currentState == state.Investigation)
        {
            towardsTarget = currentTarget.position - transform.position;
            if (towardsTarget.magnitude <= maxChaseDistance)
                ChangeState(state.Chase);
            yield return 0;
        }
    }

    IEnumerator Chase ()
    {
        List<Node> path = Pathfinder.Algorithm(transform.position, currentTarget.position);
        Node current = path[1];
        // posicion del jugador al llamar a Chase()
        Vector3 prevCurrentTarget = currentTarget.position;

        while (currentState == state.Chase)
        {
            if (currentTarget.position != prevCurrentTarget)
                path = Pathfinder.Algorithm(transform.position, currentTarget.position);
            towardsTarget = current.position - transform.position;
            MoveTowards(towardsTarget);
            // si la distancia al objetivo es menor que la maxima establecida
            if (towardsTarget.magnitude < distanceToChange && path.Count > 1)
            {
                current = path[1];
                path.RemoveAt(0);
            }
            
            if((currentTarget.position - transform.position).magnitude > maxChaseDistance)
                ChangeState(state.Normal);
            prevCurrentTarget = currentTarget.position;
            yield return 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentTarget = other.transform;
            ChangeState(state.Chase);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, maxChaseDistance);
    }
}
