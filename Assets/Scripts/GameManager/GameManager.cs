using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    [SerializeField] Button nextWave;
    [SerializeField] GameObject player;
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else { Destroy(gameObject); }
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
        yield return new WaitForSeconds(.5f);
        player.SetActive(true);
        player.GetComponent<Animator>().Play("Lie_StandUp");
        yield return new WaitForSeconds(2);
        player.GetComponent<Playercontroller>().enabled = true;
        FindFirstObjectByType<WaveController>().enabled = true;
        FindFirstObjectByType<PetManager>().enabled = true;
    }

    //Activar`botton de siguiente escena
    public void ActiveNextWavePanel() {
        nextWave.gameObject.SetActive(true);
        Button wave = nextWave.GetComponent<Button>();
        wave.onClick.AddListener(() => {
            FindFirstObjectByType<WaveController>().startNextWave();
        });
        
    }
}
