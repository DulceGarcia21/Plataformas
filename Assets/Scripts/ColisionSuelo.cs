using UnityEngine;

public class ColisionSuelo : MonoBehaviour
{
    public static bool estaEnSuelo = true;

    // Usamos Stay en lugar de Enter para que sea más estable con los Tilemaps
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("suelo"))
        {
            estaEnSuelo = true;
        }
    }

    // Es vital detectar cuando el personaje YA NO toca el suelo
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("suelo"))
        {
            estaEnSuelo = false;
        }
    }
}