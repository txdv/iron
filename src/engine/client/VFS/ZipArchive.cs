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
using System.Collections;
using System.Collections.Generic;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;

namespace IronClient.VFS
{
	public class ZipArchive : Archive
	{
		ZipFile zf;
		
		public ZipArchive (string filename)
		{
			zf = new ZipFile(filename);
		}
		
		public List<String> GetEntries() {
			List<String> ret = new List<String>();
			
			foreach (ZipEntry ze in zf) {
				ret.Add(ze.Name);
			}
			
			return ret;
		}
		
		public SizeStream Get(string path) {
			ZipEntry ze = zf.GetEntry(path);
			return new SizeStream(zf.GetInputStream(ze), (int)ze.Size);
		}
	}
}

