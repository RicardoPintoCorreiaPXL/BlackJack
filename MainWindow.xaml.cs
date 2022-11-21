using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private void EmptyBoard()
        {
            playerCardImage.Source = null;
            playerCardImageTwo.Source = null;
            playerCardImageThree.Source = null;
            playerCardImageNew.Source = null;
            playerCardImageFour.Source = null;
    
            computerCardImage.Source = null;
            computerCardImageTwo.Source = null;
            computerCardImageThree.Source = null;
            computerCardImageNew.Source = null;
            computerCardImageFour.Source = null;
        }

        private void dealButton_Click(object sender, RoutedEventArgs e)
        {
            BuildDeck();
            
            if (computerCardImageNew.Source != null)
            {
                EmptyBoard();
            }
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
                    
                    GetCard();
                    
                    switch (p)
                    {
                        case 0:
                            PlayerStatistics();
                            break;
                        case 1:
                            ComputerStatistics();
                            break;
                    } 
                }
            }
            hitButton.IsEnabled = true;
            standButton.IsEnabled = true;
        }

        private ImageSource GetCardImg()
        {
            // card.SetBinding();
            // GetCardName() return example Heart 7
            //string path = "C:\Users\corre\Desktop\DOTNETPRO\BlackJack_main\CardImg\Heart\7_of_hearts.png";
            //card = 
            //string folderPath = Environment.GetFolderPath(Environment.CurrentDirectory.(@"/CardImg/Heart/7_of_hearts.png"));
            string face = card[0];
            string number = card[1];
            BitmapImage cardImage = new BitmapImage(new Uri(new Uri(Directory.GetCurrentDirectory(), UriKind.Absolute), new Uri($@"../../CardImg/{face}/{number}.png", UriKind.Relative)));
            return cardImage;
        }

        private void SetCardImg(int player)
        {
            //ADD A FITH PICTURE
            switch (player)
            {
                case 0:
                    if (computerCardImageNew.Source == null)
                    {
                        computerCardImageNew.Source = GetCardImg();
                    }
                    else if (computerCardImage.Source == null)
                    {
                        computerCardImage.Source = computerCardImageNew.Source;
                        computerCardImageNew.Source = GetCardImg();
                    }
                    else if (computerCardImageTwo.Source == null)
                    {
                        computerCardImageTwo.Source = computerCardImageNew.Source;
                        computerCardImageNew.Source = GetCardImg();
                    }
                    else if (computerCardImageThree.Source == null)
                    {
                        computerCardImageThree.Source = computerCardImageNew.Source;
                        computerCardImageNew.Source = GetCardImg();
                    }
                    else if (computerCardImageFour.Source == null)
                    {
                        computerCardImageFour.Source = computerCardImageNew.Source;
                        computerCardImageNew.Source = GetCardImg();
                    }
                    else
                    {
                        MessageBox.Show("no more card space");
                    }
                    break;
                case 1:
                    if (playerCardImageNew.Source == null)
                    {
                        playerCardImageNew.Source = GetCardImg();
                    }
                    else if (playerCardImage.Source == null)
                    {
                        playerCardImage.Source = playerCardImageNew.Source;
                        playerCardImageNew.Source = GetCardImg();
                    }
                    else if (playerCardImageTwo.Source == null)
                    {
                        playerCardImageTwo.Source = playerCardImageNew.Source;
                        playerCardImageNew.Source = GetCardImg();
                    }
                    else if (playerCardImageThree.Source == null)
                    {
                        playerCardImageThree.Source = playerCardImageNew.Source;
                        playerCardImageNew.Source = GetCardImg();
                    }
                    else if (playerCardImageFour.Source == null)
                    {
                        playerCardImageFour.Source = playerCardImageNew.Source;
                        playerCardImageNew.Source = GetCardImg();
                    }
                    else
                    {
                        MessageBox.Show("no more card space");
                    }
                    break;
            }
        }

        private void PlayerStatistics()
        {
            playerTextbox.AppendText(GetCardName());
            playerTextbox.AppendText(Environment.NewLine);
            playerPoints = CalculationOfCards(playerPoints, card[1].ToString());
            playerLabel.Content = playerPoints.ToString();
            // testing multiple images
            //playerCardImage.Source = GetCardImg();
            //playerCardImageTwo.Source = GetCardImg();
            //playerCardImageThree.Source = GetCardImg();
            SetCardImg(1);
        } 

        private void ComputerStatistics()
        {
            computerTextbox.AppendText(GetCardName());
            computerTextbox.AppendText(Environment.NewLine);
            computerPoints = CalculationOfCards(computerPoints, card[1].ToString());
            commputerLabel.Content = computerPoints.ToString();
            //computerCardImage.Source = GetCardImg();
            SetCardImg(0);
        }

        private void hitButton_Click(object sender, RoutedEventArgs e)
        {
            GetCard();
            PlayerStatistics();
            bustCheck(playerPoints);
        }

        private void standButton_Click(object sender, RoutedEventArgs e)
        {
            computerPlays();
        }

        private string GetCardName()
        {
            string cardName = "";
            for (int x = 0; x < card.Count; x++)
            {
                cardName += $"{card[x]} ";
            }
            return cardName;
        }

        private void computerPlays()
        {
            while (computerPoints < 17)
            {               
                GetCard();
                ComputerStatistics();
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
                    //changed to auto choose
                    if ((points + 11) > 21)
                    {
                        value = 1;
                    } else
                    {
                        value = 11;
                    }                    
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
            if (!(deck == null))
            {
                deck.Clear();
            }
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
            //BuildDeck(); move to deal
            // random kaart
            rnd = new Random();

            hitButton.IsEnabled = false;
            standButton.IsEnabled = false;
        }
    }
}
