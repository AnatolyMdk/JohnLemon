using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostStateMachine : MovingEntity
{
    public enum state
    {
        Normal,
        Investigation,
        Chase,
        Wait,
        ChaseIndiscriminate
    }

    public state currentState;
    public Material whiteGhost;
    public Material redGhost;
    public Material yellowGhost;
    public SkinnedMeshRenderer renderer;
    public Dijkstra Pathfinder;
    public float maxChaseDistance = 10f;
    public Transform[] Waypoints;
    private Vector3 investigationPosition;
    // float moveRadius = 5f;
    float distanceToChange = 1f;
    Vector3 targetPosition;
    Vector3 towardsTarget;
    Transform currentTarget;
    float secondsWaiting;

    void Start()
    {
        StartCoroutine(FSM());
        // Debug.Log(currentState);
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

    void ChangeState(state nextState, float s)
    {
        Debug.Log(currentState + "->" + nextState);
        secondsWaiting = s;
        currentState = nextState;
    }
    public void ChangeState(state nextState, Transform destination)
    {
        Debug.Log(currentState + "->" + nextState);
        currentTarget = destination;
        currentState = nextState;
    }

    public void ChangeToInvestigation(Vector3 iposition)
    {
        Debug.Log(currentState + "->" + state.Investigation);
        investigationPosition = iposition;
        currentState = state.Investigation;
    }

    IEnumerator Normal ()
    {
        renderer.material = whiteGhost;
        int i = 0;
        while (currentState == state.Normal)
        {
            if (i == Waypoints.Length) i = 0;
            targetPosition = Waypoints[i].position;
            towardsTarget = targetPosition - transform.position;
            MoveTowards(towardsTarget.normalized);

            if (towardsTarget.magnitude < 0.25f)
                i++;

            // Debug.DrawLine(transform.position, targetPosition, Color.green);
            yield return 0;
        }
    }

    IEnumerator Investigation()
    {
        renderer.material = yellowGhost;

        List<Node> path = Pathfinder.Algorithm(transform.position, investigationPosition);
        Node current = path[1];
        bool arrived = false;
        while (currentState == state.Investigation)
        {
            towardsTarget = current.position - transform.position;
            MoveTowards(towardsTarget);

            if (towardsTarget.magnitude < distanceToChange && path.Count > 1)
            {
                current = path[1];
                path.RemoveAt(0);
            }
            else if (path.Count <= 1) arrived = true;

            if (arrived) ChangeState(state.Wait, 3);
            yield return 0;
        }
    }

    IEnumerator Wait()
    {
        float timer = secondsWaiting;
        while(currentState == state.Wait)
        {
            timer -= Time.deltaTime;
            if (timer <= 0) ChangeState(state.Normal);
            yield return 0;
        }
    }
    IEnumerator Chase ()
    {
        renderer.material = redGhost;
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

    IEnumerator ChaseIndiscriminate()
    {
        // Debug.Log(transform.name);
        renderer.material = redGhost;
        List<Node> path = Pathfinder.Algorithm(transform.position, currentTarget.position);
        Node current = path[1];
        // posicion del jugador al llamar a Chase()
        Vector3 prevCurrentTarget = currentTarget.position;

        while (currentState == state.ChaseIndiscriminate)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, maxChaseDistance);
    }
}
