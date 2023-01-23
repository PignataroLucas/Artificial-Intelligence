using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    
    public List<Node> neightBours = new List<Node>();
    public LayerMask node;

    private void Start()
    {
        GetNeightbourd(Vector3.right);
        GetNeightbourd(Vector3.left);
        GetNeightbourd(Vector3.forward);
        GetNeightbourd(Vector3.back);
    }

    void GetNeightbourd(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 2.2f))
        {
            var node = hit.collider.GetComponent<Node>();
            if (node != null)
                neightBours.Add(node);
        }
    }

}
