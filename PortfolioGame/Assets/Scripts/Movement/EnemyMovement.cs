using UnityEngine.AI;
using UnityEngine;
using System;

public class EnemyMovement : MonoBehaviour
{
    public Transform Player;
    private NavMeshAgent agent;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    private bool playerInRange = false;
    private Vector3 startPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
    }

   
    void Update()
    {
        agent.SetDestination(Player.position);
        if (playerInRange)
        {
            // speler volgen
            agent.SetDestination(Player.position);
        }
        else
        {
            // terug naar startpositie
            agent.SetDestination(startPosition);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
