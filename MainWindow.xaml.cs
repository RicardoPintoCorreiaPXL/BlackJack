using System;
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

namespace Blackjack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private List<string> deck;
        private List<List<string>> deck;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void dealButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void hitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void standButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CalculationOfCards(int points, int card)
        {

        }

        private void CheckWhoWon(int playerPoint, int computerPoints)
        {

        }

        private void GetCard()
        {

        }

        private void RemoveCard(int card)
        {

        }

        private void BuildDeck()
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //deck = new List<string>(52) { "Spade00", "Spade02", "Spade03", "Spade04", "Spade05", "Spade06", "Spade07", "Spade08", "Spade09", "Spade10",
            //"SpadeJJ", "SpadeQQ", "SpadeKK"};
            deck = new List<List<string>>(52) {};
            //maak methode aan met dubbele for lussen om kaarten in list te steken
            //deck.Add(new List<string>(2) {"Spade", "Ace"});
            //deck[0][0];
        }
    }
}
