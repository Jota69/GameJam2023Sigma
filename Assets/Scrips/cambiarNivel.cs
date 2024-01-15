using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cambiarNivel : MonoBehaviour
{
    [SerializeField] private AnimationClip clipTransition;
    private Animator transitionAnimator;
    [SerializeField] private GameObject hijo;

    public int SceneSig;
    private float transitionTime;
    // Start is called before the first frame update
    void Start()
    {
        transitionAnimator = hijo.GetComponent<Animator>();
        transitionTime = clipTransition.length;
        Invoke("ActivarCanva", transitionTime);

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
