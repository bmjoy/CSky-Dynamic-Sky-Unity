///////////////////////////////////////////////
/// CSky.
/// Name: Helper.
/// Description: Helper methods for sky sphere.
/// 
///////////////////////////////////////////////

using UnityEngine;

namespace AC.CSky
{

    public static class CSky_Helper 
    {


        #region Meshes

        /// <summary>
        /// Return quad mesh (Unity values + 0.5f).
        /// <param name="name"></param>
        /// <returns></returns>
        public static Mesh QuadMesh(float size = 1.0f, string name = "")
        {

            Vector3[] verts = new Vector3[]
            {
                new Vector3(-1, -1, 0) * size,
                new Vector3( 1,  1, 0) * size,
                new Vector3( 1, -1, 0) * size,
                new Vector3(-1,  1, 0) * size,
            };

            Vector2[] uv = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 1)
            };

            int[] triangles = new int[] { 0, 1, 2, 1, 0, 3 };
            Mesh mesh = new Mesh();
            mesh.name = name;
            mesh.vertices = verts;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }


        /*
		public static Mesh Quad(float size = 1.0f, string name = "")
		{

			Vector3[] verts = new Vector3[]
			{

				new Vector3(-1, 1, 0) * size,
				new Vector3( 1, 1, 0) * size,
				new Vector3( 1,-1, 0) * size,
				new Vector3(-1,-1, 0) * size,
			};

			Vector2[] uv = new Vector2[]
			{
				new Vector2(-1, 1),
				new Vector2(0, 1),
				new Vector2(0, 0),
				new Vector2(-1, 0)
			};

			int[] triangles = new int[]{ 0, 1, 2, 2, 3, 0 };
			Mesh mesh       = new Mesh ();
			mesh.name       = name;
			mesh.vertices   = verts;
			mesh.uv         = uv;
			mesh.triangles  = triangles;
			mesh.RecalculateNormals ();
			return mesh;
		}*/

        #endregion

    }
}
