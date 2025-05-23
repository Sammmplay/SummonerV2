using SD;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    public GameObject _mainMenu;
    public GameObject _menuPause;
    public GameObject _menuEnd;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    public void Inicializar() {
        int index = SceneManager.GetActiveScene().buildIndex;
        switch (index) {
            case 0:
                _mainMenu.SetActive(true);
                _menuPause.SetActive(false);
                _menuEnd.SetActive(false);
                break;
                case 1:
                _mainMenu.SetActive(false);
                _menuPause.SetActive(false);
                _menuEnd.SetActive(false);
                GameManager.instance.StarGameIntro();
                break;
        }
    }
    public void Jugar() {
        LoadEscenChangeManager.instance.LoadEscene(1);
        
    }
    public void Exit() {
        Application.Quit();
    }
    public void Restar() {
        LoadEscenChangeManager.instance.LoadEscene(0);
    }
}
