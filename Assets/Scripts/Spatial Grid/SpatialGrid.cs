using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SpatialGrid : MonoBehaviour
{

    public bool showGrid;

    public float x;
    public float z;

    public float cellWidth;
    public float cellHeight;

    public int width;
    public int height;

    private Dictionary<IGridEntity,Tuple<int,int>> _lastPosition = new Dictionary<IGridEntity, Tuple<int, int>>();

    private HashSet<IGridEntity>[,] _buckets;

    readonly public Tuple<int,int> Outside = Tuple.Create(-1 , -1);

    readonly public IGridEntity[] Empty= new IGridEntity[0];


    private void Awake()
    {
        _buckets = new HashSet<IGridEntity>[width , height];


        for (var i = 0; i < width; i++)
        {
            for (int j = 0; i < height; j++)
            {
                _buckets[i, j] = new HashSet<IGridEntity>();
            }
        }

        var ents = RecursiveWalker(transform)
                    .Select(n => n.GetComponent<IGridEntity>())
                    .Where(n => n != null);


        foreach (var item in ents)
        {
            item.OnEntityAdded += UpdateEntity;
            UpdateEntity(item);
        }
    }


    private void Update()
    {
        for (var i = 0; i < width; i++)
        {
            for (int j = 0; i < height; j++)
            {
                _buckets[i, j] = new HashSet<IGridEntity>();
            }
        }

        var ents = RecursiveWalker(transform)
                    .Select(n => n.GetComponent<IGridEntity>())
                    .Where(n => n != null);


        foreach (var item in ents)
        {
            item.OnEntityAdded += UpdateEntity;
            UpdateEntity(item);
        }
    }

    public void UpdateEntity(IGridEntity entity)
    {
        var lastPos = _lastPosition.ContainsKey(entity) ? _lastPosition[entity] : Outside;
        var currentPos = GetPositionInGrid(entity.Position);

        if (lastPos.Equals(currentPos)) return;

        if (IsInsideGrid(lastPos)) _buckets[currentPos.Item1,currentPos.Item2].Add(entity);
        else { _lastPosition.Remove(entity); }
    }

    public IEnumerable <IGridEntity> Query (Vector3 aabbFrom, Vector3 aabbTo , Func<Vector3,bool> filterByPosition)
    {
        var from = new Vector3(Mathf.Min(aabbFrom.x, aabbTo.x), 0, Mathf.Min(aabbFrom.z, aabbTo.z));
        var to = new Vector3(Mathf.Max(aabbFrom.x, aabbTo.x), 0, Mathf.Max(aabbFrom.z, aabbTo.z));

        var fromCoord = GetPositionInGrid(from);
        var toCoord = GetPositionInGrid(to);

        fromCoord = Tuple.Create(Util.Clamp(fromCoord.Item1, 0, width), Util.Clamp(fromCoord.Item2, 0, height));
        toCoord = Tuple.Create(Util.Clamp(toCoord.Item1, 0, width), Util.Clamp(toCoord.Item2, 0, height));


        var cols = Util.Generate(fromCoord.Item1, x => x + 1)
                       .TakeWhile(n => n < width && n <= toCoord.Item1);

        var rows = Util.Generate(fromCoord.Item2, y => y + 1)
                       .TakeWhile(y => y < height && y <= toCoord.Item2);

        var cells = cols.SelectMany(
                                    col => rows.Select(
                                                       row => Tuple.Create(col, row)
                                                      )
                                   );




        return cells
              .SelectMany(cell => _buckets[cell.Item1, cell.Item2])
              .Where(e =>
                         from.x <= e.Position.x && e.Position.x <= to.x &&
                         from.z <= e.Position.z && e.Position.z <= to.z
                    )
              .Where(n => filterByPosition(n.Position));
    }


    public Tuple<int, int> GetPositionInGrid(Vector3 pos)
    {        
        return Tuple.Create(Mathf.FloorToInt((pos.x - x) / cellWidth),
                            Mathf.FloorToInt((pos.z - z) / cellHeight));
    }

    public bool IsInsideGrid(Tuple<int, int> position)
    {        
        return 0 <= position.Item1 && position.Item1 < width &&
               0 <= position.Item2 && position.Item2 < height;
    }

    void OnDestroy()
    {
        var ents = RecursiveWalker(transform).Select(n => n.GetComponent<IGridEntity>())
                                             .Where(n => n != null);

        foreach (var e in ents) e.OnEntityAdded -= UpdateEntity;
    }

    private static IEnumerable<Transform> RecursiveWalker(Transform parent)
    {
        foreach (Transform child in parent)
        {
            foreach (Transform grandchild in RecursiveWalker(child))
                yield return grandchild;
            yield return child;
        }
    }

    public bool areGizmosShutDown;
    public bool activatedGrid;
    public bool showLogs = true;

    private void OnDrawGizmos()
    {

        if (showGrid)
        {
            var rows = Util.Generate(z, curr => curr + cellHeight)
                       .Select(row => Tuple.Create(new Vector3(x, 0, row),
                                                   new Vector3(x + cellWidth * width, 0, row)));
            //equivalente de rows
            /*for (int i = 0; i <= height; i++)
            {
                Gizmos.DrawLine(new Vector3(x, 0, z + cellHeight * i), new Vector3(x + cellWidth * width,0, z + cellHeight * i));
            }*/

            var cols = Util.Generate(x, curr => curr + cellWidth)
                           .Select(col => Tuple.Create(new Vector3(col, 0, z),
                                                       new Vector3(col, 0, z + cellHeight * height)));

            var allLines = rows.Take(width + 1).Concat(cols.Take(height + 1));

            foreach (var elem in allLines)
            {
                Gizmos.DrawLine(elem.Item1, elem.Item2);
            }
        }

        /*
            if (buckets == null || areGizmosShutDown) return;

            var originalCol = GUI.color;
            GUI.color = Color.red;
            if (!activatedGrid) {
                var allElems = new List<IGridEntity>();
                foreach (var elem in buckets)
                    allElems = allElems.Concat(elem).ToList();

                int connections = 0;
                foreach (var entity in allElems) {
                    foreach (var neighbour in allElems.Where(x => x != entity)) {
                        Gizmos.DrawLine(entity.Position, neighbour.Position);
                        connections++;
                    }

                    if (showLogs)
                        Debug.Log("tengo " + connections + " conexiones por individuo");
                    connections = 0;
                }
            }
            else {
                int connections = 0;
                foreach (var elem in buckets) {
                    foreach (var ent in elem) {
                        foreach (var n in elem.Where(x => x != ent)) {
                            Gizmos.DrawLine(ent.Position, n.Position);
                            connections++;
                        }

                        if (showLogs)
                            Debug.Log("tengo " + connections + " conexiones por individuo");
                        connections = 0;
                    }
                }
            }

            GUI.color = originalCol;
            showLogs  = false;
        */
    }

}
