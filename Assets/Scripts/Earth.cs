using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
	public float moveSpeed = 1;
	public Transform background;
	public float paralaxAmount = 1;
	public float Zangle = -1;
	public Vector2 movementBounds;
	public Transform EarthRotate;

	private Vector3 initialPosition;
	private Quaternion initialRotation;
	private Vector3 initialBackgroundPosition;
	private Quaternion initialEarthRotation;

	// Use this for initialization
	void Start ()
	{
		initialPosition = transform.position;
		initialRotation = transform.rotation;
		if (background != null) initialBackgroundPosition = background.position;
		if (EarthRotate != null) initialEarthRotation = EarthRotate.rotation;
	}

	public void Restart()
	{
		transform.position = initialPosition;
		transform.rotation = initialRotation;
		if (background != null) background.position = initialBackgroundPosition;
		if (EarthRotate != null) EarthRotate.rotation = initialEarthRotation;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameController.Instance.state == GameController.State.Running)
		{
			var moveH = moveSpeed * Input.GetAxis("Horizontal");
			var moveV = moveSpeed * Input.GetAxis("Vertical");
			transform.Translate(moveH, moveV, 0);

			var pos = transform.position;
			var posX = Mathf.Clamp(pos.x, -movementBounds.x, movementBounds.x);
			var posY = Mathf.Clamp(pos.y, -movementBounds.y, movementBounds.y);
			transform.position = new Vector3(posX, posY, 0);

			if (background != null)
			{
				background.position = -paralaxAmount * transform.position;
			}
		}

		if (EarthRotate != null)
		{
			EarthRotate.Rotate(0, 0, Zangle);
		}
	
	}
}
