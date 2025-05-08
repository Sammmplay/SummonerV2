using UnityEngine;
using TMPro;

public class WavesUI : MonoBehaviour
{
    public static WavesUI instance;
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI hpText;

    private WaveController waveController;
    private Playercontroller playerController;
    public GameObject losePanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        waveController = FindFirstObjectByType<WaveController>();
        playerController = FindFirstObjectByType<Playercontroller>();
        TextUpdate();
    }
    private void Update()
    {
        /*if (playerController.currentCharacterHP <= 0)
        {
            losePanel.SetActive(true);
        }*/
    }

    public void TextUpdate()
    {
        waveNumberText.text = "Wave: " + waveController.waveNumber.ToString();
        hpText.text = "HP: " + playerController.currentCharacterHP.ToString() + "/" + playerController.characterHP.ToString();
    }
}