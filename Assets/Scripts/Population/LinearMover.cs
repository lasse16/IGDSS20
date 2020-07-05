using Boo.Lang;
using IGDSS20;
using IGDSS20.Assets.Scripts.Navigation;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    internal class LinearMover : MonoBehaviour
    {
        public UnityEvent TargetReached;

        [SerializeField] private MoverSettings _settings;


        internal void Move(GameObject gameObject, Tile startPosition, PotentialMap potentialMap)
        {
            List<Waypoint> path = FindPath(startPosition, potentialMap);

            StartCoroutine(MovementCoroutine(gameObject, path));
        }

        private List<Waypoint> FindPath(Tile startPosition, PotentialMap potentialMap)
        {
            var currentWeight = int.MaxValue;
            var currentTile = startPosition;
            var path = new List<Waypoint>();

            while (currentWeight > 0)
            {
                var neighbours = currentTile.NeighbouringTiles;
                var lowestWeight = int.MaxValue;
                var lowestWeightNeighbour = default(Tile);

                foreach (var neighbour in neighbours)
                {
                    var neighbourWeight = potentialMap.GetWeight(neighbour);
                    if (neighbourWeight < lowestWeight)
                    {
                        lowestWeight = neighbourWeight;
                        lowestWeightNeighbour = neighbour;
                    }
                }

                var duration = currentWeight - lowestWeight;
                currentWeight = lowestWeight;
                currentTile = lowestWeightNeighbour;
                path.Add(new Waypoint(duration * _settings.WeightDurationRatio, currentTile.gameObject.transform.position));
            }

            return path;
        }

        private IEnumerator MovementCoroutine(GameObject gameObject, List<Waypoint> targetPositions)
        {
            foreach (var waypoint in targetPositions)
            {
                var currentPosition = gameObject.transform.position;
                var targetPosition = waypoint.Point;
                while (Vector3.Distance(currentPosition, targetPosition) > 0.01)
                {
                    currentPosition = Vector3.MoveTowards(currentPosition, targetPosition, Time.deltaTime);
                    gameObject.transform.position = currentPosition;
                    gameObject.transform.rotation = Quaternion.LookRotation(currentPosition - targetPosition);
                    yield return null;
                }
            }

            TargetReached.Invoke();
            yield break;
        }

        private readonly struct Waypoint
        {
            public readonly Vector3 Point;
            public readonly float Duration;

            public Waypoint(float duration, Vector3 point)
            {
                Duration = duration;
                Point = point;
            }
        }
    }
}