using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Transform[] patrolPath;

    public NavMeshAgent agent;

    [SerializeField]
    private bool atTarget = false;
    private Transform currentDestination;
    private int pathIndex = -1;

    // Start is called before the first frame update
    void Awake()
    {
        NextPath();
    }

    // Update is called once per frame
    void Update()
    {
        atTarget = agent.remainingDistance < agent.stoppingDistance;

        if(atTarget)
        {
            AtDestination();
            atTarget = false;
        }

        
        SetTargetPos(currentDestination.position);
    }

    void SetTargetPos(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    void AtDestination()
    {
        NextPath();
    }

    void NextPath()
    {
        pathIndex++;

        if(pathIndex >= patrolPath.Length)
        {
            pathIndex = 0;
        }
        currentDestination = patrolPath[pathIndex];
    }

}
