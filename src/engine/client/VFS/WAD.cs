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
using System.Collections.Generic;
using Iron.BSPUtils;

namespace IronClient.VFS
{
	public class WADArchive : Archive
	{
		WADParser wp;
		List<WADFile> wadList;
		List<string> entriesList;
		
		
		public WADArchive(string filename)
		{
			FileStream fs = File.OpenRead(filename);
			wp = new WADParser(fs);
			if (!wp.Magic) throw new Exception("WAD file magic missmatch");
			
			wadList = new List<WADFile>();
			entriesList = new List<string>();
			
			wp.OnLoadFile += delegate(WADFile file) { 
				wadList.Add(file);
				entriesList.Add(file.filename); 
			};
			
			wp.LoadFiles();
						
		}
		
		public List<string> GetEntries()
		{			
			return entriesList;
		}
		
		public SizeStream Get(string path)
		{
			foreach (WADFile file in wadList) {
				if (file.filename == path) {
					// Load all data
					//MemoryStream ms = new MemoryStream(wp.LoadFile(file));
					//return new SizeStream(ms, (int)ms.Length);
					
					// Load only offset
					//MipTexture mtex = wp.LoadMipTexture(file);					
					//return new SizeStream(wp.GetTextureStream(file, mtex), mtex.TextureSize);
				}
			}
			return null;
		}
		
	}
}