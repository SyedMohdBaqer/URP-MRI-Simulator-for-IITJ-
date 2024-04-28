using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
   public void playAnimationAndSound()
    {
        animator.Play("Talking animation");
    }
}
