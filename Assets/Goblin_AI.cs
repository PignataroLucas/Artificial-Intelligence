using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_AI : MonoBehaviour
{
    public LayerMask nodes;

    public Node CurrentNode()
    {

        Node node = new global::Node();
        Collider[] nodeColliders = Physics.OverlapSphere(transform.position, 5, this.nodes);
        float distance = new float();
        for (int i = 0; i < nodeColliders.Length; i++)
        {
            if (distance == 0)
            {
                /*if (Physics.Raycast(transform.position, nodeColliders[i].transform.position, 5, obstacleMask))
                {
                    return node;
                }*/
                distance = Vector3.Distance(nodeColliders[i].transform.position, transform.position);
                node = nodeColliders[i].GetComponent<Node>();
            }
            else
            {
                if (Vector3.Distance(nodeColliders[i].transform.position, transform.position) < distance)
                {
                    /*if (Physics.Raycast(transform.position, nodeColliders[i].transform.position, 5, obstacleMask))
                    {
                        return node;
                    }*/
                    distance = Vector3.Distance(nodeColliders[i].transform.position, transform.position);
                    node = nodeColliders[i].GetComponent<Node>();
                }
            }
        }
        return node;
    }
}
