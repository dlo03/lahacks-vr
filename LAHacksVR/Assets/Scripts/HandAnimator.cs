using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimator : MonoBehaviour
{
    private Animator animator;
    public float graspValue;
    private float[] animValue;
    private int animIndex;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //animValue = new float[] { 0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f};
        animValue = new float[] { 0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f };
        //0.28f for optimal grip?
    }

    // Update is called once per frame
    void Update()
    {
        // Later on, have a non-binary grasp value and remove the else
        if (Input.GetKey("p"))
        {
            AnimateGrasp(animValue[animIndex]);
            if (animIndex < animValue.Length-1)
            {
                animIndex = animIndex + 1;
            }
        } else
        {
            AnimateGrasp(animValue[animIndex]);
            if (animIndex > 0)
            {
                animIndex = animIndex - 1;
            }
        }
        
    }

    void AnimateGrasp(float value)
    {
        animator.SetFloat("Grasp", value);
        graspValue = value;
    }
}
