using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instancia { get; private set; }

    private int puntosTotales = 0;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SumarPuntos(int cantidad)
    {
        puntosTotales += cantidad;
        Debug.Log($"<color=yellow>[PUNTOS]</color> +{cantidad} | Total: {puntosTotales}");
    }

    public int GetPuntos() => puntosTotales;
}