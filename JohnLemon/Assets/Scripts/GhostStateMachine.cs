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

    Transform currentTarget;
    float maxChaseDistance = 5f;

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
        while( currentState == state.Chase)
        {
            /*
            towardsTarget = currentTarget.position - transform.position;
            MoveTowards(towardsTarget);

            if (towardsTarget.magnitude > maxChaseDistance)
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
