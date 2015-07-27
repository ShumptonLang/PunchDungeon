using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	public float speed = 3.0f;
	public float jumpSpeed = 50.0f;
	private float playerY = 0;
	private Animator playerAnimator;
	private Rigidbody2D playerRigidbody;
	private SpriteRenderer playerRenderer;//<-- I am adding a renderer so i can judge the sprites length / 2 so raycast will work properly
	private Transform playerTransform;
	private float distToGround;
	public float leeway = 0.1f;
	public LayerMask mask = -1;
	public float downPull = 3f;

	void Start () {
		playerAnimator = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody2D> ();
		playerTransform = GetComponent<Transform> ();
		playerRenderer = GetComponent<SpriteRenderer> ();
		distToGround = playerRenderer.bounds.extents.y;
	}
	void Update () {

	}

	void FixedUpdate () {

		playerY = playerRigidbody.velocity.y;
		playerRigidbody.velocity = new Vector2 (Input.GetAxis ("Horizontal") * speed,
		                                       playerY);
		playerAnimator.SetFloat ("Walking", Mathf.Abs (Input.GetAxis ("Horizontal")));
		if (Input.GetAxis ("Horizontal") > 0) {
			transform.localRotation = Quaternion.Euler (0, 0, 0);	
		} else if (Input.GetAxis ("Horizontal") < 0) {
			transform.localRotation = Quaternion.Euler (0, 180, 0);
		}

		if (Input.GetKey (KeyCode.Space) && IsGrounded () && playerY > -0.5f && playerY < 0.5f) {
			playerRigidbody.AddForce (Vector2.up * jumpSpeed, ForceMode2D.Impulse);
		} 
		if (Input.GetKeyUp (KeyCode.Space) && IsGrounded() == false) {
			playerRigidbody.velocity -= new Vector2(0f, downPull);
		}

	


	}

	bool IsGrounded(){
		Debug.DrawRay (playerTransform.position, -Vector2.up * (distToGround + leeway));
		return Physics2D.Raycast (playerTransform.position, -Vector3.up, distToGround + leeway, mask);
	}
}
