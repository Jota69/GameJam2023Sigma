using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolumeControl : MonoBehaviour
{
    public AudioSource[] audioSources; // Array de AudioSources
    public Slider volumeSlider; // Slider para controlar el volumen

    private void Start()
    {
        // Cargar el valor del volumen guardado
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
    }

    private void Update()
    {
        // Actualizar el volumen de los AudioSources
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = volumeSlider.value;
        }
    }

    public void SaveVolume()
    {
        // Guardar el valor del volumen
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }

    private void OnDisable()
    {
        // Guardar el volumen cuando se cambia de escena
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnEnable()
    {
        // Remover el evento cuando el objeto es destruido
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene current)
    {
        SaveVolume();
    }
}
