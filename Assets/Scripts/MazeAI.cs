using UnityEngine;

public class MazeAI : MonoBehaviour
{

    public Transform target;
    private UnityEngine.AI.NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent=GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (target!=null)
        {
            agent.SetDestination(target.position);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
