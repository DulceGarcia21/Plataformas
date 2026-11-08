using UnityEngine;

public abstract class ObjetoRecolectable : MonoBehaviour
{

    protected abstract void AplicarEfecto(GameObject jugador);

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            AplicarEfecto(collision.gameObject);


            Destroy(gameObject);
        }
    }
}