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
using System.Text;
using Iron.BinaryExtensions;

namespace Iron.BSPUtils
{
	/*
	 * GoldSrc has bsp map version 30(0x1E)
	 */
		
	public class BSPParser
	{
		#region Delegates
		//public delegate void LoadDirectoryTableDelegate();
		public delegate void LoadEntitiesDelegate(string entities);
		public delegate void LoadPlaneDelegate(Plane plane);
		public delegate void LoadMipTextureDelegate(MipTexture texture);
		public delegate void LoadVertexDelegate(Vector3f vertex);
		public delegate void LoadBSPNode(BSPNode node);
		public delegate void LoadFaceTextureInfoDelegate(FaceTextureInfo textureInfo);
		public delegate void LoadFaceDelegate(Face face);
		public delegate void LoadLightMapDelegate(byte lightmap);
		public delegate void LoadClipNodeDelegate(ClipNode node);
		public delegate void LoadBSPLeafDelegate(BSPLeaf leaf);
		public delegate void LoadFaceListElementDelegate(short face_index);
		public delegate void LoadEdgeDelegate(Edge edge);
		public delegate void LoadEdgeListElementDelegate(short edge);
		public delegate void LoadModelDelegate(Model model);
		#endregion
		
		#region Events
		//public event LoadDirectoryTableDelegate OnLoadDirectoryTable;
		public event LoadEntitiesDelegate        OnLoadEntities;
		public event LoadPlaneDelegate           OnLoadPlane;
		public event LoadMipTextureDelegate      OnLoadMipTexture;
		public event LoadVertexDelegate          OnLoadVertex;
		public event LoadBSPNode                 OnLoadBSPNode;
		public event LoadFaceTextureInfoDelegate OnLoadFaceTextureInfo;
		public event LoadFaceDelegate            OnLoadFace;
		public event LoadLightMapDelegate        OnLoadLightMap;
		public event LoadClipNodeDelegate        OnLoadClipNode;
		public event LoadBSPLeafDelegate         OnLoadBSPLeaf;
		public event LoadFaceListElementDelegate OnLoadFaceListElement;
		public event LoadEdgeDelegate            OnLoadEdge;
		public event LoadEdgeListElementDelegate OnLoadEdgeListElement;
		public event LoadModelDelegate           OnLoadModel;	    
	    #endregion
		
		private BinaryReader br;
		
		public BSPParser(Stream stream)
		{
			br = new BinaryReader(stream, Encoding.ASCII);
		}
		
		#region Load Functions
		
		public bool LoadDirectoryTables()
		{
			try 
			{
				Version = br.LReadInt32();
				
				// directories
				Entities       = br.BReadDirectoryEntry();
				Planes         = br.BReadDirectoryEntry();
				MipTextures    = br.BReadDirectoryEntry();
				Vertices       = br.BReadDirectoryEntry();
				VisibilityList = br.BReadDirectoryEntry();
				Nodes          = br.BReadDirectoryEntry();
				TextureInfo    = br.BReadDirectoryEntry();
				Faces          = br.BReadDirectoryEntry();
				Lightmaps      = br.BReadDirectoryEntry();
				Clipnodes      = br.BReadDirectoryEntry();
				Leaves         = br.BReadDirectoryEntry();
				FaceList       = br.BReadDirectoryEntry();
				Edges          = br.BReadDirectoryEntry();
				EdgeList       = br.BReadDirectoryEntry();
				Models         = br.BReadDirectoryEntry();
				
				//if (OnLoadDirectoryTable != null) OnLoadDirectoryTable();
			} catch { return false; }
			
			return true;
		}
		
		public bool LoadEntities()
		{
			if (OnLoadEntities != null) OnLoadEntities(ReadEntities());
			
			return true;
		}
		
		public string ReadEntities()
		{
			br.BaseStream.Seek(Entities.offset, SeekOrigin.Begin);			
			return Encoding.ASCII.GetString(br.ReadBytes((int)Entities.size));
		}
		
		public bool LoadPlanes()
		{
			br.BaseStream.Seek(Planes.offset, SeekOrigin.Begin);
			for (int i = Planes.offset; i < Planes.offset + Planes.size; i += Plane.Size)
			{
				Plane p;
				p.normal = br.ReadVector3f();
				p.distance = br.ReadSingle();
				p.type = br.LReadInt32();
				if (OnLoadPlane != null) OnLoadPlane(p);
			}
			return true;
		}
		
		public bool LoadMipTextureOffsets()
		{
			br.BaseStream.Seek(MipTextures.offset, SeekOrigin.Begin);
			int size = br.LReadInt32();
			MipTextureOffsets = new int[size];
			for (int j = 0; j < size; j++) MipTextureOffsets[j] = br.LReadInt32();
			return true;
		}
		public bool LoadMipTextures()
		{
			if (MipTextureOffsets == null) return false;
			foreach(int offset in MipTextureOffsets)
			{
				br.BaseStream.Seek(MipTextures.offset + offset, SeekOrigin.Begin);
				OnLoadMipTexture(br.BReadMipTexture());
			}
			return true;
		}
		
		public bool LoadVertices()
		{
			br.BaseStream.Seek(Vertices.offset, SeekOrigin.Begin);
			for (int i = Vertices.offset; i < Vertices.offset + Vertices.size; i += Vector3f.Size)
			{
				OnLoadVertex(br.ReadVector3f());
			}
			return true;
		}
		
		public bool LoadBSPNodes()
		{
			br.BaseStream.Seek(Nodes.offset, SeekOrigin.Begin);
			
			for (int i = Nodes.offset; i < Nodes.offset + Nodes.size; i += BSPNode.Size)
			{
				OnLoadBSPNode(br.BReadBSPNode());
			}
			
			return true;
		}
		
		public bool LoadFaceTextureInfo()
		{
			br.BaseStream.Seek(TextureInfo.offset, SeekOrigin.Begin);
			for (int i = TextureInfo.offset; i < TextureInfo.offset + TextureInfo.size; i += FaceTextureInfo.Size)
			{
				if (OnLoadFaceTextureInfo != null) OnLoadFaceTextureInfo(br.BReadFaceTextureInfo());
			}
			return true;
		}
		
		public bool LoadFaces()
		{
			br.BaseStream.Seek(Faces.offset, SeekOrigin.Begin);
			for (int i = Faces.offset; i < Faces.offset + Faces.size; i += Face.Size)
			{
				if (OnLoadFace != null) OnLoadFace(br.BReadFace());
			}
			return true;
		}
		
		public bool LoadLightMaps()
		{
			if (OnLoadLightMap != null)
			{
				br.BaseStream.Seek(Lightmaps.offset, SeekOrigin.Begin);
				for (int i = Lightmaps.offset; i < Lightmaps.offset + Lightmaps.size; i++) // one byte
					OnLoadLightMap(br.ReadByte());
			}
			return true;
		}
		
		public bool LoadClipNodes()
		{
			if (OnLoadClipNode != null)
			{
				br.BaseStream.Seek(Clipnodes.offset, SeekOrigin.Begin);
				for (int i = Clipnodes.offset; i < Clipnodes.offset + Clipnodes.size; i += ClipNode.Size)
					OnLoadClipNode(br.BReadClipNode());
			}
			return true;
		}
		
		public bool LoadBSPLeaves()
		{
			if (OnLoadBSPLeaf != null)
			{
				br.BaseStream.Seek(Leaves.offset, SeekOrigin.Begin);
				for (int i = Leaves.offset; i < Leaves.offset + Leaves.size; i += BSPLeaf.Size)
					OnLoadBSPLeaf(br.BReadBSPLeaf());
				
			}
			return true;
		}
		
		public bool LoadFaceList()
		{
			if (OnLoadFaceListElement != null)
			{
				br.BaseStream.Seek(FaceList.offset, SeekOrigin.Begin);
				for (int i = FaceList.offset; i < FaceList.offset + FaceList.size; i+= Face.Size)
					OnLoadFaceListElement(br.LReadInt16());
			}
			return true;
		}
		
		public bool LoadEdges()
		{
			if (OnLoadEdge != null)
			{
				br.BaseStream.Seek(Edges.offset, SeekOrigin.Begin);
				for (int i = Edges.offset; i < Edges.offset + Edges.size; i += Edge.Size)
					OnLoadEdge(br.BReadEdge());
			}
			return true;
		}
		
		public bool LoadEdgeList()
		{
			if (OnLoadEdgeListElement != null)
			{
				br.BaseStream.Seek(EdgeList.offset, SeekOrigin.Begin);
				for (int i = EdgeList.offset; i < EdgeList.offset + EdgeList.size; i += 2)
					OnLoadEdgeListElement(br.LReadInt16());
			}
			return true;
		}
		
		public bool LoadModels()
		{
			if (OnLoadModel != null)
			{
				br.BaseStream.Seek(Models.offset, SeekOrigin.Begin);
				for (int i = Models.offset; i < Models.offset + Models.size; i += Model.Size)
					OnLoadModel(br.BReadModel());
				
			}
			return true;
		}
		
		#endregion
		
		#region Public Fields
		
		public int Version { get; protected set; }
		public DirectoryEntry Entities { get; protected set; }
		public DirectoryEntry Planes { get; protected set; }
		public DirectoryEntry MipTextures { get; protected set; }
		public int[] MipTextureOffsets { get; protected set; }
		public DirectoryEntry Vertices { get; protected set; }
		public DirectoryEntry VisibilityList { get; protected set; }
		public DirectoryEntry Nodes { get; protected set; }
		public DirectoryEntry TextureInfo { get; protected set; }
		public DirectoryEntry Faces { get; protected set; }
		public DirectoryEntry Lightmaps { get; protected set; }
		public DirectoryEntry Clipnodes { get; protected set; }
		public DirectoryEntry Leaves { get; protected set; }
		public DirectoryEntry FaceList { get; protected set; }
		public DirectoryEntry Edges { get; protected set; }
		public DirectoryEntry EdgeList { get; protected set; }
		public DirectoryEntry Models { get; protected set; }
		
		#endregion
	}
}

