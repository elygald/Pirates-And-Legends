using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class ControllerPlayer : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public bool groundedPlayer;
    private float playerSpeed = 5.0f;
    private float jumpHeight = 6.0f;
    private float gravityValue = -9.81f;
    public GameObject camera;
    public float inputY;
    public float inputX;
    private Animator anim;
    private bool jump = false;
    public float trunSmoothTime = 0.1f;
    float trunSmoothVelocity;
    float timerattack;
    

    private void Start()
    {
        anim = GetComponent<Animator> ();
        controller = GetComponent<CharacterController>();;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        inputY = Input.GetAxis ("Vertical");
        inputX = Input.GetAxis ("Horizontal");
        
         if(Input.GetMouseButtonDown(0))
        {
            if(timerattack > 1.30f){
                if(groundedPlayer){
                    StartCoroutine(Attack());
                    timerattack = 0f;
                }
            }
        }
        timerattack += Time.deltaTime;

        Vector3 direction = new Vector3(inputX, 0, inputY).normalized;
        
        if(direction.magnitude >= 0.01f){
            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref trunSmoothVelocity, trunSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 movDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(movDir.normalized * Time.deltaTime * playerSpeed);

        }

        if (groundedPlayer){
            anim.SetFloat("Horizontal", inputX);
            anim.SetFloat("Vertical", inputY);
        }      
       
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jump =false;
        }
       

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && !jump)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -1.0f * gravityValue);
            jump = true;
            anim.SetBool("jump",jump);
        }
        
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        
        anim.SetBool("jump",jump);

       
    }

     private IEnumerator Attack(){
        anim.SetLayerWeight(anim.GetLayerIndex("attack"),1);            
        anim.SetBool("attack", true);

        yield return new WaitForSeconds(1.30f);
        anim.SetLayerWeight(anim.GetLayerIndex("attack"),0);  
        anim.SetBool("attack", false);
     }
}
