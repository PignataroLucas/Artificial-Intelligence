using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxQuery : MonoBehaviour , IQuery
{
    public SpatialGrid targetGrid;

    public float width;
    public float heigth;

    public IEnumerable<IGridEntity> selected = new List<IGridEntity>();

    private void Awake()
    {
       
    }

    public void Start()
    {
        targetGrid = transform.parent.parent.parent.parent.parent.GetComponent<SpatialGrid>();
    }

    public IEnumerable<IGridEntity> Query()
    {
        var w = width;
        var h = heigth;

        return targetGrid.Query(
                                transform.position + transform.forward + new Vector3(-w, 0, -h),
                                transform.position + transform.forward + new Vector3(w, 0, h),
                                x => true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.forward, new Vector3(width, 1, heigth));
    }


}
