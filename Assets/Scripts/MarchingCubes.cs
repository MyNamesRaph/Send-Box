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
	public static int[] FindTriangles(List<Vector4> vertices, out List<Vector3> newVertices,int width,int height)
	{
		List<int> triangleList = new List<int>();
		int[] edges = Tables.marchingCubesEdges[GetLookupIndex(vertices)];
		//TODO: make it work for any number of vertices
		int[] Iterator3D = { 0, 1, width, width + 1,width*2+width*2+1};
        for (int n = 0; n < width-2; n++)
        {

        }
		newVertices = new List<Vector3>();
		int i = 0;
		bool isOver = false;
        while (true)
        {
            switch (edges[i])
            {
				case -1:
					isOver = true;
					break;
				case 0:
					newVertices.Add(new Vector3(vertices[0].x + vertices[2].x, vertices[0].y + vertices[2].y, vertices[0].z + vertices[2].z)*0.5F);
					break;
				case 1:
					newVertices.Add(new Vector3(vertices[6].x + vertices[2].x, vertices[6].y + vertices[2].y, vertices[6].z + vertices[2].z) * 0.5F);
					break;
				case 2:
					newVertices.Add(new Vector3(vertices[6].x + vertices[4].x, vertices[6].y + vertices[4].y, vertices[6].z + vertices[4].z) * 0.5F);
					break;
				case 3:
					newVertices.Add(new Vector3(vertices[0].x + vertices[4].x, vertices[0].y + vertices[4].y, vertices[0].z + vertices[4].z) * 0.5F);
					break;
				case 4:
					newVertices.Add(new Vector3(vertices[1].x + vertices[3].x, vertices[1].y + vertices[3].y, vertices[1].z + vertices[3].z) * 0.5F);
					break;
				case 5:
					newVertices.Add(new Vector3(vertices[7].x + vertices[3].x, vertices[7].y + vertices[3].y, vertices[7].z + vertices[3].z) * 0.5F);
					break;
				case 6:
					newVertices.Add(new Vector3(vertices[7].x + vertices[5].x, vertices[7].y + vertices[5].y, vertices[7].z + vertices[5].z) * 0.5F);
					break;
				case 7:
					newVertices.Add(new Vector3(vertices[1].x + vertices[5].x, vertices[1].y + vertices[5].y, vertices[1].z + vertices[5].z) * 0.5F);
					break;
				case 8:
					newVertices.Add(new Vector3(vertices[1].x + vertices[0].x, vertices[1].y + vertices[0].y, vertices[1].z + vertices[0].z) * 0.5F);
					break;
				case 9:
					newVertices.Add(new Vector3(vertices[2].x + vertices[3].x, vertices[2].y + vertices[3].y, vertices[2].z + vertices[3].z) * 0.5F);
					break;
				case 10:
					newVertices.Add(new Vector3(vertices[6].x + vertices[7].x, vertices[6].y + vertices[7].y, vertices[6].z + vertices[7].z) * 0.5F);
					break;
				case 11:
					newVertices.Add(new Vector3(vertices[4].x + vertices[5].x, vertices[4].y + vertices[5].y, vertices[4].z + vertices[5].z) * 0.5F);
					break;
				default:
					throw new Exception("Error in marching cubes tables");
            }
			if (isOver)
				break;
			triangleList.Add(i);
			++i;
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
			Debug.Log(densities[i]);
			if (densities[i] >= 0.5F)
				output = output | (1 << i);
			Debug.Log(Convert.ToString(output, 2));
		}
		return output;
	}
}