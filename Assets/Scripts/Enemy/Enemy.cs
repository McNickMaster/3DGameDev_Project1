using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Transform[] patrolPath;
    public Transform head;

    public NavMeshAgent agent;

    public float vision_distance = 200f;

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
        RaycastHit[] hits = SendCast();

        foreach(RaycastHit hit in hits)
        {
            if(hit.collider != null)
            {
            //Debug.Log(hit.collider.name);

                if(hit.collider.name.Equals("PlayerModel"))
                {
                
                    Time.timeScale = 0.025f;
                    Player.instance.GetCaught(head);
                    Debug.Log("*GAME OVER*");
                }
            }
        }
        
    }

    RaycastHit[] SendCast()
    {
        RaycastHit hit1, hit2, hit3, hit4, hit5, hit6;
        
        Physics.Raycast(head.position - (transform.right * 0.2f) + (transform.forward * 0.5f), transform.forward, out hit1);
        Physics.Raycast(head.position - (transform.right * 0.2f) + (transform.forward * 0.5f) - (transform.up * 0.5f), transform.forward, out hit2);
        Physics.Raycast(head.position - (transform.right * 0.2f) + (transform.forward * 0.5f) - (transform.up), transform.forward, out hit3);

        Physics.Raycast(head.position + (transform.right * 0.2f) + (transform.forward * 0.5f), transform.forward, out hit4);
        Physics.Raycast(head.position + (transform.right * 0.2f) + (transform.forward * 0.5f) - (transform.up * 0.5f), transform.forward, out hit5);
        Physics.Raycast(head.position + (transform.right * 0.2f) + (transform.forward * 0.5f) - (transform.up), transform.forward, out hit6);

        return new RaycastHit[]{hit1, hit2, hit3};
    }

    void OnDrawGizmos()
    {
       // Gizmos.DrawWireCube(head.position + transform.forward * vision_distance, transform.localScale);
    }

}
