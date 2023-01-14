using System;
using System.Collections.Generic;
using ShipSystems;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private ShipConfig config;

    private List<IShipSystem> shipSystems;

    void Start()
    {
        if (config == null)
        {
            throw new Exception("Ship cannot be created without a ship config");
        }
    }
}
