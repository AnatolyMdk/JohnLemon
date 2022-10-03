using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticNPC : MonoBehaviour
{
    private Sensores sensor;
    private Actuadores actuador;
    private enum Estado(Avanzar, RotarDerecha);
    private Estado estadoActual;

    void Start()
    {
        sensor = GetComponent<Sensores>();
        actuador = GetComponent<Actuadores>();
        estadoActual = Estado.AvanzarAlFrente;
    }

    void Update()
    {
        
    }
}
