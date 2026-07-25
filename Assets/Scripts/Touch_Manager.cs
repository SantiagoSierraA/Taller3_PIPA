using UnityEngine;
using UnityEngine.EventSystems;

public class Touch_Manager : MonoBehaviour
{
    [Header("Sensibilidad")]
    public float umbralSwipe = 80f;         
    public float tiempoMaximoToque = 0.6f; 
    public float distanciaRaycast = 100f;  

    private Vector2 posicionInicial;    
    private float tiempoInicial;           
    private bool tocando = false; 

    // Update se ejecuta una vez por frame: lee la entrada táctil o del mouse
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch dedo = Input.GetTouch(0);

            if (dedo.phase == TouchPhase.Began)
                iniciarToque(dedo.position);
            else if (dedo.phase == TouchPhase.Ended)
                terminarToque(dedo.position);
        }
        else
        {
            // respaldo con mouse para poder probar la aplicación en el editor
            if (Input.GetMouseButtonDown(0))
                iniciarToque(Input.mousePosition);
            else if (Input.GetMouseButtonUp(0))
                terminarToque(Input.mousePosition);
        }
    }

    // Guarda la posición y el momento en que se inició el contacto
    void iniciarToque(Vector2 posicion)
    {
        posicionInicial = posicion;
        tiempoInicial = Time.time;
        tocando = true;
    }

    // Al soltar, decide si el gesto fue un deslizamiento o un toque
    void terminarToque(Vector2 posicionFinal)
    {
        if (!tocando) return;
        tocando = false;

        Vector2 desplazamiento = posicionFinal - posicionInicial;
        float duracion = Time.time - tiempoInicial;

        // swipe: desplazamiento horizontal amplio y mayor que el vertical
        if (Mathf.Abs(desplazamiento.x) >= umbralSwipe &&
            Mathf.Abs(desplazamiento.x) > Mathf.Abs(desplazamiento.y))
        {
            detectarSwipe(desplazamiento.x > 0);
        }
        // toque: apenas se movió el dedo y fue breve
        else if (desplazamiento.magnitude < umbralSwipe && duracion <= tiempoMaximoToque)
        {
            detectarToque(posicionFinal);
        }
    }

    // Lanza un rayo desde la pantalla y, si golpea un Pokémon, abre su ficha de la Pokédex
    void detectarToque(Vector2 posicionPantalla)
    {
        // si el toque fue sobre la interfaz (por ejemplo, el panel de la Pokédex), lo ignora
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Camera.main == null) return;

        Ray rayo = Camera.main.ScreenPointToRay(posicionPantalla);
        RaycastHit impacto;

        if (Physics.Raycast(rayo, out impacto, distanciaRaycast))
        {
            // GetComponentInParent permite tocar cualquier parte del modelo (mallas hijas)
            Pokemon_info info = impacto.collider.GetComponentInParent<Pokemon_info>();

            if (info != null)
            {
                info.reproducirSonido();

                if (PokedexUI.instancia != null)
                    PokedexUI.instancia.Mostrar(info);
            }
        }
    }

    // Cambia la evolución de las líneas evolutivas cuya carta está siendo detectada
    void detectarSwipe(bool haciaDerecha)
    {
        Evolutions[] lineas = FindObjectsOfType<Evolutions>();

        foreach (Evolutions linea in lineas)
        {
            if (!linea.cartaVisible) continue;

            if (haciaDerecha)
                linea.siguienteEvolucion();
            else
                linea.evolucionAnterior();
        }
    }
}