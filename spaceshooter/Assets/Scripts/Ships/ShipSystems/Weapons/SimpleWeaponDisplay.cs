using System;
using UnityEngine;

namespace Ships.ShipSystems.Weapons
{
    public class SimpleWeaponDisplay : MonoBehaviour
    {
        [SerializeField] private LineRenderer laserLine;
        [SerializeField] private ParticleSystem particles;

        private float timeSinceFired = 0f;

        private void Update()
        {
            timeSinceFired -= Time.deltaTime;
            if (timeSinceFired <= 0f)
            {
                laserLine.enabled = false;
            }
        }

        public void ShowShot(Vector3 source, Vector3 destination, bool hit)
        {
            laserLine.enabled = true;
            laserLine.SetPosition(0, source);
            laserLine.SetPosition(1, destination);
            timeSinceFired = .1f;
            if (hit)
            {
                particles.transform.position = destination;
                particles.Emit(100);
            }
        }
    }
}
