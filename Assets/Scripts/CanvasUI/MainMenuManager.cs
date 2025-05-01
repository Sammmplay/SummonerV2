using SD;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    public GameObject _mainMenu;
    public GameObject _menuPause;
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
                break;
                case 1:
                _mainMenu.SetActive(false);
                _menuPause.SetActive(false);
                break;
        }
    }
    public void Jugar() {
        LoadEscenChangeManager.instance.LoadEscene(1);
    }
}
