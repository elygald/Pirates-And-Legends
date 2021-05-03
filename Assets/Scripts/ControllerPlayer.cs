using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class ControllerPlayer : MonoBehaviour
{
    public GameObject camera;
    private string turnImputAxisX= "Horizontal";
    private string turnImputAxisY= "Vertical";
    private Animator anim;
    [Tooltip("Rate per seconds holding down input")]
    private float inputX;
    private float inputY;

    private Quaternion target;
    public float rotationRate = 90;
    // Update is called once per frame
    

    void Start()
    {
        anim = GetComponent<Animator> ();
    }
    void Update()
    {
        inputX = Input.GetAxis (turnImputAxisX);
        inputY = Input.GetAxis (turnImputAxisY);
        if(inputY != 0f){
            target = Quaternion.Euler(transform.rotation.x, camera.transform.eulerAngles.y, transform.rotation.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, target,  rotationRate * Time.deltaTime);
        }else if(inputX != 0f){
            ApplyInput(inputX);
        }
        
        anim.SetFloat(turnImputAxisX, inputX);
        anim.SetFloat(turnImputAxisY, inputY);

        if(Input.GetMouseButtonDown(0))
        {
            anim.SetBool("ataque", true);
        }else{
            anim.SetBool("ataque", false);
        }    
    }

    void ApplyInput(float trunImput)
    {
        Turn(trunImput);
    }

    void Turn(float input)
    {
        if(input < 0 ){      
            target = Quaternion.Euler(transform.rotation.x, camera.transform.eulerAngles.y , transform.rotation.z);
              transform.rotation = Quaternion.Slerp(transform.rotation, target,  rotationRate * Time.deltaTime);
        }else if(input > 0 ){
            target = Quaternion.Euler(transform.rotation.x, camera.transform.eulerAngles.y, transform.rotation.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, target,   rotationRate * Time.deltaTime);
        }
    }    
}
