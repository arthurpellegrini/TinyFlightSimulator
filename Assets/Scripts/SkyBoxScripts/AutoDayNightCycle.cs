using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDayNightCycle : MonoBehaviour
{
    public float dayNightDuration = 600.0f;

    private void Update()
    {
        // Calcul de la position actuelle dans le cycle jour-nuit (valeur entre 0 et 1)
        float timeInCycle = Mathf.Repeat(Time.time, dayNightDuration) / dayNightDuration;

        // Calcul de l'angle de rotation en fonction de la position dans le cycle
        float rotationAngle = timeInCycle * 360.0f;

        // Applique la rotation à la Directional Light
        transform.rotation = Quaternion.Euler(rotationAngle, 0, 0);
    }
}
