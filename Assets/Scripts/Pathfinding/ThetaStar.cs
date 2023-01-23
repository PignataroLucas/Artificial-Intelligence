using System.Collections;
using System.Collections.Generic;


public class ThetaStar<T>
{

    public delegate Dictionary<T, float> GetNeighbours(T current);
    public delegate bool Satisfies (T current);
    public delegate float Heuristic(T current);
    public delegate bool InSight (T grandFather, T current);
    public delegate float Cost(T grandFather, T current);

    public List<T> Execute(T start,Satisfies satisfies, GetNeighbours getNeighbourds , Heuristic heuristic,InSight inSight, Cost newCost) 
    {
        PriorityQueue<T> pending = new PriorityQueue<T>();
        HashSet<T> visited = new HashSet<T>();
        Dictionary<T,T> parents = new Dictionary<T,T>();
        Dictionary<T,float> costs = new Dictionary<T,float>();
        pending.Enqueue(start, 0);
        costs.Add(start, 0);

        while (!pending.IsEmpty)
        {
            T current = pending.Dequeue();

            if (satisfies(current))
            {
                return ContructPath(current, parents);
            }

            visited.Add(current);
            Dictionary<T, float> neighbours = getNeighbourds(current);//

            foreach (var item in neighbours)
            {
                var currNeigh = item.Key;
                var currNeighCost = item.Value;
                if (visited.Contains(currNeigh)) continue;

                if (parents.ContainsKey(current) && inSight(parents[current], currNeigh))
                {
                    var grandFather = parents[current];
                    var tentativeCost = costs[grandFather] + newCost(grandFather, currNeigh) + heuristic(currNeigh);
                    if (costs.ContainsKey(currNeigh) && tentativeCost > costs[currNeigh]) continue;
                    parents[currNeigh] = grandFather;
                    costs[currNeigh] = tentativeCost;
                    pending.Enqueue(currNeigh, tentativeCost);
                    continue;
                }
                else
                {
                    var tentativeCost = costs[current] + currNeighCost + heuristic(currNeigh);
                    if (costs.ContainsKey(currNeigh) && tentativeCost > costs[currNeigh]) continue;
                    parents[currNeigh] = current;
                    costs[currNeigh] = tentativeCost;
                    pending.Enqueue(currNeigh, tentativeCost);
                }
            }
        }

        return null;
    }
    List<T> ContructPath(T end, Dictionary<T, T> parents)
    {
        var path = new List<T>();
        path.Add(end);
        while (parents.ContainsKey(path[path.Count - 1]))
        {
            var lastNode = path[path.Count - 1];
            path.Add(parents[lastNode]);
        }
        path.Reverse();
        return path;
    }
}
