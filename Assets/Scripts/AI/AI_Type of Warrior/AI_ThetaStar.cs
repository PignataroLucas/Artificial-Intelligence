using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ThetaStar : MonoBehaviour
{
    public float radius;
    public Vector3 offset;
    public Node init;
    public Node finish;
    public List<Node> _nodes;


    public LayerMask obstacle;

    ThetaStar<Node> _theta = new ThetaStar<Node>();

    public AI_ThetaStar SetInit(Node initial) 
    {
        init = initial;
        return this;
    }

    public AI_ThetaStar SetFinal(Node final) 
    {
        finish = final;
        return this;
    }

    public List<Node> PathfindingTheta() 
    {
        _nodes = _theta.Execute(init, Satisfies, GetNeighboursCost, Heuristic, InSight, GrandCost);
        return _nodes;
    }

    float GrandCost(Node grandFather, Node grandChild)
    {
        return Vector3.Distance(grandFather.transform.position, grandChild.transform.position);
    }
    bool InSight(Node grandFather, Node grandChild)
    {
        RaycastHit hit;
        var dir = (grandChild.transform.position - grandFather.transform.position);
        if (Physics.Raycast(grandFather.transform.position, dir.normalized, out hit, dir.magnitude, obstacle))
        {
            return false;
        }
        return true;
    }

    float Heuristic(Node cur)
    {
        return Vector3.Distance(cur.transform.position, finish.transform.position);
    }

    bool Satisfies(Node curr)
    {
        return curr.Equals(finish);
    }
    Dictionary<Node, float> GetNeighboursCost(Node curr)
    {
        var dic = new Dictionary<Node, float>();
        foreach (var item in curr.neightBours)
        {
            float cost = 0;
            cost += Vector3.Distance(item.transform.position, curr.transform.position);
            dic.Add(item, cost);
        }
        return dic;
    }

    List<Node> Neighbours(Node curr)
    {
        var list = new List<Node>();
        foreach (var item in curr.neightBours)
        {
            list.Add(item);
        }
        return list;
    }

    public Node FindClosestNode(Vector3 destination)
    {
        Node closestNode = null;
        float closestNodeDistance = float.MaxValue;

        foreach ( Node node in _nodes)
        {
            float distance = Vector3.Distance(node.transform.position, destination);
            if (distance < closestNodeDistance)
            {
                closestNode = node;
                closestNodeDistance = distance;
            }
        }
        return closestNode;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (init != null)
            Gizmos.DrawSphere(init.transform.position + offset, radius);
        if (finish != null)
            Gizmos.DrawSphere(finish.transform.position + offset, radius);
        if (_nodes == null) return;
        Gizmos.color = Color.blue;
        foreach (var item in _nodes)
        {
            if (item != init && item != finish)
                Gizmos.DrawSphere(item.transform.position + offset, radius);
        }
    }

}
