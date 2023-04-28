using UnityEngine;

/// <summary>
/// A simple rotation script for the camera
/// to orbit around a point in specific scenes. 
/// </summary>
public class ExampleOrbitCamera : MonoBehaviour
{
	public Transform lookAtPoint;
	public float speed = 1;
	public void FixedUpdate()
	{
		transform.LookAt(lookAtPoint);
		transform.Translate(Vector3.left * (speed * Time.deltaTime));
	}
}
