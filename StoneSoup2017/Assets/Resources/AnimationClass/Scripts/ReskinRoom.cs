using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An example of a simple room generator.
// Walls are spawned randomly (unless an exit is required)
// Also spawns a random number of normal rocks and basic enemies.
public class ReskinRoom : Room {

	// The room needs references to the things it can spawn.
	public GameObject rockPrefab;
	public GameObject explodingRockPrefab;
	public GameObject enemyPrefab;
	public GameObject brutalEnemyPrefab;
	public GameObject projectileShooter;
	public GameObject[] decorationPrefabs;

	public int minNumRocks = 4, maxNumRocks = 14;
	public int minNumEnemies = 1, maxNumEnemies = 4;
	public int minBrutal = 0, maxBrutal = 2;

	// Chance there will be a decoration object!
	public float decorationProbability = .1f;

	//chance a shooter will spawn
	public float projectilerProbability = 1f;

	// Chance a wall
	public float borderWallProbability = 0.7f;
	public float RockExplodesProbability = 0.1f;


	public override void generateRoom(LevelGenerator ourGenerator, params Dir[] requiredExits) {
		// It's very likely you'll want to do different generation methods depending on which required exits you receive
		// Here's an example of randomly choosing between two generation methods.
		if (Random.value <= 0.5f) {
			roomGenerationVersionOne(ourGenerator, requiredExits);
		}		
		else {
			roomGenerationVersionTwo(ourGenerator, requiredExits);
		}

	}

	protected void roomGenerationVersionOne(LevelGenerator ourGenerator, Dir[] requiredExits) {
		//First we place ground tiles
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				Tile.spawnTile (ourGenerator.groundTilePrefab, transform, x, y);
			}
		}

		// In this version of room, there's no enemies. Just walls.
		generateWalls(ourGenerator, requiredExits);

		//Then we'll put decoration objects in here too.

		// Finally, let's make our random decoration object!
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (Random.value < decorationProbability) {
					int rd = Random.Range (0, decorationPrefabs.Length);
					Tile.spawnTile (decorationPrefabs[rd], transform, x, y);
				}
			}
		}
	}

	protected void roomGenerationVersionTwo(LevelGenerator ourGenerator, Dir[] requiredExits) {

		//now Diego is trying to add ground tiles.
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				Tile.spawnTile (ourGenerator.groundTilePrefab, transform, x, y);
			}
		}

		// In this version of room generation, I generate walls and then other stuff.
		generateWalls(ourGenerator, requiredExits);
		// Inside the borders I make some rocks and enemies.
		int numRocks = Random.Range(minNumRocks, maxNumRocks+1);
		int numEnemies = Random.Range(minNumEnemies, maxNumEnemies+1);
		int numBigEnemies = Random.Range (minBrutal, maxBrutal + 1);

		// First, let's make an array keeping track of where we've spawned objects already.
		bool[,] occupiedPositions = new bool[LevelGenerator.ROOM_WIDTH, LevelGenerator.ROOM_HEIGHT];
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (x == 0 || x == LevelGenerator.ROOM_WIDTH-1
					|| y == 0 || y == LevelGenerator.ROOM_HEIGHT-1) {
					// All border zones are occupied.
					occupiedPositions[x, y] = true;
				}
				else {
					occupiedPositions[x, y] = false;
				}
			}
		}

		// Now we spawn rocks and enemies in random locations
		List<Vector2> possibleSpawnPositions = new List<Vector2>(LevelGenerator.ROOM_WIDTH*LevelGenerator.ROOM_HEIGHT);
		for (int i = 0; i < numRocks; i++) {
			possibleSpawnPositions.Clear();
			for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
				for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
					if (occupiedPositions[x, y]) {
						continue;
					}
					possibleSpawnPositions.Add(new Vector2(x, y));
				}
			}
			if (possibleSpawnPositions.Count > 0) {
				Vector2 spawnPos = GlobalFuncs.randElem(possibleSpawnPositions);
				if (Random.value < RockExplodesProbability) {
					Tile.spawnTile (explodingRockPrefab, transform, (int)spawnPos.x, (int)spawnPos.y);
				} else {
					Tile.spawnTile (rockPrefab, transform, (int)spawnPos.x, (int)spawnPos.y);
				}
				occupiedPositions[(int)spawnPos.x, (int)spawnPos.y] = true;
			}
		}
		for (int i = 0; i < numEnemies; i++) {
			possibleSpawnPositions.Clear();
			for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
				for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
					if (occupiedPositions[x, y]) {
						continue;
					}
					possibleSpawnPositions.Add(new Vector2(x, y));
				}
			}
			if (possibleSpawnPositions.Count > 0) {
				Vector2 spawnPos = GlobalFuncs.randElem(possibleSpawnPositions);
				Tile.spawnTile(enemyPrefab, transform, (int)spawnPos.x, (int)spawnPos.y);
				occupiedPositions[(int)spawnPos.x, (int)spawnPos.y] = true;
			}
		}

		for (int i = 0; i < numBigEnemies; i++) {
			possibleSpawnPositions.Clear();
			for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
				for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
					if (occupiedPositions[x, y]) {
						continue;
					}
					possibleSpawnPositions.Add(new Vector2(x, y));
				}
			}
			if (possibleSpawnPositions.Count > 0) {
				Vector2 spawnPos = GlobalFuncs.randElem(possibleSpawnPositions);
				Tile.spawnTile(brutalEnemyPrefab, transform, (int)spawnPos.x, (int)spawnPos.y);
				occupiedPositions[(int)spawnPos.x, (int)spawnPos.y] = true;
			}
		}

		//if there's a projectileShooter, let's try and place that.
		if (projectileShooter != null) {
			if (Random.value < projectilerProbability) {
				for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
					for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
						if (occupiedPositions [x, y]) {
							continue;
						}
						possibleSpawnPositions.Add (new Vector2 (x, y));
					}
				}
				Vector2 spawnPos = GlobalFuncs.randElem (possibleSpawnPositions);
				Tile.spawnTile (projectileShooter, transform, (int)spawnPos.x, (int)spawnPos.y);
				occupiedPositions [(int)spawnPos.x, (int)spawnPos.y] = true;

			}
		}

		// Finally, let's make our random decoration object!
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (occupiedPositions[x, y]) {
					continue;
				}
				possibleSpawnPositions.Add(new Vector2(x, y));
			}
		}
		for (int p = 0; p < possibleSpawnPositions.Count-1; p++){
			Vector2 spawnPos = possibleSpawnPositions [p];
			if (Random.value < decorationProbability) {
				int rd = Random.Range (0, decorationPrefabs.Length);
				Tile.spawnTile (decorationPrefabs[rd], transform, (int)spawnPos.x, (int)spawnPos.y);
				occupiedPositions[(int)spawnPos.x, (int)spawnPos.y] = true;
			}
		}
	}



	protected void generateWalls(LevelGenerator ourGenerator, Dir[] requiredExits) {
		// Basically we go over the border and determining where to spawn walls.
		bool[,] wallMap = new bool[LevelGenerator.ROOM_WIDTH, LevelGenerator.ROOM_HEIGHT];
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (x == 0 || x == LevelGenerator.ROOM_WIDTH-1
					|| y == 0 || y == LevelGenerator.ROOM_HEIGHT-1) {

					if (x == LevelGenerator.ROOM_WIDTH/2 
						&& y == LevelGenerator.ROOM_HEIGHT-1
						&& containsDir(requiredExits, Dir.Up)) {
						wallMap[x, y] = false;
					}
					else if (x == LevelGenerator.ROOM_WIDTH-1
						&& y == LevelGenerator.ROOM_HEIGHT/2
						&& containsDir(requiredExits, Dir.Right)) {
						wallMap[x, y] = false;
					}
					else if (x == LevelGenerator.ROOM_WIDTH/2
						&& y == 0
						&& containsDir(requiredExits, Dir.Down)) {
						wallMap[x, y] = false;
					}
					else if (x == 0 
						&& y == LevelGenerator.ROOM_HEIGHT/2 
						&& containsDir(requiredExits, Dir.Left)) {
						wallMap[x, y] = false;
					}
					else {
						wallMap[x, y] = Random.value <= borderWallProbability;
					}
					continue;
				}
				wallMap[x, y] = false;
			}
		}

		// Now actually spawn all the walls.
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (wallMap[x, y]) {
					Tile.spawnTile(ourGenerator.normalWallPrefab, transform, x, y);
				}
			}
		}
	}



	// Simple utility function because I didn't bother looking up a more general Contains function for arrays.
	// Whoops.
	protected bool containsDir (Dir[] dirArray, Dir dirToCheck) {
		foreach (Dir dir in dirArray) {
			if (dirToCheck == dir) {
				return true;
			}
		}
		return false;
	}

}
