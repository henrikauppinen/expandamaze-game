using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	public float generationStepDelay;
	public IntVector2 size;
	public MazeCell cellPrefab;
	public MazePassage passagePrefab;
	public MazeWall wallPrefab;

	private MazeCell[,] cells;

	public MazeCell GetCell (IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.z];
	}

	public void SetSize(IntVector2 newSize) {
		size = newSize;
	}

	public void Generate() {
		cells = new MazeCell[size.x, size.z];
		List<MazeCell> activeCells = new List<MazeCell> ();

		activeCells.Add (CreateCell (RandomCoordinates));

		while (activeCells.Count > 0) {
			DoNextGenerationStep (activeCells);
		}
	}
	
	private void DoNextGenerationStep(List<MazeCell>activeCells) {
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells [currentIndex];
		if (currentCell.IsFullyInitialized) {
			activeCells.RemoveAt (currentIndex);
			return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2 ();
		if (ContainsCoordinates (coordinates)) {
			MazeCell neighbor = GetCell (coordinates);
			if (neighbor == null) {
				neighbor = CreateCell (coordinates);
				CreatePassage (currentCell, neighbor, direction);
				activeCells.Add (neighbor);
			} else {
				CreateWall (currentCell, neighbor, direction);
			}
		} else {
			CreateWall(currentCell, null, direction);
		}
	}

	private MazeCell CreateCell(IntVector2 coordinates) {
		MazeCell newCell = Instantiate (cellPrefab) as MazeCell;
		cells [coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3 (coordinates.x - size.x / 2 + 0.5f, 0.0f, coordinates.z - size.z / 2 + 0.5f);

		return newCell;
	}

	private void CreatePassage(MazeCell cell, MazeCell OtherCell, MazeDirection direction) {
		MazePassage passage = Instantiate (passagePrefab) as MazePassage;
		passage.Initialize (cell, OtherCell, direction);
		passage = Instantiate (passagePrefab) as MazePassage;
		passage.Initialize (OtherCell, cell, direction.GetOpposite());
	}

	private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazeWall wall = Instantiate (wallPrefab) as MazeWall;
		wall.Initialize (cell, otherCell, direction);
		if (otherCell != null) {
			wall = Instantiate(wallPrefab) as MazeWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}

	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range (0,size.z));
		}
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return	coordinate.x >= 0 &&
			coordinate.x < size.x &&
			coordinate.z >= 0 &&
			coordinate.z < size.z;
	}

}
