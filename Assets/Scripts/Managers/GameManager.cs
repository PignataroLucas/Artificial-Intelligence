using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour , IUpdate
{


    [Header("PositionsToSpawnReference")]
    [SerializeField] private GameObject _dwarfsTransformParent;
    [SerializeField] private List<Transform> _dwarfPositionSpawn;

    public GameObject dwarf;

    private void Start()
    {
        UpdateManager.Instance.AddUpdate(this);
        _dwarfPositionSpawn= _dwarfsTransformParent.GetComponentsInChildren<Transform>().ToList();
        _dwarfPositionSpawn.Remove(_dwarfPositionSpawn.First());
    }


    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < _dwarfPositionSpawn.Count; i++)
            {          
                Instantiate(dwarf, _dwarfPositionSpawn[i].position, Quaternion.identity);                         
            }
        }
    }

}
