using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu, title, menuEND, menuPause;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuStop();
        }
    }

    private void Start()
    {
        mainMenu.SetActive(true);
        menuEND.SetActive(false);
        menuPause.SetActive(false);

        LeanTween.moveLocal(mainMenu, new Vector3(0f, -25f, 0f), .85f).setDelay(4f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.scale(title, new Vector3(0f, 0f, 0f), .01f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.scale(title, new Vector3(.6f, .6f, .6f), 3f).setDelay(.2f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.moveLocal(title, new Vector3(0f, 250f, 0f), 2.5f).setDelay(1.5f).setEase(LeanTweenType.easeInOutCubic);
    }

    private void menuFinal()
    {
        menuEND.SetActive(true);
        LeanTween.moveLocal(menuEND, new Vector3(0f, 0f, 0f), .75f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic);
    }

    public void menuStop()
    {
        menuPause.SetActive(true);
        Time.timeScale = 0f;
        LeanTween.moveLocal(menuPause, new Vector3(0f, 0f, 0f), .75f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic).setIgnoreTimeScale(true);
    }

    public void Continue()
    {
        menuPause.SetActive(false);
        Time.timeScale = 1f;
    }
}
