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
using System.Drawing;

using Iron.BSPUtils;
using Iron.WADUtils;
using IronClient.Geometry;
using IronClient.Renderer;
using IronClient.VFS;
using Tao.OpenGl;
using Tao.Glfw;

namespace IronClient
{
	public static class HexExtensions
	{
		public static string ToHex(this int i)
		{
			return string.Format("0x{0}", Convert.ToString(i, 16));
		}
		public static string ToHex(this uint i)
		{
			return string.Format("0x{0}", Convert.ToString(i, 16));
		}
		public static string ToHex(this long i)
		{
			return string.Format("0x{0}", Convert.ToString(i, 16));
		}
	}
	public class ViewPoint
	{
		public ViewPoint()
		{
			Rotation = new bool[3][];
			Rotation[0] = new bool[2];
			Rotation[1] = new bool[2];
			Rotation[2] = new bool[2];
			
			RotationValue = new float[3];
			
			Translation = new bool[3][];
			Translation[0] = new bool[2];
			Translation[1] = new bool[2];
			Translation[2] = new bool[2];
			
			TranslationValue = new float[3];
		}
		
		public bool[][] Rotation { get; protected set; }
		public float[] RotationValue { get; protected set; }
		public bool[][] Translation { get; protected set; }
		public float[] TranslationValue { get; protected set; }
		public bool Shift { get; set; }
		
		public void Rotate()
		{
			// TODO: get rid of opengl code
			Gl.glRotatef(RotationValue[0], 1.0f, 0.0f, 0.0f);
			Gl.glRotatef(RotationValue[1], 0.0f, 1.0f, 0.0f);
			Gl.glRotatef(RotationValue[2], 0.0f, 0.0f, 1.0f);
		}
		
		public void Translate()
		{
			// TODO: get rid of the opengl code
			Gl.glTranslatef(TranslationValue[0], TranslationValue[1], TranslationValue[2]);
		}
		
		public void Update()
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					if (Translation[i][j]) TranslationValue[i] += (j == 1 ? 0.1f : -0.1f);	
					if (Rotation[i][j]) RotationValue[i] += (j == 1 ? 1.0f : -1.0f);
				}
			}
		}
		
		public void SetKeys(int integer, int action)
		{
			
			// Console.WriteLine ("{0} {1}", integer, action);
			switch (integer)
			{
			case (int)'A':
				Translation[0][0] = action == 1;
				break;
			case (int)'D':
				Translation[0][1] = action == 1;
				break;
			case (int)'W':
				Translation[1][0] = action == 1;
				break;
			case (int)'S':
				Translation[1][1] = action == 1;
				break;
			case (int)'Z':
				if (Shift) {
					Rotation[2][0] = action == 1;
				} else {
					Rotation[2][1] = action == 1;	
				}
				
				break;
				
				
			case Glfw.GLFW_KEY_LEFT:
				Rotation[0][0] = action == 1;
				break;
			case Glfw.GLFW_KEY_RIGHT:
				Rotation[0][1] = action == 1;
				break;
			case Glfw.GLFW_KEY_UP:
				Rotation[1][0] = action == 1;
				break;
			case Glfw.GLFW_KEY_DOWN:
				Rotation[1][1] = action == 1;
				break;
			case 287:
				Shift = action == 1;
				break;
			case 32:
				if (Shift) {
					Translation[2][1] = action == 1 && Shift;
				} else {
					Translation[2][0] = action == 1;	
				}
				break;
			}
		}
	}
	
	class MainClass
	{
		private static bool sizeup = false;
		private static bool sizedown = false;
		private static float size = 1.0f;
		private static ViewPoint vp = new ViewPoint();
		
		private static long describeLast = 0;
		public static void Describe(WADFile wf, Iron.WADUtils.MipTexture mt)
		{
			Console.WriteLine ("Tarpas: {0}",(wf.offset - describeLast).ToHex() );
			Console.WriteLine ("WADFile: {0}", wf.filename);			
			Console.WriteLine ("\tOffset:{0}", wf.offset.ToHex());
			Console.WriteLine ("MipTexture {0} {1}x{2}", mt.name, mt.width, mt.height);
			
			Console.WriteLine ("\tTextureSizes: {0} {1} {2} {3}", mt.TextureSize1.ToHex(), mt.TextureSize2.ToHex(), mt.TextureSize3.ToHex(), mt.TextureSize4.ToHex());
			Console.WriteLine ("\tDataSize: {0}", mt.DataSize.ToHex());
			Console.WriteLine ("\toffset1 start: {0}\t{1}", mt.offset1.ToHex(), (wf.offset + mt.offset1).ToHex());
			Console.WriteLine ("\toffset1   end: {0}\t{1}", (mt.offset1 + mt.TextureSize1).ToHex(), (wf.offset + mt.offset1 + mt.TextureSize1).ToHex()); 
			Console.WriteLine ("\toffset2 start: {0}\t{1}", mt.offset2.ToHex(), (wf.offset + mt.offset2).ToHex());
			Console.WriteLine ("\toffset2   end: {0}\t{1}", (mt.offset2 + mt.TextureSize2).ToHex(), (wf.offset + mt.offset2 + mt.TextureSize1).ToHex()); 
			Console.WriteLine ("\toffset3 start: {0}\t{1}", mt.offset3.ToHex(), (wf.offset + mt.offset3).ToHex());
			Console.WriteLine ("\toffset3   end: {0}\t{1}", (mt.offset3 + mt.TextureSize3).ToHex(), (wf.offset + mt.offset3 + mt.TextureSize1).ToHex()); 
			Console.WriteLine ("\toffset4 start: {0}\t{1}", mt.offset4.ToHex(), (wf.offset + mt.offset4).ToHex());
			Console.WriteLine ("\toffset4   end: {0}\t{1}", (mt.offset4 + mt.TextureSize4).ToHex(), (wf.offset + mt.offset4 + mt.TextureSize4).ToHex()); 						
			describeLast = (wf.offset + mt.offset4 + mt.TextureSize4);
			
			Console.WriteLine ();
		}
		
		public static void ConvertToPNG(WADParser wp, Iron.WADUtils.WADFile wf, Iron.WADUtils.MipTexture mt, string name)
		{
			byte[] texture = wp.LoadTexture1(wf, mt);
			byte[][] palette = wp.LoadPallete(wf, mt);
			
			Console.WriteLine ("{0}x{1}", mt.width, mt.height);
			Console.WriteLine ("{0}", texture.Length);
			
			Bitmap b = new Bitmap(mt.width, mt.height);
			for (int i = 0; i < b.Width; i++)
			{
				for (int j = 0; j < b.Height; j++)
				{
					int index = mt.width * j + i;
					int color = texture[index];
					//int color = texture[i * mt.height * j];
					byte[] colors = palette[color];		
					
					//Console.WriteLine ("color({0}) = rgb({1},{2},{3})", color, colors[0], colors[1], colors[2]);
					
					b.SetPixel(i, j, System.Drawing.Color.FromArgb(colors[0], colors[1], colors[2]));
				}
			}
			b.Save(name);
		}
		
		public static void Main (string[] args)
		{	
			WADParser wp = new WADParser(File.OpenRead("halflife.wad"));
			WADFile f;
			
			/*
			for (int i = 0; i < wp.FileCount; i++)
			{
				f = wp.LoadFile(i);
				Iron.WADUtils.MipTexture mt = wp.LoadMipTexture(f);
				ConvertToPNG(wp, f, mt, string.Format("halflife{0}.png", i));
			}
			*/
			
			
			IronClient.Renderer.Renderer renderer = new IronClient.Renderer.OpenGL.OGLRenderer();
			
			if (!renderer.CreateWindow(1280, 780, false))
				return;

			StaticMesh mesh = new StaticMesh();
			mesh.vertices = new Vertex[] { new Vertex(0.0f, 1.0f, 0.0f), new Vertex(-1.0f, -1.0f, 0.0f), 
				new Vertex(1.0f, -1.0f, 0.0f)};
			mesh.texCoords0 = new TexCoord[] { new TexCoord(0.5f, 0.0f), new TexCoord(0.0f, 1.0f), 
				new TexCoord(1.0f, 1.0f) };
			mesh.MeshType = MeshType.TRIANGLES;
						
			//FileSystem fs = FileSystem.GetInstance();
			//fs.AddArchive(new WADArchive("halflife.wad"));
			// TODO: Get this working Giedriau!
			//mesh.Material = MaterialManager.getInstance().CreateTextureMaterial(fs.Get("{GRASS1"));
			
			List<Vector3f> vertexList = new List<Vector3f>();
			List<Edge> edgeList = new List<Edge>();
			BSPParser p = new BSPParser(File.OpenRead("datacore.bsp"));			
			
			if (p.LoadDirectoryTables())
			{
				p.OnLoadVertex += delegate(Vector3f vertex) { vertexList.Add(vertex); };
				p.LoadVertices();
				
				p.OnLoadEdge += delegate(Edge edge) { edgeList.Add(edge); };
				p.LoadEdges();
			}
									
			while (renderer.IsOpen()) {
				
				renderer.Clear();
				
				vp.Rotate();
				
				Gl.glTranslatef(0.0f, 0.0f, 0.0f);

				DrawLines();
				
				vp.Translate();
				
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
								
				Gl.glEnd();
				
				Glfw.glfwSetKeyCallback(keyboard);
				
				AdjustView();
				vp.Update();
								
				//renderer.DrawStaticMesh(mesh);
				
				renderer.Render();
			}
		}
		
		public static void keyboard(int integer, int action)
		{
			
			
			switch (integer)
			{
			case 45:
				sizedown = action == 1;
				break;
			case 61:
				sizeup = action == 1;
				break;
			default:
				vp.SetKeys(integer, action);
				break;
			}

		}
		
		public static void AdjustView()
		{
			if (sizeup) size += 0.1f;
			if (sizedown) size -= 0.1f;
		}
				                        
		public static void DrawLines()
		{
			// TODO: get rid of opengl code
			
						
			Gl.glLineWidth(5.0f);
			
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

			Gl.glLineWidth(1.0f);
			
		}
	}
}

