using Ships;
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

            speedText.text = targetShip.engineSystem.Speed + " m/s";
            throttleText.text = targetShip.engineSystem.Throttle + " %";
            enginePowerText.text = (targetShip.engineSystem.CurrentPower() / targetShip.Config.energyCapacity) + " %";
            shieldPowerText.text = (targetShip.shieldSystem.CurrentPower() / targetShip.Config.energyCapacity) + " %";
            weaponPowerText.text = (targetShip.weaponSystem.CurrentPower() / targetShip.Config.energyCapacity) + " %";
        }
    }
}
