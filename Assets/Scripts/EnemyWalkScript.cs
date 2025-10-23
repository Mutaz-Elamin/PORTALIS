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

    public float health = 15f;
    
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage! Remaining HP: {health}");

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject); 
    }


    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        attackCollider = GetComponentInChildren<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Determine behavior based on player's position relative to sight and attack ranges
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
    // Hold attack position and face player and run attack cycle
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
    // Chase the player by setting destination to player's position
    private void Chase()
    {
        agent.SetDestination(player.transform.position);
        StopAttackRoutine();
    }
    // Walk to random destination points within specified range and interval
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
    // Search for a valid random destination point on the ground within specified range
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
    // toggle attack collider on and off based on active duration and interval
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
    // Stop the attack routine and disable the attack collider
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
