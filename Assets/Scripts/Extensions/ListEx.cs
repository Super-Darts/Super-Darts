using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Extensions
{
    public static class ListEx
    {
        public static void Flush<T>(this List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i);
                }
            }
        }

        public static void AddOnce<T>(this List<T> list, T item)
        {
            while (list.Contains(item))
            {
                list.Remove(item);
            }

            list.Add(item);
        }

        public static T Random<T>(this List<T> list)
        {
            // return list.Count <= 0 ? default : list[UnityEngine.Random.Range(0, list.Count)];

            List<T> copy = list.ToArray().ToList();

            while(copy.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, copy.Count);
				T theChosenOne = copy[randomIndex];
                if(theChosenOne != null)
                    return theChosenOne;
                copy.RemoveAt(randomIndex);
            }

            return default;
        }
    }
}