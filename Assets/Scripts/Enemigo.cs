using System.Collections;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public Transform player; // Transform del jugador
    public float followRange = 30f; // Rango en el que el enemigo sigue al jugador
    public float areaLimit = 50f; // Tamaño del área en el que el enemigo puede moverse
    public float speed = 5f; // Velocidad del enemigo al seguir al jugador
    public float speedAlejamiento = 7f; // Velocidad del enemigo al alejarse del jugador

    private Vector3 initialPosition; // Posición inicial del enemigo
    private bool perseguirJugador = true; // Controla si el enemigo sigue al jugador
    private bool alejarse = false; // Controla si el enemigo debe alejarse del jugador
    private bool regresandoAPosicionInicial = false; // Controla si el enemigo está volviendo a su posición inicial

    void Start()
    {
        // Guarda la posición inicial del enemigo
        initialPosition = transform.position;
    }

    void Update()
    {
        float minHeight = Terrain.activeTerrain.SampleHeight(transform.position);

        if (transform.position.y < minHeight)
        {
            // Corrige la posición para que se mantenga sobre el terreno
            transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
        }
        // Si está en modo alejamiento, se aleja del jugador
        if (alejarse)
        {
            Vector3 direccionContraria = (transform.position - player.position).normalized;
            transform.LookAt(transform.position + direccionContraria);
            transform.position += direccionContraria * speedAlejamiento * Time.deltaTime;

            // Si el enemigo se alejó más allá del área límite, detiene el alejamiento
            if (Vector3.Distance(transform.position, player.position) >= areaLimit)
            {
                alejarse = false; // Detenemos el alejamiento
            }
            return;
        }

        // Si está regresando a la posición inicial
        if (regresandoAPosicionInicial)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
            transform.LookAt(new Vector3(initialPosition.x, transform.position.y, initialPosition.z));

            // Si llegó a la posición inicial, espera antes de reanudar la persecución
            if (Vector3.Distance(transform.position, initialPosition) < 0.5f)
            {
                regresandoAPosicionInicial = false;
                StartCoroutine(EsperarAntesDePerseguir());
            }
            return;
        }

        // Si debe perseguir al jugador
        if (perseguirJugador)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            float distanceToInitial = Vector3.Distance(transform.position, initialPosition);

            if (distanceToPlayer <= followRange && distanceToInitial <= areaLimit)
            {
                // Mira y sigue al jugador
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            else
            {
                // Si está fuera de rango, regresa a la posición inicial
                transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
                transform.LookAt(new Vector3(initialPosition.x, transform.position.y, initialPosition.z));
            }
        }
    }

    // Método para deshabilitar temporalmente la persecución
    public void DeshabilitarPersecucion()
    {
        alejarse = true; // Activa el modo alejamiento
        perseguirJugador = false; // Detiene la persecución temporalmente
        regresandoAPosicionInicial = false; // Asegúrate de que no regrese a su posición inicial
        StartCoroutine(EsperarAntesDeReanudar()); // Inicia la cuenta regresiva para reanudar la persecución
    }

    // Corrutina para esperar 8 segundos antes de reanudar la persecución
    private IEnumerator EsperarAntesDeReanudar()
    {
        yield return new WaitForSeconds(8f); // Espera 8 segundos
        alejarse = false; // Detenemos el alejamiento
        regresandoAPosicionInicial = false; // Previene el regreso inmediato
        perseguirJugador = true; // Permite que vuelva a perseguir
    }

    // Corrutina para esperar un tiempo antes de reanudar la persecución
    private IEnumerator EsperarAntesDePerseguir()
    {
        // Espera 8 segundos antes de volver a perseguir
        yield return new WaitForSeconds(8f);
        perseguirJugador = true; // Reactiva la persecución
    }
}
