﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalCalc.Model
{
    public class GameConfig
    {
        /*
         * Outstanding questions:
         * 
         * - What's the formula for how long breeding will take?
         * - What's the probability of wild pals having a specific gender?
         * - What's the probability of wild pals having exactly N traits?
         * - Do child pals ALWAYS inherit at least one of the parent's traits?
         */

        public static readonly int MaxTotalTraits = 4;

        // supposedly breeding time is a constant 5 minutes
        public static readonly TimeSpan BreedingTime = TimeSpan.FromMinutes(5);

        // actual breeding time can be extended if the parents are idle and not actually at
        // the breeding farm. assume parents are never idle except for sleep/night time, and
        // assume equal time for day+night, meaning the parents are actively breeding for
        // half the time of each day and the number of total eggs produced is cut in half
        //
        // (i.e., average effective breeding time is doubled)
        public static readonly TimeSpan AvgBreedingTime = BreedingTime * 2;

        public static readonly bool MultipleBreedingFarms = true;

        // roughly estimate time to catch a given pal based on paldex num. and whether it's a pal variant rather than base pal
        public static TimeSpan TimeToCatch(Pal pal)
        {
            var minTime = TimeSpan.FromMinutes(3);

            // TODO - tweak
            return minTime + pal.Id.PalDexNo * TimeSpan.FromSeconds(2);
        }

        // https://www.reddit.com/r/Palworld/comments/1af9in7/passive_skill_inheritance_mechanics_in_breeding/
        // supposedly the child will always inherit at least 1 trait directly from a parent?

        // probability of getting N traits from parent pool
        public static Dictionary<int, float> TraitProbabilityDirect = new Dictionary<int, float>()
        {
            { 4, 0.10f },
            { 3, 0.20f },
            { 2, 0.30f },
            { 1, 0.40f },
        };

        // probability of getting N traits from parent pool without any random passives
        public static Dictionary<int, float> TraitProbabilityNoRandom = new Dictionary<int, float>()
        {
            { 4, 0.10f },
            { 3, 0.08f },
            { 2, 0.12f },
            { 1, 0.16f },
        };

        public static Dictionary<int, float> TraitProbabilityAtLeastN = new Dictionary<int, float>()
        {
            { 4, 0.10f },
            { 3, 0.30f },
            { 2, 0.60f },
            { 1, 1.00f },
        };

        public static Dictionary<int, float> TraitProbabilityNoRandomAtLeastN = new Dictionary<int, float>()
        {
            { 4, 0.10f },
            { 3, 0.12f },
            { 2, 0.24f },
            { 1, 0.40f },
        };

        // probability of getting N additional random traits added
        public static Dictionary<int, float> TraitRandomAddedProbability = new Dictionary<int, float>()
        {
            { 4, 0.0f },
            { 3, 0.10f },
            { 2, 0.20f },
            { 1, 0.30f },
            { 0, 0.40f },
        };

        // probability of a wild pal having, at most, N random traits
        // (assume equal probability of gaining anywhere from 0 through 4 random traits)
        // (20% chance of exactly N traits)
        public static Dictionary<int, float> TraitWildAtMostN = new Dictionary<int, float>()
        {
            { 0, 0.2f },
            { 1, 0.4f },
            { 2, 0.6f },
            { 3, 0.8f },
            { 4, 1.0f },
        };
    }
}
