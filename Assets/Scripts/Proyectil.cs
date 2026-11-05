using UnityEngine;

public class Proyectil : MonoBehaviour
{
    [SerializeField] private float velocidad = 8.0f;
    void FixedUpdate() {
        if (CAD.dirDisparo == 1) { //Derecha
            transform.position += new Vector3(1, 0, 0) * Time.deltaTime * velocidad;
        } else if (CAD.dirDisparo == 2) { //Izquierda
            transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * velocidad;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "limites") {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "enemigo") {
            //collision.transform.GetComponent<Enemigo>().TomarDaño(1);
            Destroy(this.gameObject);
        }
    }
}
