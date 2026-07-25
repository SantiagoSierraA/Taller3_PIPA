using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokedexUI : MonoBehaviour
{
    // instancia estatica para acceder a la Pokedex desde cualquier script
    public static PokedexUI instancia;

    [Header("Panel")]
    public GameObject panel;                 // panel completo de la Pokedex (oculto al inicio)

    [Header("Textos")]
    public TMP_Text textoNombre;             // nombre del Pokemon
    public TMP_Text textoTipo;               // tipo del Pokemon
    public TMP_Text textoDescripcion;        // descripción breve

    [Header("Imagen (opcional)")]
    public Image imagenPokemon;              // imagen del Pokemon dentro de la ficha

    // Awake se ejecuta antes que Start: asignamos la instancia del singleton
    void Awake()
    {
        instancia = this;
    }

    // Start se ejecuta al iniciar la escena: la Pokedex comienza cerrada
    void Start()
    {
        if (panel != null) panel.SetActive(false);
    }

    // Muestra la ficha del Pokemon recibido
    public void Mostrar(Pokemon_info info)
    {
        if (info == null || panel == null) return;

        if (textoNombre != null)      textoNombre.text = info.nombrePokemon;
        if (textoTipo != null)        textoTipo.text = "Tipo: " + info.tipo;
        if (textoDescripcion != null) textoDescripcion.text = info.descripcion;

        // si el Pokemon tiene imagen asignada se muestra; si no, se oculta el espacio
        if (imagenPokemon != null)
        {
            imagenPokemon.sprite = info.imagen;
            imagenPokemon.enabled = (info.imagen != null);
        }

        panel.SetActive(true);
        // fuerza a recalcular el layout para que el primer toque no salga desordenado
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(panel.GetComponent<RectTransform>());
    }

    // Cierra la Pokedex. Se conecta al boton de la interfaz.
    public void Cerrar()
    {
        if (panel != null) panel.SetActive(false);
    }

    // Indica si la Pokedex esta abierta en este momento
    public bool estaAbierta()
    {
        return panel != null && panel.activeSelf;
    }
}