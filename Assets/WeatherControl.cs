using UnityEngine;

public class WeatherControl : MonoBehaviour
{
    public ParticleSystem rainParticles;
    public ParticleSystem snowParticles;

    private float timeSinceLastRandomization = 0.0f;
    private float randomizationIntervalMin = 60.0f;  // 1 minute
    private float randomizationIntervalMax = 150.0f; // 2 minutes 30 secondes
    private float transitionDuration = 30.0f; // Durée de la transition en secondes

    private int[][] intensityRanges = {
        new int[] {50, 150, -4},
        new int[] {250, 400, -2},
        new int[] {500, 700, 0},
        new int[] {800, 1000, 2}
    };
    
    void Start()
    {
        // Désactivez initialement les deux systèmes de particules
        if (rainParticles != null) rainParticles.Stop();
        if (snowParticles != null) snowParticles.Stop();

        // Commencez avec une météo aléatoire
        RandomizeWeather();
    }

    void Update()
    {
        // Mettez à jour le temps depuis la dernière randomisation
        timeSinceLastRandomization += Time.deltaTime;

        // Vérifiez si le temps écoulé dépasse l'intervalle de randomisation
        if (timeSinceLastRandomization >= Random.Range(randomizationIntervalMin, randomizationIntervalMax))
        {
            // Réinitialisez le compteur
            timeSinceLastRandomization = 0.0f;

            // Randomisez la météo
            RandomizeWeather();
        }
    }

    void RandomizeWeather()
    {
        if (rainParticles != null) rainParticles.Stop();
        if (snowParticles != null) snowParticles.Stop();

        // Nombre aléatoire entre 0 et 99 pour déterminer la météo avec ce ratio (60% beau temps, 30% pluie, 10% neige)
        int weatherChance = Random.Range(0, 100);

        if (weatherChance < 60)
        {
            // Beau temps (65% de chance)
            Debug.Log("Beau temps !");

            CloudMaster cloudMasterScript = FindObjectOfType<CloudMaster>();
            if (cloudMasterScript != null)
            {
                float randomDensityOffset = Random.Range(-7.0f, -4.0f);
                cloudMasterScript.densityOffset = randomDensityOffset;
            }
        }
        else if (weatherChance < 90)
        {
            // Pluie (25% de chance)
            if (rainParticles != null)
            {
                int intensity = Random.Range(0, 3);
                int[] selectedRangeIntesity = intensityRanges[intensity];
                SetRainIntensity(Random.Range(selectedRangeIntesity[0], selectedRangeIntesity[1])); // permet d'avoir une pluie plus ou moins forte
                rainParticles.Play();

                CloudMaster cloudMasterScript = FindObjectOfType<CloudMaster>();
                if (cloudMasterScript != null)
                {
                    cloudMasterScript.densityOffset = selectedRangeIntesity[2] / 2;
                }
            }
            Debug.Log("Il pleut ! Il mouille c'est la fête à la grenouille");
        }
        else
        {
            // Neige (10% de chance)
            if (snowParticles != null)
            {
                int intensity = Random.Range(0, 3);
                int[] selectedRangeIntesity = intensityRanges[intensity];
                SetSnowIntensity(Random.Range(selectedRangeIntesity[0], selectedRangeIntesity[1])); // permet d'avoir une neige plus ou moins forte
                snowParticles.Play();

                CloudMaster cloudMasterScript = FindObjectOfType<CloudMaster>();
                if (cloudMasterScript != null)
                {
                    cloudMasterScript.densityOffset = selectedRangeIntesity[2]/2;
                }
            }
            Debug.Log("Il neige ! Maman il pleut blanc !");
        }
    }

    void SetRainIntensity(float intensity)
    {
        var emissionModule = rainParticles.emission;
        emissionModule.rateOverTime = intensity;
    }

    void SetSnowIntensity(float intensity)
    {
        var emissionModule = snowParticles.emission;
        emissionModule.rateOverTime = intensity;
    }

}
