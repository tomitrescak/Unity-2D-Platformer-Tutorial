using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets._2D;
using System;

public class LevelManager: MonoBehaviour {

	public enum Orientation {
		XY,
		XZ
	}

	// holds the definition of a tile
	[Serializable]
	public class Tile {
		// id of the tile, should be a single letter
		public char id;
		// reference to the tile
		public GameObject tile;
	}
	
	public Orientation orientation;
	public List<Tile> tiles;
	public string startLevel;

	void Start() {
		if (!string.IsNullOrEmpty (startLevel)) {
			this.LoadWorld(startLevel);
		}
	}

	public void LoadWorld(string definitionFile) {
		// load file from the resource folder (Assets/Resources)
		// the file has text format
		var world = Resources.Load<TextAsset> ("Levels/" + definitionFile);

		// the structure of the file is
		// 1.......2........
		// ......1111......4

		var worldText = world.text;
		var worldLines = world.text.Split('\n');

		// browse all lines and instantiate objects at desired positions
		for (var reverseRowIndex=worldLines.Length - 1; reverseRowIndex>=0; reverseRowIndex--) {
			var line = worldLines[reverseRowIndex];
			var rowIndex = worldLines.Length - reverseRowIndex;
			
			for (var columnIndex=0; columnIndex<line.Length; columnIndex++) {
				// assign tileId
				var tileId = line[columnIndex];

				// if it is an empty space, move on
				if (tileId == '.' || tileId == ' ') {
					continue;
				}
				// find the tile
				var tile = this.tiles.Find(w => w.id == tileId);

				// if tile does not exists, notify developer
				if (tile == null) {
					Debug.LogErrorFormat("Tile with id '{0}' does not exists!", tileId);
				}

				// instantiate tile at a given position according to orientation
				if (this.orientation == Orientation.XY) {
					Instantiate(tile.tile, new Vector3(columnIndex, rowIndex, 0), Quaternion.identity);
				} else if (this.orientation == Orientation.XZ) {
					Instantiate(tile.tile, new Vector3(columnIndex, 0, rowIndex), Quaternion.identity);
				}
			
			}
		}
		
		// make camera follow player
		var follow = Camera.main.GetComponent<SmoothFollow2D>();
		follow.target = GameObject.FindGameObjectWithTag("Player").transform;

	}

}
