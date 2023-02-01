using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private int _maxDwarfToSpawnDwarf = 5;

    private int currentIndexGoblin = 0;
    private int _maxGoblinToSpawn = 5;

    private bool canStartBattle;

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
        if (currentIndexDwarf == 4) { EventTriggers.TriggerEvent(GenericEvents.DisableButtomDwarf); }
        if (currentIndexGoblin == 4) { EventTriggers.TriggerEvent(GenericEvents.DisableButtomGoblin); }

        if (currentIndexDwarf == 4 && currentIndexGoblin == 4) { EventTriggers.TriggerEvent(GenericEvents.TurnOnStartButtom); }       
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
            Instantiate(dwarf, spawnPos, Quaternion.identity);
            _dwarfPositionSpawn.RemoveAt(randomIndex);
            currentIndexDwarf++;
        }        
    }
    private void BuyUnitGoblin(Hashtable obj)
    {
        if (currentIndexGoblin < _maxGoblinToSpawn && _goblinPositionSpawn.Count > 0)
        {
            int randomIndex = Random.Range(0, _goblinPositionSpawn.Count);
            Vector3 spawnPos = _goblinPositionSpawn[randomIndex].position;
            Instantiate(goblin, spawnPos, Quaternion.identity);
            _goblinPositionSpawn.RemoveAt(randomIndex);
            currentIndexGoblin++;
        }        
    }

    private void StartBattle(Hashtable obj)
    {
        Debug.Log("Empieza la batalla");
        //Aca deberia Mandar un evento a generic Warrior para que se asigne el target de forrma random
        //Desabilitar el boton de Start
        //Mandar un Evento para que cambie a Seek State
    }

}
