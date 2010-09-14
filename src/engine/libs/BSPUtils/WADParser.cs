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
	public class WADParser
	{
		public delegate void LoadFileDelegate(WADFile file);
		public event LoadFileDelegate OnLoadFile;
		
		public static readonly string MagicString = "WAD3";
		public static readonly int    MagicInt    = 1463895091;
		
		/// <summary>
		/// Returns true if the magic number fits
		/// </summary>
		public bool Magic { get; set; }				
		public uint Offset { get; set; }
		public uint FileCount { get; set; }		
		
		private BinaryReader br;
		
		public WADParser(Stream stream)
		{
			br = new BinaryReader(stream);
			
			Magic = CheckMagic(br.ReadBytes(4));
			
			FileCount = br.BReadUInt32();
			Offset = br.BReadUInt32();
		}
		
		public void LoadFiles()
		{
			if (OnLoadFile != null)
			{
				br.BaseStream.Seek(Offset, SeekOrigin.Begin);
				
				for (int i = 0; i < FileCount; i++)
				{
					 OnLoadFile(br.ReadWADFile());
				}
			}
		}
		
		public WADFile LoadFile(int index)
		{
			if ((index < 0) || (index > FileCount)) throw new Exception("Out of range");
			br.BaseStream.Seek(Offset + WADFile.Size * index, SeekOrigin.Begin);
			return br.ReadWADFile();
		}
		
		public static bool CheckMagic(byte[] bytes)
		{
			return CheckMagic(bytes, 0);
		}
		
		public static bool CheckMagic(byte[] bytes, int startindex)
		{
			if (bytes.Length < startindex + 4) return false;
			string m = Encoding.ASCII.GetString(bytes, startindex, 4);
			return (m == MagicString);
		}
		
		public byte[] LoadFile(WADFile file)
		{
			br.BaseStream.Seek(file.offset, SeekOrigin.Begin);
			return br.ReadBytes((int)file.compressedFileSize);
		}
		
		public MipTexture LoadMipTexture(WADFile file)
		{
			br.BaseStream.Seek(file.offset, SeekOrigin.Begin);
			return br.BReadMipTexture();
		}
		
		public byte[] LoadTexture1(WADFile file, MipTexture texture)
		{
			br.BaseStream.Seek(file.offset + texture.offset1, SeekOrigin.Begin);
			return br.ReadBytes(texture.TextureSize1);
		}
		
		public byte[] LoadTexture2(WADFile file, MipTexture texture)
		{
			br.BaseStream.Seek(file.offset + texture.offset2, SeekOrigin.Begin);
			return br.ReadBytes(texture.width * texture.height / 4);
		}

		public byte[] LoadTexture3(WADFile file, MipTexture texture)
		{
			br.BaseStream.Seek(file.offset + texture.offset3, SeekOrigin.Begin);
			return br.ReadBytes(texture.width * texture.height / 16);
		}
		
		public byte[] LoadTexture4(WADFile file, MipTexture texture)
		{
			br.BaseStream.Seek(file.offset + texture.offset4, SeekOrigin.Begin);
			return br.ReadBytes(texture.width * texture.height / 64);
		}
		
		public byte[][] LoadPallete(WADFile file, MipTexture texture)
		{
			br.BaseStream.Seek(file.offset + texture.offset1 + texture.DataSize + 2, SeekOrigin.Begin);
			byte[][] ret = new byte[256][];
			for (int i = 0; i < 256; i++)
			{
				ret[i] = new byte[3];
				ret[i][0] = br.ReadByte();
				ret[i][1] = br.ReadByte();
				ret[i][2] = br.ReadByte();
				
			}
			return ret;
		}
		public int GetColorsFromPallete(byte[][] pallette)
		{
			return 0;
		}
		
		public Stream GetTextureStream(WADFile file, MipTexture texture)
		{
			br.BaseStream.Seek(file.offset + texture.offset1, SeekOrigin.Begin);
			return br.BaseStream;
		}
	}}