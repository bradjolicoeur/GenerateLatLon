﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GenerateLatLon.Helpers
{
    public static class IEnumerableExtensions
    {

        public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, Random rand, Func<T, double> weightSelector)
        {
            double totalWeight = sequence.Sum(weightSelector);

            // The weight we are after...
            double itemWeightIndex = rand.NextDouble() * totalWeight;
            double currentWeightIndex = 0;

            foreach (var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) })
            {
                currentWeightIndex += item.Weight;

                // If we've hit or passed the weight we are after for this item then it's the one we want....
                if (currentWeightIndex > itemWeightIndex)
                    return item.Value;

            }

            return default(T);

        }

    }
}
