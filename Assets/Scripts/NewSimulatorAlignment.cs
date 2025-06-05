using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using Unity.XR.CoreUtils;

public class NewSimulatorAlignment : MonoBehaviour
{
    [SerializeField] private XRDeviceSimulator simulator;
    [SerializeField] private XROrigin xrOrigin;

    void Update()
    {
        if (simulator == null || !simulator.enabled || xrOrigin == null || xrOrigin.Camera == null)
            return;

        // 1. Sincronizza la POSIZIONE (funziona sempre)
        xrOrigin.transform.position = simulator.transform.position;

        // 2. Sincronizza la ROTAZIONE - Metodo alternativo
        // Opzione A: Se il simulatore ha una camera figlia diretta
        Camera simulatedCamera = simulator.GetComponentInChildren<Camera>();
        if (simulatedCamera != null)
        {
            xrOrigin.Camera.transform.rotation = simulatedCamera.transform.rotation;
        }
        // Opzione B: Usa la rotazione del simulatore stesso
        else
        {
            xrOrigin.Camera.transform.rotation = simulator.transform.rotation;
        }
    }
}