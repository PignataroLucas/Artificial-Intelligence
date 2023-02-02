using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridEntity 
{    
    event Action <IGridEntity> OnEntityAdded;

    Vector3 Position { get; set; }
}
