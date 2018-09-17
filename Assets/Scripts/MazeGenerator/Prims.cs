/* Prims Maze Generator
 * 
 * Tiago Costa @ tiagojcosta29@gmail.com
 * 
 * 2018/09/17
 */ 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prims : MonoBehaviour {

    public int Witdh;
    public int Height;
    private int[,] grid;
    private List<string> gridElements;
    
	// Use this for initialization
	void Start ()
    {
        grid = new int[Witdh, Height];
        gridElements = new List<string>();
        FirstStep();
    }
	
    /// <summary>
    ///     Picks the first random piece of the maze. It can be anywhere
    /// </summary>
    private void FirstStep()
    {
        int randomx = UnityEngine.Random.Range(0, Witdh);
        int randomy = UnityEngine.Random.Range(0, Height);

        grid[randomx, randomy] = 1;
        gridElements.Add(randomx + "," + randomy);

        PickAndGo();
    }

    /// <summary>
    ///     
    /// </summary>
    private void PickAndGo()
    {
        if (gridElements.Count == 0)
        {
            DrawMaze();
            return;
        }

        // pick random element from list
        int random = UnityEngine.Random.Range(0, gridElements.Count);
        List<string> neighbours = new List<string>();

        // parse and fetch neighbours
        string[] index = gridElements[random].Split(',');
        int x = Int32.Parse(index[0]);
        int y = Int32.Parse(index[1]);
        int neighbourX;
        int neighbourY;

        // fetch neighbours
        if (grid[x, y] == 1)
        {           
            //right side neighbour
            if (x + 2 < Witdh && grid[x + 2, y] == 0)
            {
                neighbourX = x + 2;
                neighbours.Add(neighbourX + "," + y);
            }

            //left side
            if (x - 2 >= 0 && grid[x - 2, y] == 0)
            {
                neighbourX = x - 2;
                neighbours.Add(neighbourX + "," + y);
            }

            // Up
            if (y + 2 < Height && grid[x, y + 2] == 0)
            {
                neighbourY = y + 2;
                neighbours.Add(x + "," + neighbourY);
            }

            // Down
            if (y - 2 >= 0 && grid[x, y - 2] == 0)
            {
                neighbourY = y - 2;
                neighbours.Add(x + "," + neighbourY);
            }

            // remove if no neighbours
            if (neighbours.Count == 0)
            {
                gridElements.Remove(gridElements[random]);
                PickAndGo();
                return;
            }

            // picks one neighbour and build the path to them 
            if (neighbours.Count == 1)
            {
                SetValues(neighbours[0], x, y);
            }
            else
            {
                int randomneighbour = UnityEngine.Random.Range(0, neighbours.Count);
                SetValues(neighbours[randomneighbour], x, y);
            }

            PickAndGo();
        }
    }

    /// <summary>
    ///      
    /// </summary>
    /// <param name="position">neighbour position</param>
    /// <param name="originX">last node selected X</param>
    /// <param name="originY">last node selected Y</param>
    private void SetValues(string position, int originX, int originY)
    {
        string[] index = position.Split(',');
        int x = Int32.Parse(index[0]);
        int y = Int32.Parse(index[1]);

        if (x > originX) {
            grid[originX + 1, y] = 1;
        } else if (x < originX)
        {
            grid[originX - 1, y] = 1;
        } else if(y > originY)
        {
            grid[x, originY + 1] = 1;
        }
        else if (y < originY)
        {
            grid[x, originY - 1] = 1;
        }
        grid[x, y] = 1;

        gridElements.Add(x + "," + y);
    }

    /// <summary>
    ///     prints the maze board
    /// </summary>
    public void DrawMaze()
    {
        for (int i = 0; i < Witdh; i++ )
        {
            for (int j = 0; j < Height; j++)
            {
                if (grid[i, j] == 0)
                {
                  GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.position = new Vector3(i, j, 0);
                }
            }
        }
        Camera.main.transform.position = new Vector3(Witdh / 2, Height / 2, -Height);
    }
}