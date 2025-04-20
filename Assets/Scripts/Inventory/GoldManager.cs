using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
   public static GoldManager instance;
    [SerializeField] int oroActual = 20;
    [SerializeField] TextMeshProUGUI textOro;
    [SerializeField] TextMeshProUGUI _textoOroInventory;
    private void Awake() {

        if (instance == null) {
            instance = this;
        }
        
    }
    public void AgregarOro(int cant) {
        oroActual += cant;
        ActualizarUI();
    }
    public void QuitarOro(int cant) {
        if (oroActual >= cant) {
            oroActual -= cant;
            ActualizarUI();
        }
    }
    public int obtenerOro() {
        return oroActual;
    }
    void ActualizarUI() {
        if (textOro != null) {
            _textoOroInventory.text = oroActual.ToString();
            textOro.text = oroActual.ToString();
        }
    }
}
