using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class UIManager : MonoBehaviour , IEventListener
{
    [SerializeField]
    private GameObject dwarfButtom, goblinButton,startButtom;

    private void Awake()
    {
        OnEnableListenerSubscriptions();
    }
    public void BuyDwarfEvent()
    {
        EventManager.TriggerEvent(GenericEvents.BuyUnitDwarf);
    }
    public void BuyGoblinEvent()
    {
        EventManager.TriggerEvent(GenericEvents.BuyUnitGoblin);
    }
    public void StartEvent()
    {
        EventManager.TriggerEvent(GenericEvents.StartBattle);
    }


    public void OnEnableListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.DisableButtomDwarf, DisableButtomDwarf);
        EventManager.StartListening(GenericEvents.DisableButtomGoblin, DisableButtomGoblin);
        EventManager.StartListening(GenericEvents.TurnOnStartButtom, TurnOnStartButtom);
        EventManager.StartListening(GenericEvents.DisableStartButtom, TurnOfStartButtom);
    }  
    public void OnDisableListenerSubscriptions()
    {
        EventManager.StopListering(GenericEvents.DisableButtomDwarf, DisableButtomDwarf);
        EventManager.StopListering(GenericEvents.DisableButtomGoblin, DisableButtomGoblin);
        EventManager.StopListering(GenericEvents.TurnOnStartButtom, TurnOnStartButtom);
        EventManager.StopListering(GenericEvents.DisableStartButtom, TurnOfStartButtom);
    }
    private void DisableButtomDwarf(Hashtable obj)
    {
        dwarfButtom.SetActive(false);
    }
    private void DisableButtomGoblin(Hashtable obj)
    {
        goblinButton.SetActive(false);
    }
    private void TurnOnStartButtom(Hashtable obj)
    {
        startButtom.SetActive(true);
    }
    private void TurnOfStartButtom(Hashtable obj)
    {
        EventManager.StopListering(GenericEvents.TurnOnStartButtom, TurnOnStartButtom);        
        startButtom.SetActive(false);
    }

}
