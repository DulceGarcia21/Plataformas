using UnityEngine;

public class CentinelaFisico : EnemigoCentinela
{
    [Header("Multiplicadores (Físico)")]
    public int multiplicadorFisico = 2; 
    public int divisorDistancia = 1; 


    public override void TomarDanio(int cantidad)
    {
        int danioFinal = cantidad * multiplicadorFisico;
        base.TomarDanio(danioFinal);
        Debug.Log($"<color=red>¡Golpe Crítico Físico!</color> El enemigo recibió {danioFinal} de daño.");
    }

   
    public override void TomarDanioDistancia(int cantidad)
    {
      
        int danioFinal = cantidad / divisorDistancia;
        base.TomarDanioDistancia(danioFinal);
    }
}