using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] RectTransform mainMenu, title, menuEND, menuPause;

    [SerializeField] float scaled;
    [SerializeField] LeanTweenType type;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) {
            mainMenu.gameObject.SetActive(true);
            menuEND.gameObject.SetActive(false);
            menuPause.gameObject.SetActive(false);

            LeanTween.move(mainMenu, new Vector3(488, -140f), .85f).setDelay(4f).setEase(LeanTweenType.easeOutCubic).setIgnoreTimeScale(true);
            LeanTween.scale(title, new Vector3(0f, 0f, 0f), .01f).setEase(LeanTweenType.easeInOutSine).setIgnoreTimeScale(true);
            LeanTween.scale(title, new Vector3(.6f, .6f, .6f), 3f).setDelay(.2f).setEase(LeanTweenType.easeInOutCubic).setIgnoreTimeScale(true);
            LeanTween.move(title, new Vector3(0f, 0f), 2.5f).setDelay(1.5f).setEase(LeanTweenType.easeInOutCubic).setIgnoreTimeScale(true);
        }
        
    }

    private void menuFinal()
    {
        menuEND.gameObject.SetActive(true);
        Time.timeScale = 0f;
        LeanTween.move(menuEND, new Vector3(0f, 0f, 0f), .75f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic).setIgnoreTimeScale(true);
    }

    public void menuStop()
    {
        menuPause.gameObject.SetActive(true);
        Time.timeScale = 0f;
        LeanTween.move(menuPause, new Vector3(0f, 0f, 0f), .75f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic).setIgnoreTimeScale(true);
    }

    public void Continue()
    {
        menuPause.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UpScaleButton(GameObject button)
    {
        LeanTween.scale(button, Vector3.one * scaled, 0.1f).setEase(type).setIgnoreTimeScale(true);
    }

    public void DownScaleButton(GameObject button)
    {
        LeanTween.scale(button, Vector3.one, 0.1f).setEase(type).setIgnoreTimeScale(true);
    }
}
