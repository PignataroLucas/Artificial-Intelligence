using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeQuery : MonoBehaviour, IQuery //IA2-P2
{
    public SpatialGrid targetGrid;
    public float radius = 5f;
    public float angle = 45;


    private void Start()
    {
        targetGrid = transform.parent.parent.parent.parent.parent.GetComponent<SpatialGrid>();
    }

    public IEnumerable<IGridEntity> Query()
    {
        return targetGrid.Query(transform.position   + new Vector3(-radius, 0, -radius),
                                transform.position   + new Vector3(radius, 0, radius),
                                x => (Vector3.Distance(x, transform.position) <= radius) && (Vector3.Angle(x-transform.position,transform.forward)<angle));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay( transform.position, Quaternion.AngleAxis(angle, Vector3.up) * transform.forward * radius);
        Gizmos.DrawRay( transform.position, Quaternion.AngleAxis(-angle, Vector3.up) * transform.forward * radius);
        
    }
}
