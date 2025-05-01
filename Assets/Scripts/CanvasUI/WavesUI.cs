using UnityEngine;
using TMPro;

public class WavesUI : MonoBehaviour
{
    public static WavesUI instance;
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI hpText;

    public WaveController waveController;
    public Playercontroller playerController;

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

    public void TextUpdate()
    {
        waveNumberText.text = "Wave: " + waveController.waveNumber.ToString();
        hpText.text = "HP: " + playerController.characterHP.ToString();
    }
}