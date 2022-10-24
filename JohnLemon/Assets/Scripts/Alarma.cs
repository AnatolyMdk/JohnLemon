using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alarma : MonoBehaviour
{
    public static bool alarma_Activada;

    private GameObject[] ghosts, gargoyles;
    public Timer gameTime;
    public AudioSource alarma, alarma_in;
    public GameObject player;
    void Start()
    {
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        gargoyles = GameObject.FindGameObjectsWithTag("Gargoyle");
        alarma_Activada = false;
    }

    void Update() {
        if (gameTime.time < 60f && !alarma_Activada) {
            alarmaActivada();
        }
    }

    public void alarmaActivada() {
        foreach(GameObject ghost in ghosts) {
            ghost.gameObject.GetComponent<GhostStateMachine>().rage(player.transform);
        }

        alarma_Activada = true;

        alarma_in.Play();
        alarma.Play();
        // Debug.Log("Activada");
        gameTime.timerText.color = Color.red;
    }

    public void alarmaDesactivada() {
        foreach(GameObject ghost in ghosts) {
            ghost.gameObject.GetComponent<GhostStateMachine>().ChangeToInvestigation(player.transform.position);
        }
        
        alarma_Activada = false;
    }

}