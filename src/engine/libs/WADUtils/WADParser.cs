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

namespace Iron.WADUtils
{
	public struct WADFile
	{
		public WADFile(uint offset, uint compressedFileSize, uint uncompressedFileSize,
		               byte fileType, byte compressionType, byte padding, byte padding2, string filename)
		{
			this.offset = offset;
			this.compressedFileSize = compressedFileSize;
			this.uncomrepssedFileSize = uncompressedFileSize;
			this.fileType = fileType;
			this.compressionType = compressionType;
			this.padding = padding;
			this.padding2 = padding;
			this.filename = filename;
		}
		public uint offset;
		public uint compressedFileSize;
		public uint uncomrepssedFileSize;
		public byte fileType;
		public byte compressionType;
		public byte padding;
		public byte padding2;
		public string filename;
	}
	
	public static class BinaryReaderExtensions
	{
		public static WADFile ReadWADFile(this BinaryReader br)
		{
			return new WADFile(br.BReadUInt32(), br.BReadUInt32(), br.BReadUInt32(), br.ReadByte(), 
			                   br.ReadByte(), br.ReadByte(), br.ReadByte(), 
			                   Encoding.ASCII.GetString(br.ReadBytes(16)).TrimEnd(new char[] { '\0' })
			                   );
		}
	}
	
	public class WADParser
	{
		public delegate void LoadFileDelegate(WADFile file);
		public event LoadFileDelegate OnLoadFile;
		
		public static readonly string MagicString = "WAD3";
		public static readonly int    MagicInt    = 1463895091;
		
		private BinaryReader br;
		
		public WADParser(Stream stream)
		{
			br = new BinaryReader(stream);
			
			// TODO: through exception on fake magic
			bool magic = CheckMagic(br.ReadBytes(4));
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
		
		public uint Offset { get; set; }
		public uint FileCount { get; set; }
	}
}

