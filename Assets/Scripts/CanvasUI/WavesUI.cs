using UnityEngine;
using TMPro;

public class WavesUI : MonoBehaviour
{
    public static WavesUI instance;
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI hpText;

    private WaveController2 waveController;
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
        waveController = FindFirstObjectByType<WaveController2>();
        playerController = FindFirstObjectByType<Playercontroller>();
        TextUpdate();
    }
    private void Update()
    {
        if (playerController.characterHP <= 0)
        {
            losePanel.SetActive(true);
        }
    }

    public void TextUpdate()
    {
        waveNumberText.text = "Wave: " + waveController.waveNumber.ToString();
        hpText.text = "HP: " + playerController.characterHP.ToString();
    }
}