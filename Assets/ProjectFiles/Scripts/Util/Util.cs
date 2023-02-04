using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {
    public static void Shuffle<T>(this IList<T> list) {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
