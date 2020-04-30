using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Edge {

    public Coord src;
    public Coord dest;

    public Edge(Coord src, Coord dest) {
        this.src = src;
        this.dest = dest;
    }
}

public static class Navigation {

    private static int maxSearch = 100000;

    public static List<Coord> PathFind(Coord src, Coord dest) {

        // Iteration counter to limit the scop
        int iteration = 0;
        bool[,] matrix = WorldGenerator._instance.walkableTiles;
        // Track all edges made
        List<Edge> sets = new List<Edge>();
        // Track the shortest path
        List<Coord> path = new List<Coord>();

        int[] rowNum;
        int[] colNum;

        // src is bottom left, dest is top right
        if (src.x <= dest.x && src.y <= dest.y) {
            rowNum = new int[] { 1, 0, -1, 0 };
            colNum = new int[] { 0, 1, 0, -1 };
        }
        // src is bottom right, dest is top left
        else if (src.x >= dest.x && src.y <= dest.y) {
            rowNum = new int[] { -1, 0, 1, 0 };
            colNum = new int[] { 0, 1, 0, -1 };
        }
        // src is top left, dest is bottom right
        else if (src.x < dest.x && src.y > dest.y) {
            rowNum = new int[] { 0, 1, -1, 0 };
            colNum = new int[] { -1, 0, 0, 1 };
        }
        // src is top right, dest is bottom left
        else {
            rowNum = new int[] { -1, 0, 1, 0 };
            colNum = new int[] { 0, -1, 0, 1 };
        }


        // If Src or dest is not walkable, return null
        if (matrix[src.x, src.y] != true || matrix[dest.x, dest.y] != true) {
            return null;
        }
        else if (Distance(src, dest) <= 1) {
            path.Add(dest);
            return path;
        }

        int size = matrix.Length;
        bool[,] discovered = new bool[size, size];

        for (int j = 0; j < discovered.GetLength(1); ++j) {
            for (int i = 0; i < discovered.GetLength(0); ++i) {
                discovered[i, j] = false;
            }
        }

        discovered[src.x, src.y] = true;

        Queue<PathSumCoord> queue = new Queue<PathSumCoord>();
        PathSumCoord s = new PathSumCoord(src, 0);
        queue.Enqueue(s);

        while (queue.Count != 0) {

            PathSumCoord current = queue.Peek();
            Coord coord = current.coord;

            // Reached the destination
            if (coord.x == dest.x && coord.y == dest.y) {
                break;
            }

            if (iteration > maxSearch) {
                Debug.Log("max iteration reached");
                return null;
            }

            iteration++;
            queue.Dequeue();

            // When Dequeue, label current coord as src
            Coord lastSrc = current.coord;

            for (int i = 0; i < 4; i++) {
                int row = coord.x + rowNum[i];
                int col = coord.y + colNum[i];

                Coord currCoord = new Coord(row, col);

                if (IsValid(currCoord) && matrix[row, col] && !discovered[row, col]) {
                    discovered[row, col] = true;

                    PathSumCoord adjacent = new PathSumCoord(new Coord(row, col), current.dist + 1);

                    queue.Enqueue(adjacent);

                    // When enqueue, label current coord as dest
                    Coord lastDest = new Coord(row, col);

                    sets.Add(new Edge(lastSrc, lastDest));
                }
            }
        }

        Coord last = dest;
        path.Add(last);

        while (true) {
            // Find the edge that contains src as dest
            foreach (Edge element in sets) {
                if (element.dest.x == last.x && element.dest.y == last.y) {
                    last = element.src;
                    path.Insert(0, element.src);

                    break;
                }
            }

            // Reached the beginning
            if (last == src) {
                path.Remove(last);
                break;
            }
        }

        return path;
    }

    public static Coord FindItem(Coord src, LivingEntityType livingEntityType, int visionRadius) {

        List<Coord> nearbyTiles = FindVisibleTiles(src, visionRadius);

        int minDist = int.MaxValue;
        Coord minCoord = null;
        bool flag;

        foreach (Coord element in nearbyTiles) {
            
            if (livingEntityType == LivingEntityType.Grain) {
                flag = WorldGenerator._instance.activeTiles[element.x, element.y].gameObject.GetComponent<Tile>().hasGrain;
            }
            else if (livingEntityType == LivingEntityType.Chicken) {
                flag = WorldGenerator._instance.activeTiles[element.x, element.y].gameObject.GetComponent<Tile>().hasChicken;
                Debug.Log(element.x + "," + element.y + ":" + flag);
            }
            else if (livingEntityType == LivingEntityType.Fox) {
                flag = WorldGenerator._instance.activeTiles[element.x, element.y].gameObject.GetComponent<Tile>().hasFox;
            }
            else {
                return null;
            }

            int dist = Distance(src, element);
            
            if (flag && dist < minDist) {
                minDist = dist;
                minCoord = element;
            }
        }

        return minCoord;
    }

    public static List<Coord> FindVisibleTiles(Coord src, int visionRadius) {

        bool[,] matrix = WorldGenerator._instance.walkableTiles;

        List<Coord> nearbyCoords = new List<Coord>();

        Coord bottomLeftCoord = new Coord(src.x - visionRadius, src.y - visionRadius);

        for (int j = 0; j < 1 + visionRadius * 2; j++) {
            for (int i = 0; i < 1 + visionRadius * 2; i++) {

                int currentX = bottomLeftCoord.x + i;
                int currentY = bottomLeftCoord.y + j;

                Coord currentCoord = new Coord(currentX, currentY);

                if (IsValid(currentCoord)) {
                    if (IsNear(src, currentCoord, visionRadius)) {
                        if (IsNotWaterTile(currentCoord)) {
                            Coord validCoord = new Coord(currentX, currentY);
                            nearbyCoords.Add(validCoord);
                        }
                    }
                }
            }
        }

        return nearbyCoords;
    }

    public static bool IsValid(Coord coord) {
        return coord.x >= 0 && coord.x < WorldGenerator._instance.mapSize && coord.y >= 0 && coord.y < WorldGenerator._instance.mapSize;
    }

    private static bool IsNear(Coord src, Coord dest, int radius) {
        int dist = Distance(src, dest);

        return radius >= dist;
    }

    public static bool IsNotWaterTile(Coord src) {
        return WorldGenerator._instance.walkableTiles[src.x, src.y];
    }

    public static int Distance(Coord src, Coord dest) {
        return Math.Abs(src.x - dest.x) + Math.Abs(src.y - dest.y);
    }

    public static Vector3 CoordToWorldPosition(Coord coord) {
        return new Vector3(coord.x * WorldInfo._instance.tileSizeMultiplier, 5, coord.y * WorldInfo._instance.tileSizeMultiplier);
    }

}

public class PathSumCoord {
    public Coord coord;
    public int dist;

    public PathSumCoord(Coord coord, int dist) {
        this.coord = coord;
        this.dist = dist;
    }
}

[System.Serializable]
public class Coord {

    public int x;
    public int y;

    public Coord(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public void Print() {
        Debug.Log(x + "," + y);
    }
}