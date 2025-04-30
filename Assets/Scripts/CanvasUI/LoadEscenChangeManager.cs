using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace SD
{
    public class LoadEscenChangeManager : MonoBehaviour
    {
        public static LoadEscenChangeManager instance;
        public GameObject EsceneMenu;
        [SerializeField] Slider Carga;
        [SerializeField] TextMeshProUGUI _progressText;
        private void Awake()
        {
            #region instancie..
            if (instance == null)
            {
                instance = this;
            }
            else Destroy(gameObject);
            #endregion
            
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            EsceneMenu.SetActive(false);
        }

        public void LoadEscene(int index)
        {
            
            StartCoroutine(ChanchEscene(index));          
        }

        IEnumerator ChanchEscene(int index)
        {
            EsceneMenu.SetActive(true);
            //cargar la escena de forma asyncrona
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);

            //esperar a que la escene se cargue completamente
            //mientras la operacion no esta completa, actualiza el valor del slider
            while (!asyncOperation.isDone)
            {
                float progrees = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                Carga.value = progrees;
                float porcentaje = progrees * 100;
                _progressText.text = porcentaje.ToString() + "%";
                yield return null;
            }
            //esperar 0.5segundos adicionales despues de que la carga este completada
            yield return new WaitForSeconds(0.5f);
            //desactivar el objeto 
            MainMenuManager.Instance.Inicializar();
            EsceneMenu.SetActive(false);
            
        }

    }
}
