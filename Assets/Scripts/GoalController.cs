using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour {

	public void SetLocation(MazeCell cell)  {
		transform.localPosition = new Vector3(cell.coordinates.x /2 - 0.5f, 0.5f, cell.coordinates.z /2- 0.5f);
	}
}
