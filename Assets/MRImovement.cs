using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class MRImovement : MonoBehaviour
{
    public Transform entryPoint;
    public Transform insidePoint;
    public GameObject vrRig;  // The movable platform for the VR camera
    public float speed = 1.0f;  // Speed of movement
    private bool isInside = false;
    private AudioSource audioSource;
    public FadeScreen fadeScreen;
    //public AudioClip secondAudio;
    public GameObject uiComponent;
    public AudioClip lastClip;
    public GameObject mriAudio;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(WaitForAudioEnd());
    }

    private IEnumerator WaitForAudioEnd()
    {
        // Play the audio
        audioSource.Play();
        // Wait for the duration of the clip
        yield return new WaitForSeconds(audioSource.clip.length);
        // Perform your action here
        StartCoroutine(OnAudioComplete());
    }

    IEnumerator OnAudioComplete()
    {
        fadeScreen.FadeOut();
        float timer = 0;
        while (timer <= fadeScreen.fadeDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        uiComponent.SetActive(true);
        //audioSource.PlayOneShot(secondAudio);
        vrRig.transform.position = entryPoint.position;
        vrRig.transform.rotation = entryPoint.rotation;
        fadeScreen.FadeIn();
        
    }
    public void MoveMRI()
    {
        // Set initial position and rotation to match the entry point

        // Start the movement routine
        StartCoroutine(MoveInsideMRI());
    } 

    IEnumerator MoveInsideMRI()
    {
        // Move from the entry point to the inside point
        uiComponent.SetActive(false);
        yield return StartCoroutine(MoveToPosition(vrRig.transform, insidePoint.position, speed));
        isInside = true;

        // Wait for 2 seconds
        yield return new WaitForSeconds(10);

        // Move back to the entry point
        yield return StartCoroutine(MoveToPosition(vrRig.transform, entryPoint.position, speed));
        audioSource.PlayOneShot(lastClip);
        uiComponent.SetActive(true);
        mriAudio.SetActive(false );

        isInside = false;
    }

    IEnumerator MoveToPosition(Transform fromPosition, Vector3 toPosition, float speed)
    {
        float step = speed * Time.deltaTime;  // calculate distance to move
        while (Vector3.Distance(fromPosition.position, toPosition) > 0.01f)
        {
            fromPosition.position = Vector3.MoveTowards(fromPosition.position, toPosition, step);
            yield return null;  // Leave the coroutine and return here in the next frame
        }
    }
}
