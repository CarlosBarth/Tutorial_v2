using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrulharAleatorio : MonoBehaviour
{
    private NavMeshAgent agent;
    public float range;
    private float tempo;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() 
    {
        
        // if (gent.stoppingDistance <=0)
        // {
        //     agent.isStopped = false;
        //     // anim.SetBool("podeAndar", true);
        // } else 
        // {
        //     agent.isStopped = true;
        //     // anim.SetBool("podeAndar", false);
        // }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result) 
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public void Andar(Animator anim = null) 
    {
                
        if(agent) {
            print("SD: " + agent.stoppingDistance);
            print("RD: " + agent.remainingDistance);
            print (tempo);
            if (agent.isStopped) 
            {
                print("stoped");
            }
            if (agent.remainingDistance <= 0.11f || (tempo >= 6.0f))
            {
                if (tempo <= 6.0f) 
                {
                    tempo += Time.deltaTime;
                    agent.isStopped = true;
                    anim.SetBool("podeAndar", false);
                    return;
                } else {
                    agent.isStopped = false;
                    anim.SetBool("podeAndar", true);
                }

                Vector3 point;
                if (RandomPoint(transform.position, range, out point))
                {
                    agent.SetDestination(point);
                    tempo = 0;
                }
            } else {
                tempo += Time.deltaTime;  
            }
            Debug.DrawLine(transform.position, agent.destination, Color.magenta);
        }
    }

    
}
