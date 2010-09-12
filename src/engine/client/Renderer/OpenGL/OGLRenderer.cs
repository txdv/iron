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
using System.Runtime.InteropServices;
using Tao.Glfw;
using Tao.OpenGl;

using IronClient.Geometry;

namespace IronClient.Renderer.OpenGL
{
	public class OGLRenderer : Renderer
	{
		public OGLRenderer ()
		{
			Glfw.glfwInit();
		}
		
		public bool CreateWindow(int width, int height, bool fullscreen) {
			if (Glfw.glfwOpenWindow(width, height, 8, 8, 8, 0, 8, 0, fullscreen ? (Glfw.GLFW_FULLSCREEN) : (Glfw.GLFW_WINDOW)) == Gl.GL_FALSE) {
				Glfw.glfwTerminate();
				return false;
			}
			
			Glfw.glfwSetWindowTitle("Iron");
			Glfw.glfwEnable(Glfw.GLFW_KEY_REPEAT);
			Glfw.glfwSwapInterval(0);
			
			Gl.glDisable(Gl.GL_LIGHTING);
			Gl.glLoadIdentity();
			
			Gl.glShadeModel(Gl.GL_SMOOTH);
			
			Gl.glTranslatef(-1.5f, 0.0f, -6.0f);
			
			return true;
		}
		
		public bool IsOpen() {
			return Glfw.glfwGetWindowParam(Glfw.GLFW_OPENED) == Gl.GL_TRUE;
		}
		
		public void Close() {
			Glfw.glfwTerminate();
		}
		
		public void Clear() {
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			
			Gl.glViewport     ( 0, 0, 800, 600 );
  			Gl.glMatrixMode   ( Gl.GL_PROJECTION );
			Gl.glLoadIdentity();
			Glu.gluPerspective ( 45.0f, 1.333333, 0.1f, 100.0 );
			Gl.glMatrixMode   ( Gl.GL_MODELVIEW );
			Gl.glLoadIdentity(); 
			Gl.glTranslatef(-1.5f, 0.0f, -6.0f);
			
		}
		
		public void Render() {
			Glfw.glfwSwapBuffers();
		}
		
		/* To be used for VBO binding */
		public void RegisterPolygon(StaticMesh mesh) {
			/*
			Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
			Gl.glEnableClientState(Gl.GL_TEXTURE_COORD_ARRAY);
			
			Gl.glGenBuffers(1, out mesh.VBuffer);
			Gl.glGenBuffers(1, out mesh.texCoords0);
			
			
			Gl.glDisableClientState(Gl.GL_VERTEX_ARRAY);
			Gl.glDisableClientState(Gl.GL_TEXTURE_COORD_ARRAY);*/
		}
		
		public void DrawStaticMesh(StaticMesh mesh) {
			//uint buffer0, buffer1;
			
			if (mesh.Material.Type == Material.TEXTURE0) {
				Gl.glEnable(Gl.GL_TEXTURE_2D);
				Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
				Gl.glEnableClientState(Gl.GL_TEXTURE_COORD_ARRAY);
				
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, mesh.Material.Texture0);
				Gl.glVertexPointer(mesh.vertices.Length, Gl.GL_FLOAT, 0, mesh.vertices);
				Gl.glTexCoordPointer(mesh.texCoords0.Length, Gl.GL_FLOAT, 0, mesh.texCoords0);
				
				if (mesh.MeshType == MeshType.TRIANGLES)
					Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, mesh.vertices.Length);	
				else if (mesh.MeshType == MeshType.QUADS) 
					Gl.glDrawArrays(Gl.GL_QUADS, 0, mesh.vertices.Length);	
				else Gl.glDrawArrays(Gl.GL_POLYGON, 0, mesh.vertices.Length);	
				
				Gl.glDisableClientState(Gl.GL_VERTEX_ARRAY);
				Gl.glDisableClientState(Gl.GL_TEXTURE_COORD_ARRAY);
			} else throw new Exception("Unsupported material type");
		}
	}
}

