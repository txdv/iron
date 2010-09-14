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
using Tao.DevIl;
using Tao.OpenGl;

using Iron.Client.VFS;

namespace Iron.Client.Renderer
{
	public class MaterialManager
	{
		static MaterialManager instance;
		
		public static MaterialManager getInstance()
		{
			if (instance != null)
				return instance;
			else return instance = new MaterialManager();
		}
		
		private MaterialManager()
		{
			Il.ilInit();
		}
		
		public Material CreateTextureMaterial(SizeStream texture)
		{
			byte[] textureData = new byte[texture.Size];
			int iltexid = Il.ilGenImage();
			int gltexid = 0;
			
			
			Il.ilBindImage(iltexid);
			
			texture.InputStream.Read(textureData, 0, texture.Size);
			
			if ((textureData[0] == 0x89) && (textureData[1] == 0x50)) {
				Il.ilLoadL(Il.IL_PNG, textureData, texture.Size);
			} else if ((textureData[0] == 0xFF) && (textureData[1] == 0xD8)) {
				Il.ilLoadL(Il.IL_JPG, textureData, texture.Size);
			} else if ((textureData[0] == 'B') && (textureData[1] == 'M')) {
				Il.ilLoadL(Il.IL_BMP, textureData, texture.Size);
			} else {
				Il.ilLoadL(Il.IL_WAL, textureData, texture.Size);
			}
			
			// Il.IL_WAL
			
			Gl.glGenTextures(1, out gltexid);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, gltexid);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
			Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP),
			                Il.ilGetInteger(Il.IL_IMAGE_WIDTH), Il.ilGetInteger(Il.IL_IMAGE_HEIGHT),
			                0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, Il.ilGetData());
			
			
			Material material = new Material(Material.TEXTURE0, gltexid);
			
			Il.ilDeleteImage(iltexid);
			
			return material;
		}
	}
}

