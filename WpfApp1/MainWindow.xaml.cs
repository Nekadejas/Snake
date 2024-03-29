﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        GameEngine myGame;
        public MainWindow()
        {
            InitializeComponent();
            GameBoard.Focus();
            

            myGame = new GameEngine(GameBoard);
            myGame.GameLoop();
        }

        private void GameBoard_KeyDown(object sender, KeyEventArgs e)
        {
            myGame.UpdateMovement(e.Key);
        }
        
    }
}
