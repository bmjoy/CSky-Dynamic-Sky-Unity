//////////////////////////////////////////////
/// CSky.
/// Name: Sky Sphere Resources.
/// Description: All Resources for sky sphere.
/// 
//////////////////////////////////////////////

using System.IO;
using UnityEngine;

namespace AC.CSky
{

    [CreateAssetMenu(fileName = "Sky Sphere Resources", menuName = "AC/CSky/Sky Sphere Resources", order = 15)]
    public class CSky_SkySphereResources : ScriptableObject
    {

        #region |Meshes|

        [Header("Sphere Meshes")]
        public Mesh sphereLOD0;
        public Mesh sphereLOD1;
        public Mesh sphereLOD2;
        public Mesh sphereLOD3;

        [Header("Quad Mesh")]
        public Mesh quadMesh;

        [Header("Stars Meshes")]
        public Mesh StarsLOD0;
        public Mesh StarsLOD1;
        public Mesh StarsLOD2;

        #endregion

        #region |Shaders|

        [Header("Celestials Shaders")]
        public Shader backgroundShader;
        public Shader starsFieldShader;
        public Shader sunShader;
        public Shader moonShader;

        [Header("Atmosphere Shaders")]
        public Shader defautlAtmosphereShader;
        public Shader PreethamAtmosphereShader;
        public Shader defautlSkyboxShader;
        public Shader PreethamSkyboxShader;

        #endregion

        #region |Materials|

        [Header("Celestials Materials")]
        public Material backgroundMaterial;
        public Material starsFieldMaterial;
        public Material sunMaterial;
        public Material moonMaterial;

        [Header("Atmosphere Materials")]
        public Material atmosphereMaterial;
        public Material skyboxMaterial;

        #endregion

    }

}