﻿using PalCalc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalCalc.Solver
{
    // a working set of pal instances to be used as potential parents
    internal class WorkingSet
    {
        private HashSet<IPalReference> content;

        public IReadOnlySet<IPalReference> Content => content;

        public WorkingSet()
        {
            content = new HashSet<IPalReference>();
        }

        public WorkingSet(IEnumerable<IPalReference> initialContent)
        {
            content = new HashSet<IPalReference>();
            AddFrom(initialContent);
        }

        // returns the number of entries added/updated
        public int AddFrom(IEnumerable<IPalReference> newRefs)
        {
            // since we know the breeding effort of each potential instance, we can ignore new instances
            // with higher effort than existing known instances
            //
            // (this is the main optimization that lets the solver complete in less than a week)
            var numChanged = 0;
            foreach (var newInst in PruneCollection(newRefs))
            {
                var existingInstances = content.Where(pi =>
                    pi.Pal == newInst.Pal &&
                    pi.Gender == newInst.Gender &&
                    pi.Traits.EqualsTraits(newInst.Traits)
                ).ToList();

                var existingInst = existingInstances.SingleOrDefault();

                if (existingInst != null)
                {
                    if (newInst.BreedingEffort < existingInst.BreedingEffort)
                    {
                        content.Remove(existingInst);
                        content.Add(newInst);
                        numChanged++;
                    }
                }
                else
                {
                    content.Add(newInst);
                    numChanged++;
                }
            }

            return numChanged;
        }

        public int AddFrom(WorkingSet ws) => AddFrom(ws.Content);

        // gives a new, reduced collection which only includes the "most optimal" / lowest-effort
        // reference for each instance spec (gender, traits, etc.)
        private static IEnumerable<IPalReference> PruneCollection(IEnumerable<IPalReference> refs) =>
            refs
                .GroupBy(pref => (
                    pref.Pal,
                    pref.Gender,
                    string.Join(" ",
                        pref
                            .Traits
                            .Select(t => t.ToString())
                            .OrderBy(t => t)
                    )
                ))
                .Select(g => g.MinBy(pref => pref.BreedingEffort));
    }
}
