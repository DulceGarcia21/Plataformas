using UnityEngine;

public class Corazon : ObjetoRecolectable
{
    [Header("Configuración de Curación")]
    [SerializeField] private int corazonesQueCura = 1;

    protected override void AplicarEfecto(GameObject jugador)
    {
        SaludPlayer saludPlayer = jugador.GetComponent<SaludPlayer>();
        if (saludPlayer != null)
        {
            saludPlayer.Curar(corazonesQueCura);
        }
    }
}