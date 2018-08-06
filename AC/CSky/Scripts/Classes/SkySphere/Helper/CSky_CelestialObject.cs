///////////////////////////////////////////
/// CSky.
/// Name: Celestial Object.
/// Description: Celestial object class.
/// 
///////////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AC.CSky
{
	
	[System.Serializable]
	public class CSky_CelestialObject
	{

        public GameObject   gameObject;
        public Transform    transform;
        public MeshFilter   meshFilter;
        public MeshRenderer meshRenderer;

		public bool CheckComponents
		{
			get
			{

				if(gameObject   == null) return false;
				if(transform    == null) return false;
				if(meshRenderer == null) return false;
				if(meshRenderer == null) return false;

				return true;
			}
		}

		public void GetComponents()
        {

            if(gameObject != null)
            {
                transform    = gameObject.transform;
                meshFilter   = gameObject.GetComponent<MeshFilter>();
                meshRenderer = gameObject.GetComponent<MeshRenderer>();
            }
            else
            {
                meshFilter   = null;
                meshRenderer = null;
                transform    = null;
            }
        }

 		public void InitTransform(Transform parent)
        {

            if(transform == null) return;

            transform.parent        = parent;
            transform.position      = Vector3.zero;
            transform.localPosition = Vector3.zero;
            transform.rotation      = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale    = Vector3.one;
        }

		
        public void Build(string skySphereName, string name)
        {

            if(gameObject == null)
            {
                var childObj = GameObject.Find("/" + skySphereName + "/" + name); // Check if exist gameobject with this name.

                if (childObj != null)
                    gameObject = childObj;
                else
                    gameObject = new GameObject(name);
            }

            if(transform == null)
                transform = gameObject.transform; // Get transform.

            if(meshFilter == null)
            {
                bool componentIsAssigned = (meshFilter = gameObject.GetComponent<MeshFilter>()); // Get mesh filter component.

                if (!componentIsAssigned)
                    meshFilter = gameObject.AddComponent<MeshFilter>(); // Add mesh filter component.
            }

            if(meshRenderer == null)
            {

                bool componentIsAssigned = (meshRenderer = gameObject.GetComponent<MeshRenderer>()); // Get mesh renderer component.

                if (!componentIsAssigned)
                    meshRenderer = gameObject.AddComponent<MeshRenderer>(); // Add mesh renderer component.
            }
        }

	}



    [System.Serializable]
    public class CSky_CelestialLight
    {


        public GameObject gameObject;
        public Transform  transform;
        public Light      light;

      
        public bool CheckComponents
        {
            get
            {

                if(gameObject == null) return false;
                if(light == null)      return false;
                if(transform == null)  return false;

                return true;
            }
        }


        public void GetComponents()
        {

            if (light != null)
                transform = light.transform;
            else
                transform = null;
        }

        public void InitTransform(Transform parent)
        {
            if (transform == null) return;

            transform.parent        = parent;
            transform.position      = Vector3.zero;
            transform.localPosition = Vector3.zero;
            transform.rotation      = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale    = Vector3.one;
            light.type              = LightType.Directional;
        }


        public void Build(string skySphereName, string parentName, string lightName)
        {

            if (gameObject == null)
            {

                var childObj = GameObject.Find("/" + skySphereName + "/" + parentName + "/" + lightName); // Check if exist gameobject with this name.

                if (childObj != null)
                    gameObject = childObj;
                else
                    gameObject = new GameObject(lightName);
            }

            if (transform == null)
               transform = gameObject.transform; // Get transform.

            // Get or add mesh filter.
            if (light == null)
            {
                bool componentIsAssigned = (light = gameObject.GetComponent<Light>()); // Get Light Component.

                if (!componentIsAssigned)
                    light = gameObject.AddComponent<Light>(); // Add light component.
            }
            else
            {
                light.type = LightType.Directional; // Set light type.
            }
        }

    }
}