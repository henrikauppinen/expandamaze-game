using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float vInput;
	private float hInput;
	private Vector3 movement;

	public float speed;
	public Rigidbody playerRigidbody;

	public GameManager Gm;

	public void SetLocation(MazeCell cell)  {

		transform.localPosition = new Vector3(1f, 1f, 1f);
	}

	private void Start() {
		playerRigidbody = GetComponent<Rigidbody> ();

		GameObject GmGameObject = GameObject.Find ("Game Manager");
		Gm = GmGameObject.GetComponent<GameManager> ();
		Debug.Log (Gm);
	}

	private void Update() {
		hInput = Input.GetAxisRaw ("Horizontal");
		vInput = Input.GetAxisRaw ("Vertical");

		move (hInput, vInput);

		rotate (hInput, vInput);
	}

	private void move(float h, float v) {
		movement.Set (h, 0f, v);

		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition (transform.position + movement);
	}

	private void rotate(float h, float v) {
		Vector3 inputVector = new Vector3 (h, 0, v);

		if(inputVector != Vector3.zero) {
			playerRigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVector), Time.deltaTime * speed * 2));
		}
	}


	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Goal")) {
			Gm.NextLevel();
		}
	}

}
