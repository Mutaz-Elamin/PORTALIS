using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalkScript : MonoBehaviour
{
    public GameObject player;

    private BoxCollider attackCollider;

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
    public float attackRange;
    private bool playerInAttackRange;

    public float attackInterval;
    public float attackActiveDuration;

    private Coroutine attackRoutine;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        attackCollider = GetComponentInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            GetComponent<Renderer>().material.color = Color.white;
            Walk();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            GetComponent<Renderer>().material.color = Color.red;
            Chase();
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            Attack();
        }
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);
        if (player != null)
        {
            transform.LookAt(player.transform);
        }

        if (attackRoutine == null)
        {
            attackRoutine = StartCoroutine(AttackCycle(attackActiveDuration, attackInterval));
        }
    }

    private void Chase()
    {
        agent.SetDestination(player.transform.position);
        StopAttackRoutine();
    }

    public void Walk()
    {
        StopAttackRoutine();

        if (!desPointSet)
        {
            if (Time.time - lastWalkTime < walkInterval) return;
            SearchDesPoint();
        }

        if (desPointSet)
        {
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

    private void SearchDesPoint()
    {
        float zPos = RandRange();
        float xPos = RandRange();

        desPoint = new Vector3(transform.position.x + xPos, transform.position.y, transform.position.z + zPos);

        if (Physics.Raycast(desPoint, -transform.up, 2f, groundLayer))
        {
            desPointSet = true;
        }
    }

    private float RandRange()
    {
        float pos = Random.Range(desPointMin, desPointMax);
        return Random.value > 0.5f ? pos : -pos;
    }

    private IEnumerator AttackCycle(float activeDuration, float interval)
    {
        float offDuration = Mathf.Max(0f, interval - activeDuration);

        while (true)
        {
            if (attackCollider != null)
            {
                attackCollider.enabled = true;
                GetComponent<Renderer>().material.color = Color.yellow;
            }

            if (activeDuration > 0f)
                yield return new WaitForSeconds(activeDuration);
            else
                yield return null;

            if (attackCollider != null)
            {
                attackCollider.enabled = false;
                GetComponent<Renderer>().material.color = Color.black;
            }

            if (offDuration > 0f)
                yield return new WaitForSeconds(offDuration);
            else
                yield return null;
        }
    }

    private void StopAttackRoutine()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        // Attack did not use this function
    }
}
