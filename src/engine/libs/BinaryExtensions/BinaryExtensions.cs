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

namespace Iron.BinaryExtensions
{		
	public static class BinaryReaderExtensions
    {
        #region Little Endian

        public static int LReadInt32(this BinaryReader br)
		{
			byte b1 = br.ReadByte();
			byte b2 = br.ReadByte();
			byte b3 = br.ReadByte();
			byte b4 = br.ReadByte();
			
			return ((b4 << 24) | (b3 << 16) | (b2 << 8) | b1);
		}
		
		public static uint LReadUInt32(this BinaryReader br)
		{
			byte b1 = br.ReadByte();
			byte b2 = br.ReadByte();
			byte b3 = br.ReadByte();
			byte b4 = br.ReadByte();
			
			return (uint)((b4 << 24) | (b3 << 16) | (b2 << 8) | b1);
		}
		
		public static short LReadInt16(this BinaryReader br)
		{
			byte b1 = br.ReadByte();
			byte b2 = br.ReadByte();
			return (short)((b2 << 8) | b1);
		}
		
		public static ushort LReadUInt16(this BinaryReader br)
		{
			byte b1 = br.ReadByte();
			byte b2 = br.ReadByte();
			return (ushort)((b2 << 8) | b1);
		}
		
		#endregion
		
		#region Big Endian
		
		public static int BReadInt32(this BinaryReader br)
		{
			byte b1 = br.ReadByte();
			byte b2 = br.ReadByte();
			byte b3 = br.ReadByte();
			byte b4 = br.ReadByte();
			
			return ((b1 << 24) | (b2 << 16) | (b3 << 8) | b4);
		}
		
		public static uint BReadUInt32(this BinaryReader br)
		{
			byte b1 = br.ReadByte();
			byte b2 = br.ReadByte();
			byte b3 = br.ReadByte();
			byte b4 = br.ReadByte();
			
			return (uint)((b1 << 24) | (b2 << 16) | (b3 << 8) | b4);
		}
	
		public static short BReadInt16(this BinaryReader br)
		{
			byte b1 = br.ReadByte();
			byte b2 = br.ReadByte();
			return (short)((b1 << 8) | b2);
		}

		public static ushort BReadUInt16(this BinaryReader br)
		{
			byte b1 = br.ReadByte();
			byte b2 = br.ReadByte();
			return (ushort)((b1 << 8) | b2);
		}		
		#endregion
	}
}

