using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class ShuffleCards : MonoBehaviour
    {
        private List<Transform> objectsToShuffle;

        private void Awake()
        {
            objectsToShuffle = new();
            foreach (Transform item in GetComponentInChildren<Transform>())
            {
                objectsToShuffle.Add(item);
            }
        }

        public void ShuffleCardsList()
        {
            Shuffle(objectsToShuffle);
        }

        private void Shuffle(List<Transform> list)
        {
            System.Random rng = new();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (list[j].position, list[i].position) = (list[i].position, list[j].position);
            }
        }
    }
}
