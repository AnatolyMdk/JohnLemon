using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alarma : MonoBehaviour
{
    public static bool alarma_Activada;
    public AudioSource alarma_s, alarma_in_s;
    public Timer gameTime;
    private GameObject[] ghosts;
    public GameObject player;
    void Start()
    {
        alarma_Activada = false;
    }

    void Update()
    {
        if(gameTime.time < 60f == !alarma_Activada) {
            activarAlarma();
        }
    }

    private void activarAlarma() {
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in ghosts) {
            GhostStateMachine ghostState = ghost.gameObject.GetComponent<GhostStateMachine>();
            ghostState.rage(player.transform);
        }

        alarma_Activada = true;

        gameTime.timerText.color = Color.red;

        alarma_in_s.Play();
        alarma_s.Play();
    }

    private void desactivarAlarma() {
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in ghosts) {
            GhostStateMachine ghostState = ghost.gameObject.GetComponent<GhostStateMachine>();
            ghostState.ChangeToInvestigation(player.transform.position);
        }

        alarma_Activada = false;

        gameTime.timerText.color = Color.white;
    }
}
