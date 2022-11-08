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

namespace Blackjack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private List<string> deck;
        private List<List<string>> deck;
        private Random rnd;
        private List<string> card;
        private int playerPoints;
        private int computerPoints;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void dealButton_Click(object sender, RoutedEventArgs e)
        {
            dealButton.IsEnabled = false;
            winConditionLabel.Visibility = Visibility.Hidden;
            if (!(playerTextbox == null))
            {
                playerTextbox.Clear();
                computerTextbox.Clear();
            }
            playerPoints = 0;
            computerPoints = 0;
            for (int p = 0; p < 2; p++)
            {
                for (int i = 0; i < 2; i++)
                {
                    string cardName = "";
                    GetCard();
                    for (int x = 0; x < card.Count; x++)
                    {
                        cardName += $"{card[x]} ";
                    }
                    switch (p)
                    {
                        case 0:
                            playerTextbox.AppendText(cardName);
                            playerTextbox.AppendText(Environment.NewLine);
                            playerPoints = CalculationOfCards(playerPoints, card[1].ToString());
                            playerLabel.Content = playerPoints.ToString();
                            break;
                        case 1:
                            computerTextbox.AppendText(cardName);
                            computerTextbox.AppendText(Environment.NewLine);
                            computerPoints = CalculationOfCards(computerPoints, card[1].ToString());
                            commputerLabel.Content = computerPoints.ToString();
                            break;
                    } 
                }
            }
            hitButton.IsEnabled = true;
            standButton.IsEnabled = true;
        }

        private void hitButton_Click(object sender, RoutedEventArgs e)
        {
            string cardName = "";
            GetCard();
            for (int x = 0; x < card.Count; x++)
            {
                cardName += $"{card[x]} ";
            }
            playerTextbox.AppendText(cardName);
            playerTextbox.AppendText(Environment.NewLine);
            playerPoints = CalculationOfCards(playerPoints, card[1].ToString());
            playerLabel.Content = playerPoints.ToString();
            bustCheck(playerPoints);
        }

        private void standButton_Click(object sender, RoutedEventArgs e)
        {
            computerPlays();
        }

        private void computerPlays()
        {
            while (computerPoints < 17)
            {
                string cardName = "";
                GetCard();
                for (int x = 0; x < card.Count; x++)
                {
                    cardName += $"{card[x]} ";
                }
                computerTextbox.AppendText(cardName);
                computerTextbox.AppendText(Environment.NewLine);
                computerPoints = CalculationOfCards(computerPoints, card[1].ToString());
                commputerLabel.Content = computerPoints.ToString();
                bustCheck(computerPoints);
            }
            CheckWhoWon();
        }

        private void bustCheck(int points)
        {
            if (points > 21)
            {
                CheckWhoWon();
            }
        }

        private int CalculationOfCards(int points, string card)
        {
            int value = 0;
            switch (card)
            {
                case "Ace":
                    //to be changed in future
                    value = 1;
                    return points + value;
                    break;
                case "Jack":
                case "Queen":
                case "King":
                    value = 10;
                    return points + value;
                    break ;
                default:
                    value = Int32.Parse(card);
                    return points + value;
                    break;
            }
        }

        private void CheckWhoWon()
        {
            dealButton.IsEnabled = true;
            hitButton.IsEnabled = false;
            standButton.IsEnabled=false;
            winConditionLabel.Visibility = Visibility.Visible;
            if (playerPoints == computerPoints)
            {
                winConditionLabel.Content = "TIE!";
            }
            else if (playerPoints > computerPoints && playerPoints < 22 || computerPoints > 21)
            {
                winConditionLabel.Content = "WON!";
            } else
            {
                winConditionLabel.Content = "LOST";
            }
        }

        private void GetCard()
        {
            if (!(card == null))
            {
                card.Clear();
            }
            
            int rint = rnd.Next(deck.Count);
            card.Add(deck[rint][0]);
            card.Add(deck[rint][1]);
            //MessageBox.Show($"{card[0]},{card[1]}");
            RemoveCard(rint);
        }

        private void RemoveCard(int card)
        {
            deck.RemoveAt(card);
            //TestBuiltCardDeck();
        }

        private void BuildDeck()
        {
            string cardName = "";
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        cardName = "Spade";
                        break;
                    case 1:
                        cardName = "Heart";
                        break;
                    case 2:
                        cardName = "Clover";
                        break;
                    case 3:
                        cardName = "Diamond";
                        break;
                }
                deck.Add(new List<string>(2) { $"{cardName}", "Ace" });
                for (int value = 0; value < 9; value++)
                {
                    deck.Add(new List<string>(2) { $"{cardName}", $"{value + 2}" });
                }
                deck.Add(new List<string>(2) { $"{cardName}", "Jack" });
                deck.Add(new List<string>(2) { $"{cardName}", "Queen" });
                deck.Add(new List<string>(2) { $"{cardName}", "King" });
            }
            //TestBuiltCardDeck();
        }

        //Testing methode
        private void TestBuiltCardDeck()
        {
            string listOfCards = "";
            for (int i = 0; i < deck.Count; i++)
            {
                for (int x = 0; x < 2; x++)
                {
                    listOfCards += deck[i][x].ToString() + " ";
                }
            }
            MessageBox.Show(listOfCards);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //deck = new List<string>(52) { "Spade00", "Spade02", "Spade03", "Spade04", "Spade05", "Spade06", "Spade07", "Spade08", "Spade09", "Spade10",
            //"SpadeJJ", "SpadeQQ", "SpadeKK"};
            deck = new List<List<string>>(52) {};
            card = new List<string>(2);
            //maak methode aan met dubbele for lussen om kaarten in list te steken
            //deck.Add(new List<string>(2) {"Spade", "Ace"});
            //deck[0][0];
            BuildDeck();
            // random kaart
            rnd = new Random();

            hitButton.IsEnabled = false;
            standButton.IsEnabled = false;
        }
    }
}
