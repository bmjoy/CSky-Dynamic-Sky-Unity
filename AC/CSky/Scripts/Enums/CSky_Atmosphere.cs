//////////////////////////////////////////////
/// CSky.
/// Name: Atmosphere Model.
/// Description: Atmospheric scattering model.
/// 
//////////////////////////////////////////////

using UnityEngine;

namespace AC.CSky
{

    public enum CSky_AtmosphereModel
    {
        Defautl,    // Based on GPU Gems2, Sean Oneil work and nishita papers.
        Preetham    // Based on Preetham and Holfman papers.
    }
}
        
