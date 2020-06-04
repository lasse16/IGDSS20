using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectHighlighter : MonoBehaviour, IHighlighter
    {
        [SerializeField] private GameObject Marker;
        [SerializeField] private float DeselectTimeInSeconds;
        private Dictionary<GameObject, GameObject> previousHighlights = new Dictionary<GameObject, GameObject>();


        public void HighlightGameObject(GameObject gameObject)
        {
            if (gameObject is null)
                throw new ArgumentNullException();

            var marker = Instantiate(Marker);
            marker.transform.position = gameObject.transform.position;

            previousHighlights[gameObject] = marker;

            if (DeselectTimeInSeconds != 0)
            {
                var timeSpan = TimeSpan.FromSeconds(DeselectTimeInSeconds);
                Task.Delay(timeSpan);
                RemoveHighlightFromObject(gameObject);
            }
        }

        public void RemoveHighlightFromObject(GameObject gameObject)
        {
            if (gameObject is null)
                throw new ArgumentNullException();

            if (!previousHighlights.ContainsKey(gameObject))
                throw new ArgumentException();

            var marker = previousHighlights[gameObject];
            Destroy(marker);

            previousHighlights.Remove(gameObject);
        }
    }
}
