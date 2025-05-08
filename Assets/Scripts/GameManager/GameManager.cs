using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    GameObject player;
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else { Destroy(gameObject); }
    }

    //Comienzo de escena 
    public void StarGameIntro() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        player.SetActive(false);
        StartCoroutine(WaiforSeconds());

      
    }
    IEnumerator WaiforSeconds() {
        yield return new WaitForSeconds(1);
        player.SetActive(false);
        player.GetComponent<Animator>().Play("Lie_StandUp");
        yield return new WaitForSeconds(2);
        player.GetComponent<Playercontroller>().enabled = true;
        FindFirstObjectByType<WaveController>().enabled = true;
        FindFirstObjectByType<PetManager>().enabled = true;
    }
}
