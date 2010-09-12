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
using Iron.WADUtils;
using Iron.BSPUtils;

namespace Iron.WADUtilsConsole
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			LoadBSPMap("de_dust.bsp");
			Console.WriteLine ();
			LoadWADFile("halflife.wad");
		}
		
		public static void LoadWADFile(string filename)
		{
			Console.WriteLine("Parsing WAD file: {0}\n", filename);
			FileStream fs = File.OpenRead(filename);
			
			WADParser wadp = new WADParser(fs);
			Console.WriteLine("Files in directory: {0}", wadp.FileCount);
			
			wadp.OnLoadFile += delegate(WADFile file) { };
			Console.WriteLine("Loading file information");
			wadp.LoadFiles();
			
			fs.Close();
			fs.Dispose();			
		}
		
		
		public static void LoadBSPMap(string file)
		{
			FileStream fs = File.OpenRead(file);
			Console.WriteLine ("Parsing bsp file: {0}\n", fs.Name);
			BSPParser p = new BSPParser(fs);
			Console.WriteLine ("Loading Directory table");
			if (p.LoadDirectoryTables())
			{
				p.OnLoadEntities += delegate(string entities) { };
				Console.WriteLine ("Loading Entities");
				p.LoadEntities();
				
				p.OnLoadPlane += delegate(Plane plane) { };
				Console.WriteLine ("Loading Planes");
				p.LoadPlanes();
				
				p.OnLoadMipTexture += delegate(MipTexture texture) { };
				if (p.LoadMipTextureOffsets())
				{
					Console.WriteLine ("Loading Mip Textures");
					p.LoadMipTextures();
				}
				
				p.OnLoadVertex += delegate(Vector3f vertex) { };
				Console.WriteLine ("Loading Vertices");
				p.LoadVertices();
				
				p.OnLoadBSPNode += delegate(BSPNode node) { };
				Console.WriteLine ("Loading BSP nodes");
				p.LoadBSPNodes();
				
				p.OnLoadFaceTextureInfo += delegate(FaceTextureInfo textureInfo) { };
				Console.WriteLine ("Loading Surface Texture Info");
				p.LoadFaceTextureInfo();
				
				p.OnLoadFace += delegate(Face face) { };
				Console.WriteLine ("Loading Faces");
				p.LoadFaces();
				
				p.OnLoadLightMap += delegate(byte lightmap) { };
				Console.WriteLine ("Loading lightmaps");
				p.LoadLightMaps();
				
				p.OnLoadClipNode += delegate(ClipNode node) { };
				Console.WriteLine ("Loading ClipNode");
				p.LoadClipNodes();
				
				p.OnLoadBSPLeaf += delegate(BSPLeaf leaf) { };
				Console.WriteLine ("Loading BSP leaves");
				p.LoadBSPLeaves();
				
				p.OnLoadFaceListElement += delegate(short face_index) { };
				Console.WriteLine ("Loading Face index list");
				p.LoadFaceList();
				
				p.OnLoadEdge += delegate(Edge edge) { };
				Console.WriteLine ("Loading Edges");
				p.LoadEdges();
				
				p.OnLoadEdgeListElement += delegate(short edge) { };
				Console.WriteLine ("Loading Edge List");
				p.LoadEdgeList();
				
				p.OnLoadModel += delegate(Model model) { };
				Console.WriteLine ("Loading Models");
				p.LoadModels();
			}
			
			fs.Close();
			fs.Dispose();
		}
	}
}