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
    private int currentIndex = 0;
    private int _maxDwarfToSpawn = 5;

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
           if(currentIndex < _maxDwarfToSpawn && _dwarfPositionSpawn.Count >0)
           {
                int randomIndex = Random.Range(0,_dwarfPositionSpawn.Count);
                Vector3 spawnPos = _dwarfPositionSpawn[randomIndex].position;
                Instantiate(dwarf, spawnPos, Quaternion.identity);
                _dwarfPositionSpawn.RemoveAt(randomIndex);
                currentIndex++;
           }
           else
           {
                Debug.Log("No more positions available to spawn objects");
           }
        }
    }

}
