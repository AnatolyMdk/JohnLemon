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

<<<<<<< Updated upstream
    Transform currentTarget;
    float maxChaseDistance = 5f;
=======
    public Dijkstra Pathfinder;
    public float maxChaseDistance = 10f;
    public Transform[] waypoints;
    Transform currentTarget;
    float distanceToChange = 1f;
    
>>>>>>> Stashed changes

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
            //MoveTowards(waypoints[0].position.normalized);

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
        while( currentState == state.Chase)
        {
            /*
            towardsTarget = currentTarget.position - transform.position;
            MoveTowards(towardsTarget);
<<<<<<< Updated upstream

            if (towardsTarget.magnitude > maxChaseDistance)
=======
            // si la distancia al objetivo es menor que la maxima establecida
            // se actualiza el nodo y se desencola el último
            if (towardsTarget.magnitude < distanceToChange && path.Count > 1)
            {
                current = path[1];
                path.RemoveAt(0);
            }
            if((currentTarget.position - transform.position).magnitude > maxChaseDistance)
>>>>>>> Stashed changes
                ChangeState(state.Normal);
            */
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
}
