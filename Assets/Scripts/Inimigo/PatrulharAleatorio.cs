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
            if (agent.remainingDistance <= agent.stoppingDistance || (tempo >= 6.0f))
            {
                Vector3 point;
                if (RandomPoint(transform.position, range, out point))
                {
                    agent.SetDestination(point);
                    tempo = 0;
                    // print("run?");
                    // print(!anim.GetCurrentAnimatorStateInfo(0).IsName("Slow Run"));
                    if (anim)
                    {
                        anim.SetBool("podeAndar", true);
                    }    
                    agent.isStopped = false;
                }
            } else 
            {
                
                if (agent.remainingDistance <= 1) {
                    anim.SetBool("podeAndar", false);
                    agent.isStopped = true;
                    // print("parado");
                }
                tempo += Time.deltaTime;
                // if (anim)
                // {
                //     print("idle");
                    // anim.Play("idle");
                    
                // }         
            }
            Debug.DrawLine(transform.position, agent.destination, Color.magenta);
        }
        
    }

}
