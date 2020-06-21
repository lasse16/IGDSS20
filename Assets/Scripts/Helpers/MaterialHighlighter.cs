using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IGDSS20.Helpers
{

    public class MaterialHighlighter : MonoBehaviour, IHighlighter
    {
        private Dictionary<GameObject, Material> previousMaterials = new Dictionary<GameObject, Material>();
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private float DeselectTimeInSeconds;

        public void HighlightGameObject(GameObject gameObject)
        {
            if (gameObject is null)
                throw new ArgumentNullException();

            var previousMat = gameObject.GetComponent<Renderer>().material;
            previousMaterials[gameObject] = previousMat;

            gameObject.GetComponent<Renderer>().material = highlightMaterial;

            if (DeselectTimeInSeconds != 0)
            {
                var timeSpan = TimeSpan.FromSeconds(DeselectTimeInSeconds);
                Task.Delay(timeSpan).ConfigureAwait(false);
                RemoveHighlightFromObject(gameObject);
            }
        }

        public void RemoveHighlightFromObject(GameObject gameObject)
        {
            if (gameObject is null)
                throw new ArgumentNullException();
            if (!previousMaterials.ContainsKey(gameObject))
                throw new ArgumentException("Unknown game object");

            gameObject.GetComponent<Renderer>().material = previousMaterials[gameObject];

            previousMaterials.Remove(gameObject);
        }
    }
}
