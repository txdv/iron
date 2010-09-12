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


namespace IronClient.VFS
{
	public class ArchiveEntry
	{
		public ArchiveEntry(String name, Stream istream) {
			this.name = name;
			this.inputStream = istream;
		}
		
		private String name;
		private Stream inputStream;
		
		public String Name { get { return name; } }
		
		public Stream InputStream {
			get {
				return inputStream;
			}
		}
	}
}

