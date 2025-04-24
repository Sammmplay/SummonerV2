using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu, title;

    private void Start()
    {
        LeanTween.moveLocal(mainMenu, new Vector3(450f, 0f, 0f), .5f).setDelay(5f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.scale(title, new Vector3(4f, 4f, 4f), 2f).setEase(LeanTweenType.easeInCubic);
        LeanTween.moveLocal(title, new Vector3(0f, 330f, 0f), 2.7f).setDelay(2f).setEase(LeanTweenType.easeInOutCubic);
    }
}
