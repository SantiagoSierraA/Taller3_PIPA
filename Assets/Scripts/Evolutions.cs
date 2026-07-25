using UnityEngine;

public class Evolutions : MonoBehaviour
{
    [Header("Modelos 3D en orden evolutivo")]
    public GameObject[] evoluciones;      // modelos hijos del ImageTarget, del menor al mayor

    [Header("Estado del seguimiento")]
    public bool cartaVisible = true;      // true mientras Vuforia detecta esta carta

    private int indiceActual = 0;

    // Start se ejecuta al iniciar: deja visible únicamente la primera evolucion
    void Start()
    {
        mostrarEvolucion(0);
    }

    // Avanza a la siguiente evolucion (vuelve a la primera al llegar al final)
    public void siguienteEvolucion()
    {
        if (evoluciones.Length == 0) return;

        int siguiente = (indiceActual + 1) % evoluciones.Length;
        mostrarEvolucion(siguiente);
    }

    // Retrocede a la evolucion anterior (salta a la ultima si esta en la primera)
    public void evolucionAnterior()
    {
        if (evoluciones.Length == 0) return;

        int anterior = indiceActual - 1;
        if (anterior < 0) anterior = evoluciones.Length - 1;
        mostrarEvolucion(anterior);
    }

    // Activa el modelo indicado y desactiva el resto de la linea evolutiva
    void mostrarEvolucion(int indice)
    {
        for (int i = 0; i < evoluciones.Length; i++)
        {
            if (evoluciones[i] != null)
                evoluciones[i].SetActive(i == indice);
        }

        indiceActual = indice;

        // si la Pokedex esta abierta, actualiza la informacion al Pokémon que ahora se ve
        Pokemon_info info = evoluciones[indice] != null ? evoluciones[indice].GetComponent<Pokemon_info>() : null;
        if (info != null && PokedexUI.instancia != null && PokedexUI.instancia.estaAbierta())
            PokedexUI.instancia.Mostrar(info);
    }

    // Métodos conectados a los eventos On Target Found / On Target Lost de Vuforia
    public void marcarCartaVisible()   { cartaVisible = true; }
    // Se llama desde el evento On Target Lost de Vuforia
    public void marcarCartaNoVisible()
    {
        cartaVisible = false;

        // si esta carta era la que se estaba viendo, cierra la Pokédex
        if (PokedexUI.instancia != null)
            PokedexUI.instancia.Cerrar();
    }
}