using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class cambiarNivel : MonoBehaviour
{
    [SerializeField] private AnimationClip clipTransition;
    private Animator transitionAnimator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject hijo;

    public int SceneSig;
    private float transitionTime;
    private bool bajarVol =false;
    private bool subirVol =false;
    // Start is called before the first frame update
    void Start()
    {
        transitionAnimator = hijo.GetComponent<Animator>();
        transitionTime = clipTransition.length;
        subirVol = true;
        Invoke("ActivarCanva", transitionTime);

    }
    private void Update()
    {
        if (audioSource!=null)
        {
            if (subirVol && audioSource.volume < 1)
            {
                audioSource.volume += transitionTime * Time.deltaTime;
            }
            else if (audioSource.volume == 1)
            {
                subirVol = false;
            }
            if (bajarVol && audioSource.volume > 0)
            {
                audioSource.volume += -transitionTime * Time.deltaTime;
            }
        }
        if (videoPlayer!=null)
        {
            if (subirVol && videoPlayer.GetDirectAudioVolume(0)<1)
            {
                videoPlayer.SetDirectAudioVolume(0, videoPlayer.GetDirectAudioVolume(0) + transitionTime * Time.deltaTime);
            }
            else if (videoPlayer.GetDirectAudioVolume(0) == 1)
            {
                subirVol = false;
            }
            if (bajarVol && videoPlayer.GetDirectAudioVolume(0) > 0)
            {
                videoPlayer.SetDirectAudioVolume(0, videoPlayer.GetDirectAudioVolume(0) - transitionTime * Time.deltaTime);
            }
        }
    }

    public void LoadNextScene(int Scene)
    {
        StartCoroutine(SceneLoad(Scene));
    }


    public IEnumerator SceneLoad(int Scene)
    {
        if (Scene==0)
        {
            //Eventos.eve.Recuperarvida.Invoke();
        }
        hijo.SetActive(true);
        //Time.timeScale = 0;
        transitionAnimator.SetTrigger("StartTransition");
        bajarVol = true;
        yield return new WaitForSeconds(transitionTime);
        //Time.timeScale = 1;
        SceneManager.LoadScene(Scene);

    }

    private void ActivarCanva()
    {
        hijo.SetActive(false);
    }
    private void OnEnable()
    {
        Eventos.eve.PasarNivel?.AddListener(LoadNextScene);
    }
    private void OnDisable()
    {
        Eventos.eve.PasarNivel?.RemoveListener(LoadNextScene);
    }
}
