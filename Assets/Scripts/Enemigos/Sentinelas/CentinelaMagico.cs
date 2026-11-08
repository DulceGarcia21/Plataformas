using UnityEngine;

public class CentinelaMagico : EnemigoCentinela
{
    [Header("Configuración de Disparo")]
    [SerializeField] private GameObject prefabMagia;
    [SerializeField] private Transform puntoDisparo;

    [Header("Multiplicadores (Mágico)")]
    public int multiplicadorDistancia = 2;
    public int divisorFisico = 1;

    protected override void Start()
    {
        base.Start(); // Ejecuta la configuración base

        // CORRECCIÓN VISUAL: Como el sprite del Moradito SÍ mira a la derecha por defecto,
        // deshacemos el giro negativo que heredó del Orco.
        if (moviendoADerecha)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public override void TomarDanioDistancia(int cantidad)
    {
        int danioFinal = cantidad * multiplicadorDistancia;
        base.TomarDanioDistancia(danioFinal);
        Debug.Log($"<color=magenta>¡Golpe Crítico Mágico!</color> El enemigo recibió {danioFinal} de daño.");
    }

    public override void TomarDanio(int cantidad)
    {
        int danioFinal = cantidad / divisorFisico;
        base.TomarDanio(danioFinal);
    }

    // SOBRESCRIBIMOS EL ATAQUE: En vez de daño directo, crea un proyectil
    protected override void Atacar()
    {
        // Ejecuta la animación de ataque
        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }

        // Dispara la magia
        if (prefabMagia != null && puntoDisparo != null)
        {
            GameObject magia = Instantiate(prefabMagia, puntoDisparo.position, Quaternion.identity);
            MagiaEnemiga scriptMagia = magia.GetComponent<MagiaEnemiga>();

            if (scriptMagia != null)
            {
                // Le dice a la magia en qué dirección volar dependiendo de a dónde mira el enemigo
                scriptMagia.direccion = moviendoADerecha ? 1 : -1;
            }
        }
    }
}