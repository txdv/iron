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

namespace Iron.Client.Renderer
{
	public class Material
	{
		public const int COLOR = 1;
		public const int TEXTURE0 = 2;
		public const int TEXTURE1 = 4;
		
		public int Type { get; set; } 
		
		private int texture0 = 0;
		public int Texture0 { get { return texture0; } }
		
		private int texture1 = 0;
		public int Texture1 { get { return texture1; } }
		
		public int Color { get; set; }
		
		public Material(int type)
		{
			Type = type;
		}
		
		public Material(int type, int texture0)
		{
			Type = type;
			this.texture0 = texture0;
		}
	}
}

