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
using System.Drawing;

using Iron.BSPUtils;

#if DEBUG

namespace Iron
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
	
	public class Debugger
	{
		private static long describeLast = 0;
		/// <summary>
		/// Describes A WADFiles entry miptexture with all its offsets and data
		/// </summary>
		/// <param name="wf">
		/// A <see cref="WADFile"/>
		/// </param>
		/// <param name="mt">
		/// A <see cref="Iron.WADUtils.MipTexture"/>
		/// </param>
		public static void Describe(WADFile wf, MipTexture mt)
		{
			Console.WriteLine("Tarpas: {0}",(wf.offset - describeLast).ToHex() );
			Console.WriteLine("WADFile: {0}", wf.filename);			
			Console.WriteLine("\tOffset:{0}", wf.offset.ToHex());
			Console.WriteLine("MipTexture {0} {1}x{2}", mt.name, mt.width, mt.height);
			
			Console.WriteLine("\tTextureSizes: {0} {1} {2} {3}", mt.TextureSize1.ToHex(), mt.TextureSize2.ToHex(), mt.TextureSize3.ToHex(), mt.TextureSize4.ToHex());
			Console.WriteLine("\tDataSize: {0}", mt.DataSize.ToHex());
			Console.WriteLine("\toffset1 start: {0}\t{1}", mt.offset1.ToHex(), (wf.offset + mt.offset1).ToHex());
			Console.WriteLine("\toffset1   end: {0}\t{1}", (mt.offset1 + mt.TextureSize1).ToHex(), (wf.offset + mt.offset1 + mt.TextureSize1).ToHex()); 
			Console.WriteLine("\toffset2 start: {0}\t{1}", mt.offset2.ToHex(), (wf.offset + mt.offset2).ToHex());
			Console.WriteLine("\toffset2   end: {0}\t{1}", (mt.offset2 + mt.TextureSize2).ToHex(), (wf.offset + mt.offset2 + mt.TextureSize1).ToHex()); 
			Console.WriteLine("\toffset3 start: {0}\t{1}", mt.offset3.ToHex(), (wf.offset + mt.offset3).ToHex());
			Console.WriteLine("\toffset3   end: {0}\t{1}", (mt.offset3 + mt.TextureSize3).ToHex(), (wf.offset + mt.offset3 + mt.TextureSize1).ToHex()); 
			Console.WriteLine("\toffset4 start: {0}\t{1}", mt.offset4.ToHex(), (wf.offset + mt.offset4).ToHex());
			Console.WriteLine("\toffset4   end: {0}\t{1}", (mt.offset4 + mt.TextureSize4).ToHex(), (wf.offset + mt.offset4 + mt.TextureSize4).ToHex()); 						
			describeLast = (wf.offset + mt.offset4 + mt.TextureSize4);
			Console.WriteLine ();
		}
		
		/// <summary>
		/// Converts a MipTexture to a PNG bitmap
		/// </summary>
		/// <param name="wp">
		/// A <see cref="WADParser"/>
		/// </param>
		/// <param name="wf">
		/// A <see cref="Iron.WADUtils.WADFile"/>
		/// </param>
		/// <param name="mt">
		/// A <see cref="Iron.WADUtils.MipTexture"/>
		/// </param>
		/// <param name="name">
		/// A <see cref="System.String"/>
		/// </param>
		/// 
		public static Bitmap ConvertToPNG(WADParser wp, WADFile wf, MipTexture mt)
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
					byte[] colors = palette[color];		
					
					//Console.WriteLine ("color({0}) = rgb({1},{2},{3})", color, colors[0], colors[1], colors[2]);
					
					b.SetPixel(i, j, System.Drawing.Color.FromArgb(colors[0], colors[1], colors[2]));
				}
			}
			return b;
		}
		
		/// <summary>
		/// Exports all wall's from a wad into png
		/// </summary>
		/// <param name="wadfilename">
		/// A <see cref="System.String"/>
		/// </param>
		public static void ConvertWADtoPNGs(string wadfilename)
		{
			WADParser wp = new WADParser(File.OpenRead(wadfilename));
			
			for (int i = 0; i < wp.FileCount; i++)
			{
				WADFile f = wp.LoadFile(i);
				MipTexture mt = wp.LoadMipTexture(f);
				Bitmap bmp = ConvertToPNG(wp, f, mt);
				
				bmp.Save(string.Format("{0}.png", i));
			}
		}
	}
}

#endif