using UnityEngine;

public class CentinelaMagico : EnemigoCentinela
{
    [Header("Multiplicadores (Mágico)")]
    public int multiplicadorDistancia = 2; // Recibe el doble de daño de tus balas/magia
    public int divisorFisico = 1; // Puedes hacer que reciba menos daño físico si lo deseas

    // Cuando le disparas con tu ataque a distancia (CAD)
    public override void TomarDanioDistancia(int cantidad)
    {
        int danioFinal = cantidad * multiplicadorDistancia;
        base.TomarDanioDistancia(danioFinal);
        Debug.Log($"<color=magenta>¡Golpe Crítico Mágico!</color> El enemigo recibió {danioFinal} de daño.");
    }

    // Cuando lo golpeas con tu espada/cuerpo a cuerpo (CCC)
    public override void TomarDanio(int cantidad)
    {
        // Daño normal (o reducido) al atacarlo cuerpo a cuerpo
        int danioFinal = cantidad / divisorFisico;
        base.TomarDanio(danioFinal);
    }
}