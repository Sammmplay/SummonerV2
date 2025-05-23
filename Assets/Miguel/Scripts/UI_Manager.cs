using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager manager;

    [SerializeField] RectTransform mainMenu, title, menuEND, menuPause, endContinueButton;

    [SerializeField] float scaled;
    [SerializeField] LeanTweenType type;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            mainMenu.gameObject.SetActive(true);
            menuEND.gameObject.SetActive(false);
            menuPause.gameObject.SetActive(false);

            LeanTween.move(mainMenu, new Vector3(488, -140f), .85f).setDelay(4f).setEase(LeanTweenType.easeOutCubic).setIgnoreTimeScale(true);
            LeanTween.scale(title, new Vector3(0f, 0f, 0f), .01f).setEase(LeanTweenType.easeInOutSine).setIgnoreTimeScale(true);
            LeanTween.scale(title, new Vector3(.6f, .6f, .6f), 3f).setDelay(.2f).setEase(LeanTweenType.easeInOutCubic).setIgnoreTimeScale(true);
            LeanTween.move(title, new Vector3(0f, 0f), 2.5f).setDelay(1.5f).setEase(LeanTweenType.easeInOutCubic).setIgnoreTimeScale(true);
        }
        
    }

    public void menuFinal()
    {
        menuEND.gameObject.SetActive(true);
        endContinueButton.gameObject.SetActive(false);
        Time.timeScale = 0f;
        LeanTween.move(menuEND, new Vector3(0f, 0f), .75f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic).setIgnoreTimeScale(true);
    }

    public void menuLose()
    {
        menuEND.gameObject.SetActive(true);
        Time.timeScale = 0f;
        LeanTween.move(menuEND, new Vector3(0f, 0f), .75f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic).setIgnoreTimeScale(true);
    }
    public void menuStop()
    {
        menuPause.gameObject.SetActive(true);
        Time.timeScale = 0f;
        LeanTween.move(menuPause, new Vector3(0f, 0f), .75f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic).setIgnoreTimeScale(true);
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

    public void DownScaleButton(GameObject button) {
        LeanTween.scale(button, Vector3.one, 0.1f).setEase(type).setIgnoreTimeScale(true);
    }
    public void ContinueNextWavePointerEnter(RectTransform button) {
        LeanTween.move(button, new Vector2(-310, 0), .5f).setIgnoreTimeScale(true);
    }
    public void RestarPositionButtonNextWave(RectTransform button) {
        LeanTween.move(button, new Vector2(0, 0), .5f).setOnComplete(()=>button.gameObject.SetActive(false)).setIgnoreTimeScale(true);
    }
}
