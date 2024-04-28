using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SceneTransitionManager : MonoBehaviour {
    public AudioClip sceneChangeAudio;
    public FadeScreen fadeScreen;
    public Transform targetPositionobj;
    public GameObject cameraobject;

    //public GameObject mainCamera;
     Vector3 targetPosition;
     Quaternion targetRotation;
    //public Vector3 defaultPosition = new Vector3(0, 0, 0);
    //public Quaternion defaultRotation = Quaternion.Euler(0, 0, 0);
    //public void GoToScene(int sceneIndex)
    //{
    //    StartCoroutine(GoToSceneRoutine(sceneIndex));
    //}
    //IEnumerator GoToSceneRoutine(int sceneIndex)
    //{
    //    fadeScreen.FadeOut();
    //    yield return new WaitForSeconds(fadeScreen.fadeDuration);
    //    SceneManager.LoadScene(sceneIndex);
    //}

    public void GoToSceneAsync(int sceneIndex) 
    { 
        //StartCoroutine(playSound(sceneIndex));

        StartCoroutine(GoToSceneAsyncRoutine(sceneIndex));
    }

    IEnumerator GoToSceneAsyncRoutine(int sceneIndex) 
    {
        fadeScreen.FadeOut();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float timer = 0;
             while (timer <= fadeScreen.fadeDuration)
             {
                 timer += Time.deltaTime;
                 yield return null;
             }
            
        //cameraobject.transform.position = targetPosition.position;
        //cameraobject.transform.rotation = targetPosition.rotation;
        //fadeScreen.FadeIn();
        operation.allowSceneActivation = true;  
    }

    //IEnumerator playSound(int sceneIndex)
    //{
    //    if (sceneIndex == 1)
    //    {
    //        AudioSource audio = GetComponent<AudioSource>();
    //        audio.clip = sceneChangeAudio;
    //        audio.Play();
    //        yield return new WaitForSeconds(audio.clip.length);
    //    }
    //    StartCoroutine(GoToSceneAsyncRoutine(sceneIndex));
    //}

    public void GoToPositionAsync()
    {

        StartCoroutine(GoToPositionAsyncRoutine());
    }

    IEnumerator GoToPositionAsyncRoutine()
    {
        fadeScreen.FadeOut();

        float timer = 0;
        while (timer <= fadeScreen.fadeDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        cameraobject.transform.position = targetPositionobj.position;
        cameraobject.transform.rotation = targetPositionobj.rotation;
        //mainCamera.transform.position = new Vector3(0, 0, 0);
        //mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        fadeScreen.FadeIn();
    }
}