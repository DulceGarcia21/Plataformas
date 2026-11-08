using UnityEngine;
using TMPro;

public class GestorVictoria : MonoBehaviour
{
    [Header("Configuración de UI")]
    [SerializeField] private TextMeshProUGUI textoPuntajeFinal;

    // Se ejecuta automáticamente al momento de encender la pantalla de victoria
    private void OnEnable()
    {
        ActualizarPuntajeFinal();
    }

    public void ActualizarPuntajeFinal()
    {
        // Verificamos que todo esté conectado antes de intentar mostrar los puntos
        if (textoPuntajeFinal != null && ScoreManager.Instancia != null)
        {
            int puntos = ScoreManager.Instancia.GetPuntos();

            // Puedes cambiar el texto entre comillas por lo que quieras que diga
            textoPuntajeFinal.text = $"PUNTAJE FINAL: {puntos:D6}";
        }
        else
        {
            Debug.LogWarning("<b>[UI VICTORIA]</b> Falta asignar el texto o no se encontró el ScoreManager.");
        }
    }
}