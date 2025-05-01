using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu, title;
    
    private void Start()
    {
        LeanTween.moveLocal(mainMenu, new Vector3(0f, -25f, 0f), .85f).setDelay(5f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.scale(title, new Vector3(0f, 0f, 0f), .01f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.scale(title, new Vector3(.6f, .6f, .6f), 3f).setDelay(.2f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.moveLocal(title, new Vector3(0f, 250f, 0f), 2.5f).setDelay(1.5f).setEase(LeanTweenType.easeInOutCubic);

    }
}
