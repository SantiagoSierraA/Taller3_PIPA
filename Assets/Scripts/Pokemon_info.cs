using UnityEngine;


public class Pokemon_info : MonoBehaviour
{
    [Header("Datos de la Pokedex")]
    public string nombrePokemon = "";
    public string tipo = ""; 
    [TextArea(3, 5)]
    public string descripcion = ""; 
    public Sprite imagen;

    [Header("Audio")]
    public AudioClip sonido;
    [Range(0f, 1f)] public float volumen = 1f;

    // Reproduce el sonido del Pokémon en la posicion de la camara, para que se
    // escuche siempre igual sin importar donde este el modelo en el espacio.
    public void reproducirSonido()
    {
        if (sonido == null) return;

        Vector3 posicion = Camera.main != null ? Camera.main.transform.position : transform.position;
        AudioSource.PlayClipAtPoint(sonido, posicion, volumen);
    }
}