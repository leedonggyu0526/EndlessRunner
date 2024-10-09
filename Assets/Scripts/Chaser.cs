using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour
{
    NavMeshAgent agent;
    Transform Player;

    public float chaseSpeed = 12f;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.SetDestination(Player.position);
    }

    void Update()
    {
        agent.speed = chaseSpeed + GameManager.inst.GetScore();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.SetDestination(Player.position);

        if (Vector3.Distance(transform.position, Player.position) <= 1f)
        {   //Debug.Log("Chaser caught the player");
            Player.GetComponent<PlayerMovement>().Die();
        }
    }
}
