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
using Tao.Glfw;
using Tao.OpenGl;

namespace IronClient.OpenGL
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
			
			return true;
		}
		
		public bool IsOpen() {
			return Glfw.glfwGetWindowParam(Glfw.GLFW_OPENED) == Gl.GL_TRUE;
		}
		
		public void Close() {
			Glfw.glfwTerminate();
		}
		
		public void Render() {
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Glfw.glfwSwapBuffers();
		}
	}
}

