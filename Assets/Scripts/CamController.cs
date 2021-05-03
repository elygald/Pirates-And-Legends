using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public GameObject target;
    public GameObject camposition;
    public float velocitymovement = 2.0f;
    private RaycastHit hit;

    [SerializeField]
	Transform focus = default;

	[SerializeField, Range(1f, 20f)]
	float distance = 5f;
    [SerializeField, Min(0f)]
	float focusRadius = 1f;
    Vector3 focusPoint, previousFocusPoint;
    Vector2 orbitAngles = new Vector2(45f, 0f);
    [SerializeField, Range(1f, 360f)]
	float rotationSpeed = 10f;
    private float mouseX =0.0f,mouseY =0.0f;

    [SerializeField, Range(-89f, 89f)]
	float minVerticalAngle = -30f, maxVerticalAngle = 60f;

    [SerializeField, Min(0f)]
	float alignDelay = 5f;

    float lastManualRotationTime;
    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;    
    }
    void Awake () {
		focusPoint = focus.position;
        transform.localRotation = Quaternion.Euler(orbitAngles);
	}

    void UpdateFocusPoint () {
		Vector3 targetPoint = focus.position;
		focusPoint = targetPoint;
	}

    bool ManualRotation () {

      
		Vector2 input = new Vector2( -Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
		const float e = 0.001f;
		if (input.x < -e || input.x > e || input.y < -e || input.y > e) {
			orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
            lastManualRotationTime = Time.unscaledTime;
            return true;
		}
        return false;
	}

    static float GetAngle (Vector2 direction) {
		float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
		return direction.x < 0f ? 360f - angle : angle;
	}
    bool AutomaticRotation () {
		if (Time.unscaledTime - lastManualRotationTime < alignDelay) {
			return false;
		}
        
        Vector2 movement = new Vector2(
			focusPoint.x - previousFocusPoint.x,
			focusPoint.z - previousFocusPoint.z
		);
		float movementDeltaSqr = movement.sqrMagnitude;
		if (movementDeltaSqr < 0.000001f) {
			return false;
		}
        float headingAngle = GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
        float rotationChange = rotationSpeed * Time.unscaledDeltaTime;
		orbitAngles.y = Mathf.MoveTowardsAngle(orbitAngles.y, headingAngle, rotationChange);
		
		return true;
	}
    void OnValidate () {
		if (maxVerticalAngle < minVerticalAngle) {
			maxVerticalAngle = minVerticalAngle;
		}
	}

    void ConstrainAngles () {
		orbitAngles.x =
			Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

		if (orbitAngles.y < 0f) {
			orbitAngles.y += 360f;
		}
		else if (orbitAngles.y >= 360f) {
			orbitAngles.y -= 360f;
		}
	}
    void LateUpdate()
    {
		
      UpdateFocusPoint();
      Quaternion lookRotation;
		if (ManualRotation()) { //|| AutomaticRotation()) {
			ConstrainAngles();
			lookRotation = Quaternion.Euler(orbitAngles);
		}
		else {
			lookRotation = transform.localRotation;
		}
		Vector3 lookDirection = lookRotation * Vector3.forward;
		Vector3 lookPosition = focusPoint - lookDirection * distance;
		if (Physics.Raycast(
			focusPoint, -lookDirection, out RaycastHit hit, distance
		)) {
			lookPosition = focusPoint - lookDirection * hit.distance;
		}
		transform.SetPositionAndRotation(lookPosition, lookRotation);

		if (Input.GetAxis("Mouse ScrollWheel") > 0f && distance > 1f) // forward
 		{
			distance--;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f && distance < 20f) // backwards
		{			
			distance++;
		}
        
    }
    
}
