using Ships;
using Ships.ShipSystems;
using TMPro;
using UnityEngine;

namespace UI.ShipUI
{
    public class ShipUI : MonoBehaviour
    {
        [SerializeField]
        private Ship targetShip;

        [SerializeField] private TMP_Text speedText;
        [SerializeField] private TMP_Text throttleText;
        [SerializeField] private TMP_Text enginePowerText;
        [SerializeField] private TMP_Text shieldPowerText;
        [SerializeField] private TMP_Text weaponPowerText;
        
        void Update()
        {
            if (targetShip == null)
            {
                return;
            }

            speedText.text = Mathf.RoundToInt(targetShip.engineSystem.Speed) + " m/s";
            throttleText.text = Mathf.RoundToInt(targetShip.engineSystem.Throttle * 100f) + " %";
            enginePowerText.text = PowerPercent(targetShip.engineSystem);
            shieldPowerText.text = PowerPercent(targetShip.shieldSystem);
            weaponPowerText.text = PowerPercent(targetShip.weaponSystem);
        }

        private string PowerPercent(IShipSystem system)
        {
            return Mathf.RoundToInt((system.CurrentPower() / targetShip.Config.energyCapacity) * 100f) +" %";
        }
    }
}
