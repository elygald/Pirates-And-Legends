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
    public float inputX;
    public float inputY;

    private Quaternion target;
    public float rotationRate = 90;

    public float timerjump = 1f;
    // Update is called once per frame

    public float timerattack = 1.30f;
    // Update is called once per frame
    
    public float jumpSpeed =10.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    void Start()
    {
        anim = GetComponent<Animator> ();
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        inputX = Input.GetAxis (turnImputAxisX);
        inputY = Input.GetAxis (turnImputAxisY);
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)){
            ApplyInput(0f);
        }
        if(Input.GetKey(KeyCode.A)){
            inputX = 1;
            inputY = 0;
            ApplyInput(-90f);
        } 
        if(Input.GetKey(KeyCode.D)){
            inputX = -1;
            inputY = 0;
            ApplyInput(90f);
        }

        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)){
            ApplyInput(-45f);
        }
        
        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)){
            ApplyInput(45f);
        }

        if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)){
            inputY = -1;
            ApplyInput(45f);
        }
        
        if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)){
            inputY = -1;
            ApplyInput(-45f);
        }
        
        anim.SetFloat(turnImputAxisX, inputX);
        anim.SetFloat(turnImputAxisY, inputY);

        if(Input.GetMouseButtonDown(0))
        {
            if(timerattack > 1.30f){
                if(characterController.isGrounded){
                    StartCoroutine(Attack());
                    timerattack = 0f;
                }
            }
        }
        timerattack += Time.deltaTime;
        
        if(Input.GetKey(KeyCode.Space))
        {   
           Jump();
        }else{
            timerjump = 1f;
            anim.SetBool("jump", false);
            anim.SetBool("JumpBack", false);
        }
    }
     private IEnumerator Attack(){
        anim.SetLayerWeight(anim.GetLayerIndex("attack"),1);            
        anim.SetBool("attack", true);

        yield return new WaitForSeconds(1.30f);
        anim.SetLayerWeight(anim.GetLayerIndex("attack"),0);  
        anim.SetBool("attack", false);
     }
    void ApplyInput(float angle)
    {
        target = Quaternion.Euler(transform.rotation.x, camera.transform.eulerAngles.y + angle, transform.rotation.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, target,  rotationRate * Time.deltaTime);
    }

    void Jump(){
         timerjump += Time.deltaTime;
            if(characterController.isGrounded){
                if(timerjump > 1f){
                    anim.SetBool("JumpBack", Input.GetKey(KeyCode.S)); 
                    anim.SetBool("jump", !Input.GetKey(KeyCode.S));
                    moveDirection.y = jumpSpeed;
                    anim.SetBool("attack", false);
                    anim.SetLayerWeight(anim.GetLayerIndex("attack"),0); 
                    timerjump = 0f; 
                }
                
            }
            moveDirection.y -= gravity * Time.deltaTime;
            characterController.Move(moveDirection * Time.deltaTime);
    }
 
}
