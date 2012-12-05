using System;
using System.Collections.Generic;
using System.Text;
using GLImp;

namespace Mila.Data
{
    /// <summary>
    /// Represents a set of faces that share the same material.
    /// </summary>
    public class FaceSet
    {
		private int matId = 0;
		public int materialId {
			get {
				if(matId <= 0) {
					foreach(Texture t in Texture.AllTextures) {
						if(t.Name == material) {
							matId = t.ID;
							return matId;
						}
					}
					matId = Texture.Error;
				}

				return matId;
			}
			set {
				matId = value;
			}
		}
        public string material = "";
        public Face[] faces = null;
    }
}
