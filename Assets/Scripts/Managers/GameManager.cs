using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour , IUpdate
{


    [Header("PositionsToSpawnReferenceDwarf")]
    [SerializeField] private GameObject _dwarfsTransformParent;
    [SerializeField] private List<Transform> _dwarfPositionSpawn;

    [Header("PositionsToSpawnReferenceGoblin")]
    [SerializeField] private GameObject _goblinTransformParent;
    [SerializeField] private List<Transform> _goblinPositionSpawn;


    public GameObject dwarf,goblin;
    private int currentIndexDwarf = 0;
    private int _maxDwarfToSpawnDwarf = 5;

    private int currentIndexGoblin = 0;
    private int _maxGoblinToSpawn = 5;

    private void Start()
    {
        UpdateManager.Instance.AddUpdate(this);
        _dwarfPositionSpawn= _dwarfsTransformParent.GetComponentsInChildren<Transform>().ToList();
        _dwarfPositionSpawn.Remove(_dwarfPositionSpawn.First());

        _goblinPositionSpawn = _goblinTransformParent.GetComponentsInChildren<Transform>().ToList();
        _goblinPositionSpawn.Remove(_goblinPositionSpawn.First());
    }


    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
           if(currentIndexDwarf < _maxDwarfToSpawnDwarf && _dwarfPositionSpawn.Count >0)
           {
                int randomIndex = Random.Range(0,_dwarfPositionSpawn.Count);
                Vector3 spawnPos = _dwarfPositionSpawn[randomIndex].position;
                Instantiate(dwarf, spawnPos, Quaternion.identity);
                _dwarfPositionSpawn.RemoveAt(randomIndex);
                currentIndexDwarf++;
           }
           else
           {
                Debug.Log("No more positions available to spawn objects");
           }
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            if (currentIndexGoblin < _maxGoblinToSpawn && _goblinPositionSpawn.Count > 0)
            {
                int randomIndex = Random.Range(0, _goblinPositionSpawn.Count);
                Vector3 spawnPos = _goblinPositionSpawn[randomIndex].position;
                Instantiate(goblin, spawnPos, Quaternion.identity);
                _goblinPositionSpawn.RemoveAt(randomIndex);
                currentIndexGoblin++;
            }
            else
            {
                Debug.Log("No more positions available to spawn objects");
            }
        }

    }

}
