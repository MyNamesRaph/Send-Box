using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MarchingCubes
{
	List<int> triangles = new List<int>();

	/// <summary>
	/// Takes a set of vertices and calculates the position of the new
	/// </summary>
	/// <param name="vertices">vertices representing a grid of points in 3D space, xyz:position, w:density</param>
	/// <param name="newVertices">resulting positions of the vertices</param>
	/// <returns>Integer array representing the triangles to draw</returns>
	public static int[] FindTriangles(Vector4[,,] vertices, out List<Vector3> newVertices,int width,int height)
	{
		List<int> triangleList = new List<int>();
		newVertices = new List<Vector3>();


		//TODO: make it work for any number of vertices;

        for (int z = 0; z < vertices.GetLength(2)-1; z++)
        {
			for (int y = 0; y < vertices.GetLength(1)-1; y++)
			{
				for (int x = 0; x < vertices.GetLength(0)-1; x++)
				{
					List<Vector4> iteratorVertices = new List<Vector4>
					{
						vertices[x,y,z], //0,0,0
						vertices[x+1,y,z],//1,0,0
						vertices[x,y+1,z],//0,1,0
						vertices[x+1,y+1,z],//1,1,0
						vertices[x,y,z+1], //0,0,1
						vertices[x+1,y,z+1],//1,0,1
						vertices[x,y+1,z+1],//0,1,1
						vertices[x+1,y+1,z+1],//1,1,1
					};
					
					int[] edges = Tables.marchingCubesEdges[GetLookupIndex(iteratorVertices)];
					int iterator = 0;
					bool isOver = false;
					while (true)
					{
						switch (edges[iterator])
						{
							case -1:
								isOver = true;
								break;
							case 0:
								newVertices.Add(new Vector3(iteratorVertices[0].x + iteratorVertices[2].x, iteratorVertices[0].y + iteratorVertices[2].y, iteratorVertices[0].z + iteratorVertices[2].z) * 0.5F);
								break;
							case 1:
								newVertices.Add(new Vector3(iteratorVertices[6].x + iteratorVertices[2].x, iteratorVertices[6].y + iteratorVertices[2].y, iteratorVertices[6].z + iteratorVertices[2].z) * 0.5F);
								break;
							case 2:
								newVertices.Add(new Vector3(iteratorVertices[6].x + iteratorVertices[4].x, iteratorVertices[6].y + iteratorVertices[4].y, iteratorVertices[6].z + iteratorVertices[4].z) * 0.5F);
								break;
							case 3:
								newVertices.Add(new Vector3(iteratorVertices[0].x + iteratorVertices[4].x, iteratorVertices[0].y + iteratorVertices[4].y, iteratorVertices[0].z + iteratorVertices[4].z) * 0.5F);
								break;
							case 4:
								newVertices.Add(new Vector3(iteratorVertices[1].x + iteratorVertices[3].x, iteratorVertices[1].y + iteratorVertices[3].y, iteratorVertices[1].z + iteratorVertices[3].z) * 0.5F);
								break;
							case 5:
								newVertices.Add(new Vector3(iteratorVertices[7].x + iteratorVertices[3].x, iteratorVertices[7].y + iteratorVertices[3].y, iteratorVertices[7].z + iteratorVertices[3].z) * 0.5F);
								break;
							case 6:
								newVertices.Add(new Vector3(iteratorVertices[7].x + iteratorVertices[5].x, iteratorVertices[7].y + iteratorVertices[5].y, iteratorVertices[7].z + iteratorVertices[5].z) * 0.5F);
								break;
							case 7:
								newVertices.Add(new Vector3(iteratorVertices[1].x + iteratorVertices[5].x, iteratorVertices[1].y + iteratorVertices[5].y, iteratorVertices[1].z + iteratorVertices[5].z) * 0.5F);
								break;
							case 8:
								newVertices.Add(new Vector3(iteratorVertices[1].x + iteratorVertices[0].x, iteratorVertices[1].y + iteratorVertices[0].y, iteratorVertices[1].z + iteratorVertices[0].z) * 0.5F);
								break;
							case 9:
								newVertices.Add(new Vector3(iteratorVertices[2].x + iteratorVertices[3].x, iteratorVertices[2].y + iteratorVertices[3].y, iteratorVertices[2].z + iteratorVertices[3].z) * 0.5F);
								break;
							case 10:
								newVertices.Add(new Vector3(iteratorVertices[6].x + iteratorVertices[7].x, iteratorVertices[6].y + iteratorVertices[7].y, iteratorVertices[6].z + iteratorVertices[7].z) * 0.5F);
								break;
							case 11:
								newVertices.Add(new Vector3(iteratorVertices[4].x + iteratorVertices[5].x, iteratorVertices[4].y + iteratorVertices[5].y, iteratorVertices[4].z + iteratorVertices[5].z) * 0.5F);
								break;
							default:
								throw new Exception("Error in marching cubes tables");
						}
						if (isOver)
							break;
						triangleList.Add(newVertices.Count-1);
						++iterator;
					}
				}
			}
		}

		
		return triangleList.ToArray();
	}

	/// <summary>
	/// Gets the lookup table index for an array of 8 vertices;
	/// </summary>
	/// <param name="vertices">The 8 vertices that represent the current cube</param>
	/// <returns></returns>
	private static int GetLookupIndex(List<Vector4> vertexCube)
	{
		if (vertexCube.Count != 8)
		{
			throw new System.Exception();
		}
		float[] densities = new float[]
		{
			vertexCube[0].w,
			vertexCube[2].w,
			vertexCube[6].w,
			vertexCube[4].w,
			vertexCube[1].w,
			vertexCube[3].w,
			vertexCube[7].w,
			vertexCube[5].w
		};

		int output = 0;
		for (int i = densities.Length - 1; i >= 0; i--)
		{
			//Debug.Log(densities[i]);
			if (densities[i] >= 0.5F)
				output = output | (1 << i);
		}
		//Debug.Log(Convert.ToString(output, 2));
		return output;
	}
}