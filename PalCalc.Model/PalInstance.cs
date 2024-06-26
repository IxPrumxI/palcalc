﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalCalc.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PalGender
    {
        // note: pal world is not P.C., so it only has two genders
        MALE = 0b001,
        FEMALE = 0b010,
        // (but this program is P.C., and has four genders)
        WILDCARD = 0b011,
        OPPOSITE_WILDCARD = 0b100, // contextual - pals of this gender should be paired with pals of WILDCARD gender
    }

    public class PalInstance
    {
        public Pal Pal { get; set; }
        public PalLocation Location { get; set; }
        public PalGender Gender { get; set; }
        public List<Trait> Traits { get; set; }

        public override string ToString() => $"{Gender} {Pal} at {Location} with traits ({string.Join(", ", Traits)})";

    }
}
