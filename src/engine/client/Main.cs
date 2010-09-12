// 
//     This file is part of Iron.
//     Iron is an extensive game engine written in C# aiming to
//     utilize already exitent game content 
// 
//     Copyright (C) 2010  Andrius Bentkus
//     Copyright (C) 2010  Giedrius Graževičius
// 
//     Iron is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Iron is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with Iron.  If not, see <http://www.gnu.org/licenses/>.
// 
using System;
using System.IO;
using System.Collections.Generic;

using Iron.BSPUtils;
using IronClient.Geometry;
using IronClient.Renderer;
using IronClient.VFS;
using Tao.OpenGl;
using Tao.Glfw;

namespace IronClient
{
	class MainClass
	{
		
		private static bool xrotleft = false;
		private static bool yrotleft = false;
		private static bool zrotleft = false;
		
		private static bool xrotright = false;
		private static bool yrotright = false;
		private static bool zrotright = false;
		
		private static float xrotvalue = 0.0f;
		private static float yrotvalue = 0.0f;
		private static float zrotvalue = 0.0f;
		
		private static bool xtranslationleft = false;
		private static bool ytranslationleft = false;
		private static bool ztranslationleft = false;

		private static bool xtranslationright = false;
		private static bool ytranslationright = false;
		private static bool ztranslationright = false;
		 
		private static float xtranslationvalue = 0.0f;
		private static float ytranslationvalue = 0.0f;
		private static float ztranslationvalue = 0.0f;
		
		private static bool sizeup = false;
		private static bool sizedown = false;
		private static float size = 1.0f;

		public static void Main (string[] args)
		{
			IronClient.Renderer.Renderer renderer = new IronClient.Renderer.OpenGL.OGLRenderer();
			
			if (!renderer.CreateWindow(800, 600, false))
				return;

			StaticMesh mesh = new StaticMesh();
			mesh.vertices = new Vertex[] { new Vertex(0.0f, 1.0f, 0.0f), new Vertex(-1.0f, -1.0f, 0.0f), 
				new Vertex(1.0f, -1.0f, 0.0f)};
			mesh.texCoords0 = new TexCoord[] { new TexCoord(0.5f, 0.0f), new TexCoord(0.0f, 1.0f), 
				new TexCoord(1.0f, 1.0f) };
			mesh.MeshType = MeshType.TRIANGLES;
						
			FileSystem fs = FileSystem.GetInstance();
			fs.AddZipArchive("test.zip");
			
			Stream stream = new FileStream("testtex.png", FileMode.Open);
			SizeStream ss = new SizeStream(stream, (int)stream.Length);
			mesh.Material = MaterialManager.getInstance().CreateTextureMaterial(fs.Get("bla.jpg"));
			
			List<Vector3f> vertexList = new List<Vector3f>();
			List<Edge> edgeList = new List<Edge>();
			BSPParser p = new BSPParser(File.OpenRead("de_dust.bsp"));
			if (p.LoadDirectoryTables())
			{
				p.OnLoadVertex += delegate(Vector3f vertex) { vertexList.Add(vertex); };
				p.LoadVertices();
				
				p.OnLoadEdge += delegate(Edge edge) { edgeList.Add(edge); };
				p.LoadEdges();
			}
									
			while (renderer.IsOpen()) {
				
				renderer.Clear();
				
				Gl.glRotatef(xrotvalue, 1, 0, 0);
            	Gl.glRotatef(yrotvalue, 0, 1, 0);
            	Gl.glRotatef(zrotvalue, 0, 0, 1);
				
				Gl.glTranslatef(0.0f, 0.0f, 0.0f);

				DrawLines();
				
				Gl.glTranslatef(xtranslationvalue, ytranslationvalue, ztranslationvalue);
				
				//Gl.glLineWidth(100.0f);
				
				Gl.glBegin(Gl.GL_LINES);
			
				
				Gl.glColor3f(1.0f, 1.0f, 1.0f);
				foreach (var edge in edgeList) {
					Gl.glVertex3f(vertexList[edge.vertex0].x/(float)Math.Pow(size, 2), 
					              vertexList[edge.vertex0].y/(float)Math.Pow(size, 2),
					              vertexList[edge.vertex0].z/(float)Math.Pow(size, 2));
					Gl.glVertex3f(vertexList[edge.vertex1].x/(float)Math.Pow(size, 2),
					              vertexList[edge.vertex1].y/(float)Math.Pow(size, 2),
					              vertexList[edge.vertex1].z/(float)Math.Pow(size, 2));
				}
								
				Gl.glEnd( );
				
				Glfw.glfwSetKeyCallback(keyboard);
				                      
				//Glfw.glfwSetKeyCallback(new Glfw.GLFWkeyfun(keyboard));
				
				AdjustView();
								
				renderer.DrawStaticMesh(mesh);
				
				renderer.Render();
			}
		}
		
		public static void AdjustView()
		{
			if (xrotleft) xrotvalue += 1.0f;
			if (xrotright) xrotvalue -= 1.0f;
			if (yrotleft) yrotvalue += 1.0f;
			if (yrotright) yrotvalue -= 1.0f;
			if (xtranslationleft) xtranslationvalue += 0.1f;
			if (xtranslationright) xtranslationvalue -= 0.1f;
			if (ytranslationleft) ytranslationvalue += 0.1f;
			if (ytranslationright) ytranslationvalue -= 0.1f;
			if (sizeup) size += 0.1f;
			if (sizedown) size -= 0.1f;
			
		}
		public static void keyboard(int integer, int action)
		{
			Console.WriteLine ("{0} {1}", integer, action);
			switch (integer)
			{
			case (int)'A':
				xtranslationleft = action == 1;
				break;
			case (int)'D':
				xtranslationright = action == 1;
				break;
			case (int)'W':
				ytranslationleft = action == 1;
				break;
			case (int)'S':
				ytranslationright = action == 1;
				break;
				
				
			case Glfw.GLFW_KEY_LEFT:
				xrotleft = action == 1;
				break;
			case Glfw.GLFW_KEY_RIGHT:
				xrotright = action == 1;
				break;
			case Glfw.GLFW_KEY_UP:
				yrotright = action == 1;
				break;
			case Glfw.GLFW_KEY_DOWN:
				yrotleft = action == 1;
				break;
				
			case 45:
				sizedown = action == 1;
				break;
			case 61:
				sizeup = action == 1;
				break;
			}
		}
				                        
				                        
		public static void DrawLines()
		{
		                // Draw the positive side of the lines x,y,z
            Gl.glBegin(Gl.GL_LINES);
            Gl.glColor3f(0.0f, 1.0f, 0.0f);                // Green for x axis
            Gl.glVertex3f(0f, 0f, 0f);
            Gl.glVertex3f(10f, 0f, 0f);
            Gl.glColor3f(1.0f, 0.0f, 0.0f);                // Red for y axis
            Gl.glVertex3f(0f, 0f, 0f);
            Gl.glVertex3f(0f, 10f, 0f);
            Gl.glColor3f(0.0f, 0.0f, 1.0f);                // Blue for z axis
            Gl.glVertex3f(0f, 0f, 0f);
            Gl.glVertex3f(0f, 0f, 10f);
            Gl.glEnd();

            // Dotted lines for the negative sides of x,y,z coordinates
            Gl.glEnable(Gl.GL_LINE_STIPPLE); // Enable line stipple to use a 
                                             // dotted pattern for the lines
            Gl.glLineStipple(1, 0x0101);     // Dotted stipple pattern for the lines
            Gl.glBegin(Gl.GL_LINES);
            Gl.glColor3f(0.0f, 1.0f, 0.0f);                    // Green for x axis
            Gl.glVertex3f(-10f, 0f, 0f);
            Gl.glVertex3f(0f, 0f, 0f);
            Gl.glColor3f(1.0f, 0.0f, 0.0f);                    // Red for y axis
            Gl.glVertex3f(0f, 0f, 0f);
            Gl.glVertex3f(0f, -10f, 0f);
            Gl.glColor3f(0.0f, 0.0f, 1.0f);                    // Blue for z axis
            Gl.glVertex3f(0f, 0f, 0f);
            Gl.glVertex3f(0f, 0f, -10f);
            Gl.glEnd();

			Gl.glDisable(Gl.GL_LINE_STIPPLE);
			Gl.glColor3f(1.0f, 1.0f, 1.0f);

		}
	}
}

