using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class ControllerPlayer : MonoBehaviour
{
    private string turnImputAxisX= "Horizontal";
    private string turnImputAxisY= "Vertical";
    private Animator anim;
    [Tooltip("Rate per seconds holding down input")]
    private float inputX;
    private float inputY;

    public float rotationRate = 360;
    // Update is called once per frame
    

    void Start()
    {
        anim = GetComponent<Animator> ();
    }
    void Update()
    {
        inputX = Input.GetAxis (turnImputAxisX);
        inputY = Input.GetAxis (turnImputAxisY);
        ApplyInput(inputX);

        anim.SetFloat(turnImputAxisX, inputX);
        anim.SetFloat(turnImputAxisY, inputY);    
    }

    void ApplyInput(float trunImput)
    {
        Turn(trunImput);
    }

    void Turn(float input)
    {
        transform.Rotate(0, input * rotationRate * Time.deltaTime, 0);
    }



}
