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

    public void OnEnableListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.DisableButtomDwarf, DisableButtomDwarf);
        EventManager.StartListening(GenericEvents.DisableButtomGoblin, DisableButtomGoblin);
        EventManager.StartListening(GenericEvents.TurnOnStartButtom, TurnOnStartButtom);
    }

    private void TurnOnStartButtom(Hashtable obj)
    {
        startButtom.SetActive(true);
    }

    public void OnDisableListenerSubscriptions()
    {
        EventManager.StopListering(GenericEvents.DisableButtomDwarf, DisableButtomDwarf);
        EventManager.StopListering(GenericEvents.DisableButtomGoblin, DisableButtomGoblin);
    }

    private void DisableButtomDwarf(Hashtable obj)
    {
        dwarfButtom.SetActive(false);
    }

    private void DisableButtomGoblin(Hashtable obj)
    {
        goblinButton.SetActive(false);
    }
}
