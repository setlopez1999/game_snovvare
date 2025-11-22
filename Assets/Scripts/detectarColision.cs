using System.Collections;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class detectarColision : MonoBehaviour
{
    private Vector3 initialPosition;

    public TextMeshProUGUI textoBonus;
    public int puntos = 0;
    float vida = 100;
    public float llaves = 0;
    public Image barraVida;
    public AudioSource efecto;
    public AudioSource moneda;
    public AudioSource curasonido;

    public Personaje personaje; 
    public float velocidadIncremento = 2f;
    public float duracionVelocidad = 3f; 

    public Enemigo enemigo; 
    public float tiempoFuerza = 5f; 

    void Start()
    {
        textoBonus.text = "0";
        barraVida.fillAmount = 1;
        initialPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "cura")
        {
            curasonido.Play();
            vida += 20;
            if (vida >= 100)
            {
                vida = 100;
            }
            barraVida.fillAmount = vida / 100;
        }

        if (collision.collider.tag == "Cubos")
        {
            Destroy(collision.collider.gameObject);
        }
        
        if (collision.collider.tag == "Llave")
        {
            llaves++;
            if(llaves == 1){
                GameObject llegada = GameObject.FindWithTag("A");
                if (llegada != null)
                {
                    Destroy(llegada);
                }
            }
            if (llaves == 2)
            {
                // Busca el objeto con el tag "Llegada"
                GameObject llegada2 = GameObject.FindWithTag("B");
                // Si se encuentra el objeto, destrúyelo
                if (llegada2 != null)
                {
                    Destroy(llegada2);
                }
            }
            // Destruye la llave recogida
            Destroy(collision.collider.gameObject);
        }




        if (collision.collider.tag == "Moneda")
        {
            Destroy(collision.collider.gameObject);
            puntos++;
            textoBonus.text = puntos.ToString();
            moneda.Play();

            if (puntos >= 12)
            {
                SceneManager.LoadScene(1);
            }
        }

        if (collision.collider.tag == "Enemigo")
        {
            vida -= 10;
            barraVida.fillAmount = vida / 100;

            if (efecto != null)
            {
                efecto.Play();
            }

            transform.position = initialPosition;

            if (vida <= 0)
            {
                SceneManager.LoadScene(2);
            }
        }

        if (collision.collider.tag == "Velocidad")
        {
            StartCoroutine(AumentarVelocidad());
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.tag == "Fuerza")
        {
            if (enemigo != null)
            {
                enemigo.DeshabilitarPersecucion();
            }

            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.name == "Llegada")
        {
            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator AumentarVelocidad()
    {
        if (personaje != null)
        {
            personaje.velocidadMovimiento *= velocidadIncremento;

            yield return new WaitForSeconds(duracionVelocidad);

            personaje.velocidadMovimiento /= velocidadIncremento;
        }
    }
}
