using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour , IUpdate , IEventListener
{
    [Header("PositionsToSpawnReferenceDwarf")]
    [SerializeField] private GameObject _dwarfsTransformParent;
    [SerializeField] private List<Transform> _dwarfPositionSpawn;

    [Header("PositionsToSpawnReferenceGoblin")]
    [SerializeField] private GameObject _goblinTransformParent;
    [SerializeField] private List<Transform> _goblinPositionSpawn;


    public GameObject dwarf,goblin;
    private int currentIndexDwarf = 0;
    private int _maxDwarfToSpawnDwarf = 2;

    private int currentIndexGoblin = 0;
    private int _maxGoblinToSpawn = 2;
    
    [SerializeField] public List<GameObject> dwarfUnits = new List<GameObject>();
    [SerializeField] private List<GameObject> goblinUnits = new List<GameObject>();
    private void Awake()
    {
        OnEnableListenerSubscriptions();
    }
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
       
        
        if (currentIndexDwarf == 2) { EventTriggers.TriggerEvent(GenericEvents.DisableButtomDwarf); }
        if (currentIndexGoblin == 2) { EventTriggers.TriggerEvent(GenericEvents.DisableButtomGoblin); }

        if (currentIndexDwarf == 2 && currentIndexGoblin == 2) { EventTriggers.TriggerEvent(GenericEvents.TurnOnStartButtom); }  
        
        
    }
    public void OnEnableListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.BuyUnitDwarf,BuyUnitDwarf);
        EventManager.StartListening(GenericEvents.BuyUnitGoblin, BuyUnitGoblin);
        EventManager.StartListening(GenericEvents.StartBattle, StartBattle);
    }
    public void OnDisableListenerSubscriptions()
    {
        EventManager.StopListering(GenericEvents.BuyUnitDwarf, BuyUnitDwarf);
        EventManager.StopListering(GenericEvents.BuyUnitGoblin, BuyUnitGoblin);
        EventManager.StopListering(GenericEvents.StartBattle, StartBattle);
    }
    private void BuyUnitDwarf(Hashtable obj)
    {
        if (currentIndexDwarf < _maxDwarfToSpawnDwarf && _dwarfPositionSpawn.Count > 0)
        {
            int randomIndex = Random.Range(0, _dwarfPositionSpawn.Count);
            Vector3 spawnPos = _dwarfPositionSpawn[randomIndex].position;
            GameObject dwarfInstantiate = Instantiate(dwarf, spawnPos, Quaternion.identity);
            dwarfUnits.Add(dwarfInstantiate);
            dwarfInstantiate.transform.parent = _dwarfPositionSpawn[randomIndex];
            _dwarfPositionSpawn.RemoveAt(randomIndex);
            currentIndexDwarf++;
            
            foreach (AI ai in FindObjectsOfType<AI>())
            {
                if (ai.genericSo.Class == TypeOfWarriors.Dwarf)
                {
                    //ai.SetDwarfUnits(dwarfUnits);
                    ai.SetGoblinUnits(goblinUnits);
                }
            }
        }        
    }
    private void BuyUnitGoblin(Hashtable obj)
    {
        if (currentIndexGoblin < _maxGoblinToSpawn && _goblinPositionSpawn.Count > 0)
        {
            int randomIndex = Random.Range(0, _goblinPositionSpawn.Count);
            Vector3 spawnPos = _goblinPositionSpawn[randomIndex].position;
            GameObject goblinInstantiate =  Instantiate(goblin, spawnPos, Quaternion.identity);
            goblinUnits.Add(goblinInstantiate);
            goblinInstantiate.transform.parent =  _goblinPositionSpawn[randomIndex];
            _goblinPositionSpawn.RemoveAt(randomIndex);
            currentIndexGoblin++;
            
            foreach (AI ai in FindObjectsOfType<AI>())
            {
                if (ai.genericSo.Class == TypeOfWarriors.Goblin)
                {
                    //ai.SetGoblinUnits(goblinUnits);
                    ai.SetDwarfUnits(dwarfUnits);
                }
            }
        }        
    }
    private void StartBattle(Hashtable obj)
    {      
        EventTriggers.TriggerEvent(GenericEvents.RandomTargets);
        EventTriggers.TriggerEvent(GenericEvents.DisableStartButtom);
        EventTriggers.TriggerEvent(GenericEvents.ChangeToSeekState);
    }
}
