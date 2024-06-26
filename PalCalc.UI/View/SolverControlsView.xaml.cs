﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PalCalc.UI.View
{
    /// <summary>
    /// Interaction logic for SolverSettingsView.xaml
    /// </summary>
    public partial class SolverControlsView : StackPanel
    {
        public SolverControlsView()
        {
            InitializeComponent();
        }

        public event Action OnRun;

        private void Run_Click(object sender, RoutedEventArgs e) => OnRun?.Invoke();
    }
}
