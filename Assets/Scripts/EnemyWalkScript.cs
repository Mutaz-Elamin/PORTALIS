using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalkScript : MonoBehaviour
{
    public GameObject player;

    private NavMeshAgent agent;

    public LayerMask groundLayer, playerLayer;

    private Vector3 desPoint;
    private bool desPointSet;
    public float desPointMin;
    public float desPointMax;

    public float walkInterval;
    private float lastWalkTime = -Mathf.Infinity;

    public float sightRange;
    private bool playerInSightRange;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);

        if (!playerInSightRange)
        {
            GetComponent<Renderer>().material.color = Color.white;
            Walk();
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
            Chase();
        }
    }

    private void Chase() { 
        agent.SetDestination(player.transform.position);
    }

    public void Walk()
    {
        if (!desPointSet){
            if (Time.time - lastWalkTime < walkInterval) return;
            SearchDesPoint();
        }

        if (desPointSet){
            agent.SetDestination(desPoint);

            Vector3 distanceToDesPoint = transform.position - desPoint;
            distanceToDesPoint.y = 0;

            if (distanceToDesPoint.magnitude < 3f)
            {
                desPointSet = false;
                lastWalkTime = Time.time;
            }
        }
    }

    private void SearchDesPoint(){
        float zPos = RandRange();
        float xPos = RandRange();

        desPoint = new Vector3(transform.position.x + xPos, transform.position.y, transform.position.z + zPos);

        if (Physics.Raycast(desPoint, -transform.up, 2f, groundLayer))
        {
            desPointSet = true;
        }
    }

    private float RandRange(){
        float pos = Random.Range(desPointMin, desPointMax);
        return Random.value > 0.5f ? pos : -pos;
    }
}
