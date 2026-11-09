using UnityEngine;

public class Moneda : ObjetoRecolectable
{
    [Header("Configuración de Moneda")]
    [SerializeField] private int puntosQueOtorga = 100;
    
    [Header("Efectos de Sonido")]
    [SerializeField] private AudioClip sonidoMoneda;

    protected override void AplicarEfecto(GameObject jugador)
    {
        if (ScoreManager.Instancia != null)
        {
            ScoreManager.Instancia.SumarPuntos(puntosQueOtorga);
        }

        SaludPlayer saludPlayer = jugador.GetComponent<SaludPlayer>();
        if (saludPlayer != null)
        {
            saludPlayer.ActualizarHUD();
        }

        if (sonidoMoneda != null)
        {
            GameObject tempAudio = new GameObject("SonidoMonedaTemp");
            tempAudio.transform.position = transform.position;
            AudioSource fuenteTemp = tempAudio.AddComponent<AudioSource>();
            fuenteTemp.clip = sonidoMoneda;
            fuenteTemp.Play();
            Destroy(tempAudio, sonidoMoneda.length);
        }
    }
}