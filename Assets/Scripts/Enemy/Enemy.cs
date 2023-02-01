using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Transform[] patrolPath;
    public Transform head;

    public NavMeshAgent agent;

    public float vision_distance = 100f;

    [SerializeField]
    private bool atTarget = false, castHit = false;
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

        DetectPlayer();
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

    void DetectPlayer()
    {
        RaycastHit hit = SendCast();

        if(hit.collider != null)
        {
            //Debug.Log(hit.collider.name);

            if(hit.collider.name.Equals("PlayerModel"))
            {
                Time.timeScale = 0.1f;
                Debug.Log("*GAME OVER*");
            }
        }
    }

    RaycastHit SendCast()
    {
        RaycastHit hit;
        
        castHit = Physics.BoxCast(head.position,  new Vector3(3,2,3),transform.forward, out hit, transform.rotation, vision_distance);

        return hit;
    }

    void OnDrawGizmos()
    {
       // Gizmos.DrawWireCube(head.position + transform.forward * vision_distance, transform.localScale);
    }

}
