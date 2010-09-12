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

namespace IronClient.VFS
{
	public class FileSystem
	{
		private static FileSystem instance;
		
		public static FileSystem GetInstance() {
			if (instance != null) 
				return instance;
			else return instance = new FileSystem();
		}
		
		List<Archive> archives = new List<Archive>();
		private FileSystem () {}
		
		public void AddArchive(Archive archive) {
			archives.Add(archive);
		}
		
		public void AddZipArchive(string path) {
			archives.Add(new ZipArchive(path));
		}
		
		public SizeStream Get(string path) {
			SizeStream ret;
			foreach (Archive archive in archives) {
				if ((ret = archive.Get(path)) != null)
					return ret;
			}
			return null;
		}
	}
}

