﻿using CommunityToolkit.Mvvm.ComponentModel;
using PalCalc.Model;
using PalCalc.SaveReader;
using PalCalc.Solver;
using PalCalc.UI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalCalc.UI.ViewModel
{
    public class BreedingResultViewModel : ObservableObject
    {
        // For XAML designer view
        public BreedingResultViewModel()
        {
            var db = PalDB.LoadEmbedded();
            var latest = SavesLocation.AllLocal.SelectMany(loc => loc.ValidSaveGames).MaxBy(game => game.LastModified);
            var saveGame = Storage.LoadSave(latest, db);

            var solver = new Solver.BreedingSolver(
                gameSettings: new GameSettings(),
                db: db,
                ownedPals: saveGame.OwnedPals,
                maxBreedingSteps: 3,
                maxWildPals: 0,
                maxIrrelevantTraits: 0,
                maxEffort: TimeSpan.FromHours(8)
            );

            var targetInstance = new PalSpecifier
            {
                Pal = "Galeclaw".ToPal(db),
                Traits = new List<Trait> {
                    "Swift".ToTrait(db),
                    "Runner".ToTrait(db),
                    "Nimble".ToTrait(db)
                },
            };

            DisplayedResult = solver.SolveFor(targetInstance).MaxBy(r => r.NumBredPalParticipants());
        }

        public BreedingResultViewModel(IPalReference displayedResult)
        {
            DisplayedResult = displayedResult;
        }

        public BreedingGraph Graph { get; private set; }

        public string Label => DisplayedResult?.ToString() ?? "Unknown";

        public bool HasValue => Graph != null;

        private IPalReference displayedResult = null;
        public IPalReference DisplayedResult
        {
            get => displayedResult;
            set
            {
                displayedResult = value;

                if (displayedResult == null) Graph = null;
                else Graph = BreedingGraph.FromPalReference(value);

                OnPropertyChanged(nameof(DisplayedResult));
                OnPropertyChanged(nameof(Graph));
                OnPropertyChanged(nameof(HasValue));
            }
        }
    }
}
