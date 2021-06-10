using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gatilhoporta : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject porta;
    //public Animator anim;
    public Renderer renderer;

    public Color color;
    private bool rot = false;
    void Start()
    {
        //anim = porta.GetComponent<Animator> ();
        renderer = GetComponent<Renderer>();
        color = renderer.material.color;
    }

    private void Update() {
        renderer.material.color = Color.Lerp(Color.white, Color.black,Mathf.PingPong(Time.time, 0.5f));
        if(rot){
            Debug.Log("aperte F");
            //renderer.material.color = Color.Lerp(Color.white, Color.black,Mathf.PingPong(Time.time, 0.5f));
            //if(Input.GetKeyDown(KeyCode.F))
                //anim.SetBool("rotacionar",rot);
        }else{
            //anim.SetBool("rotacionar",rot);
            //renderer.material.color = color;
        }
        
       
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            rot = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            rot = false;
        }
    }
 
}
