using UnityEngine;
using TMPro;

public class WavesUI : MonoBehaviour
{
    public static WavesUI instance;
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI hpText;

    private WaveController waveController;
    private Playercontroller playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveController = GetComponent<WaveController>();
        playerController = GetComponent<Playercontroller>();
        TextUpdate();
    }
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(gameObject); }
    }

    public void TextUpdate()
    {
        waveNumberText.text = waveController.waveNumber.ToString();
        hpText.text = playerController.characterHP.ToString();
    }
}
