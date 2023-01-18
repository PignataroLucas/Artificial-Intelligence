using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    
    public struct Data
    {
        public List<IUpdate> UpdateList;
    }

    public static UpdateManager Instance { get; private set; }

    public List<IUpdate> allUpdates = new List<IUpdate>();

    private void Awake()
    {
        if(!Instance) Instance= this;
        else Destroy(gameObject);
    }


    private void Update()
    {
        for (int i = 0; i < allUpdates.Count; i++)
        {
            allUpdates[i].OnUpdate();
        }
    }

    public void AddUpdate(IUpdate newUpdate)
    {
        if (!allUpdates.Contains(newUpdate))
            allUpdates.Add(newUpdate);
    }

    public void RemoveUpdate(IUpdate updateToRemove)
    {
        if (allUpdates.Contains(updateToRemove))
            allUpdates.Remove(updateToRemove);
    }

}
