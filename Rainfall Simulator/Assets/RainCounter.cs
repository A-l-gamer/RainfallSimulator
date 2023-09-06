using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RainCounter : MonoBehaviour
{
    bool isSimulating;

    float rainCounter;
    [SerializeField] TMP_Text rainCounterOutput;
    [SerializeField] GameObject startSimulationButton;
    [SerializeField] GameObject endSimulationButton;

    [Space]
    [SerializeField] GameObject rainEmitter;
    ParticleSystem emitter;
    
    [Space]
    [SerializeField] CharacterController controller;

    [Space]
    [SerializeField] TMP_InputField movementInput;
    Vector3 movement;

    [Space]
    [SerializeField] TMP_InputField distanceToTravelInput;
    float distanceToTravel = 100f;

    [Space]
    [SerializeField] Slider angleInput;
    Vector3 angle;

    [Space]
    [SerializeField] Slider timeInput;

    [Space]
    [SerializeField] TMP_InputField densityInput;
    float raindropsNumber;

    private void Start()
    {
        emitter = rainEmitter.GetComponent<ParticleSystem>();
    }

    public void StartSimulation()
    {
        rainCounter = 0;
        isSimulating = true;
        startSimulationButton.SetActive(false);
        endSimulationButton.SetActive(true);

        float movementSpeed = float.Parse(movementInput.text);
        movement = new Vector3(0, 0, movementSpeed);

        distanceToTravel = float.Parse(distanceToTravelInput.text);
        Debug.Log(distanceToTravel);
        Vector3 resetPosition = new Vector3(0, 0, -distanceToTravel / 2f);
        controller.Move(resetPosition);

        float timeSpeed = Mathf.Pow(2f, timeInput.value);
        Time.timeScale = timeSpeed;

        raindropsNumber = float.Parse(densityInput.text);
        var emissionVar = emitter.emission;
        emissionVar.rateOverTime = NumberOfRaindrops(raindropsNumber);
    }

    private void Update()
    {
        float angleToFloat = angleInput.value;
        angle = new Vector3(angleToFloat - 90f, 0, 0);
        rainEmitter.transform.localRotation = Quaternion.Euler(angle);

        if (isSimulating)
        {
            controller.Move(movement * Time.deltaTime);
            rainCounterOutput.text = (Mathf.Round(rainCounter * 10000.0f) * 0.0001f).ToString();
        }

        if(transform.position.z >= distanceToTravel/2)
        {
            EndSimulation();
            Debug.Log("End");
        }
    }

    int NumberOfRaindrops(float mmRain)
    {
        float volumeOfArea = 55f * 6f * mmRain;
        float volumeWaterDrop = 0.00005f;

        float numberOfWaterDropsHourly = volumeOfArea / volumeWaterDrop;
        float numberOfWaterDrops = numberOfWaterDropsHourly / 3600f;
        Debug.Log(numberOfWaterDrops);

        return Mathf.RoundToInt(numberOfWaterDrops);
    }

    private void OnParticleCollision()
    {
        if (isSimulating) rainCounter += 0.05f;
    }

    public void EndSimulation()
    {
        isSimulating = false;
        endSimulationButton.SetActive(false);
        startSimulationButton.SetActive(true);

        transform.position = new Vector3(0, 1, 0f);
    }
}
