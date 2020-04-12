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

    public static List<Coord> PathFind(Coord src, Coord dest) {

        bool[,] matrix = WorldGenerator._instance.walkableTiles;
        
        // Track all edges made
        List<Edge> sets = new List<Edge>();

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

            queue.Dequeue();

            // When Dequeue, label current coord as src
            Coord lastSrc = current.coord;

            for (int i = 0; i < 4; i++) {
                int row = coord.x + rowNum[i];
                int col = coord.y + colNum[i];

                if (IsValid(row, col) && matrix[row, col] && !discovered[row, col]) {
                    discovered[row, col] = true;

                    PathSumCoord adjacent = new PathSumCoord(new Coord(row, col), current.dist + 1);

                    queue.Enqueue(adjacent);

                    // When enqueue, label current coord as dest
                    Coord lastDest = new Coord(row, col);

                    sets.Add(new Edge(lastSrc, lastDest));
                }
            }
        }

        // Track the shortest path
        List<Coord> path = new List<Coord>();
   
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
                break;
            }
        }

        return path;
    }

    public static bool IsValid(int row, int col) {
        return row >= 0 && row < WorldGenerator._instance.mapSize && col >= 0 && col < WorldGenerator._instance.mapSize;
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
}


