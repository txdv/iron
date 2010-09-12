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

using IronClient.Geometry;
using IronClient.Renderer;
using IronClient.VFS;

namespace IronClient
{
	class MainClass
	{
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
			mesh.Material = MaterialManager.getInstance().CreateTextureMaterial(fs.Get("testtex.png"));
			

			while (renderer.IsOpen()) {
				
				renderer.Clear();
				renderer.DrawStaticMesh(mesh);
				renderer.Render();
			}
		}
	}
}

