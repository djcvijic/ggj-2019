using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
	public float moveSpeed = 1;
	public Transform background;
	public float paralaxAmount = 1;
	public Vector2 movementBounds;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
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
}
