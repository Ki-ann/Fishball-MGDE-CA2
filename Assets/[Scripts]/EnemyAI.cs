using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    NavMeshAgent agent;
    Rigidbody rigidBody;
    Transform player;
    bool retreat;
    [HideInInspector]
    public enum AI_STATE { WANDERING, ATTACK, RETREAT, DEAD };
    public AI_STATE state = AI_STATE.ATTACK;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red, 5f);
        switch (state)
        {
            default:
            case AI_STATE.WANDERING:
                break;
            case AI_STATE.ATTACK:
                Attack();
                break;
            case AI_STATE.RETREAT:
                Retreat();
                break;
            case AI_STATE.DEAD:
                break;
        }
    }

    void Attack()
    {
        agent.SetDestination(player.position);
    }

    void Retreat()
    {
        //Debug.Log(agent.velocity);
        if (!retreat)
        {
            if (Mathf.Abs(agent.velocity.x) > 0)
            {
                agent.velocity -= transform.forward * 1f;
            }
            retreat = true;
        }
        else
        {
            agent.SetDestination(-transform.forward * 2f);
            if (agent.remainingDistance > Random.Range(0.1f, 0.5f))
            {
                state = AI_STATE.ATTACK;
                retreat = false;
            }
        }

    }
    void Dead()
    {
        //explode
        Destroy(gameObject, 5f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "DeadZone")
        {
            rigidBody.velocity = transform.forward * agent.speed;

            agent.enabled = false;
            state = AI_STATE.DEAD;
            Debug.Log("TEst");
        }
        if (other.tag == "RunZone")
        {
            if ((state == AI_STATE.ATTACK || state == AI_STATE.WANDERING))
            {
                state = AI_STATE.RETREAT;
            }
        }
    }
}
