using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;

namespace Blackjack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private List<string> deck;
        private List<List<string>> deck;
        private Queue<string> history;
        private Random rnd;
        private List<string> card;
        private int playerPoints;
        private int computerPoints;
        private DispatcherTimer timer;
        private DispatcherTimer timeKeeper;
        private bool playerGetsCard = false;
        /// <summary>
        /// count telt de aantal seconden
        /// </summary>
        private int count;
        private int kapitaal;
        private int bet;
        /// <summary>
        /// is om te zien als het mogelijk om doubledown te doen
        /// </summary>
        private bool doubleDown;
        /// <summary>
        /// houd de aantal ronded bij
        /// </summary>
        private int round;
        HistoryObject status;
        public MainWindow()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// dit zal het helebord leegmaken excl. te achterliggende waardes
        /// </summary>
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
        /// <summary>
        /// dit reset het bord naar begin positie en verwijdert achterliggende waardes
        /// </summary>
        private void ClearBoard()
        {
            if (computerCardImageNew.Source != null)
            {
                EmptyBoard();
            }
            dealButton.IsEnabled = false;
            winConditionLabel.Visibility = Visibility.Collapsed;
            if (!(playerTextbox == null))
            {
                playerTextbox.Clear();
                computerTextbox.Clear();
            }
            playerPoints = 0;
            playerLabel.Content = 0;
            computerPoints = 0;
            commputerLabel.Content = 0;
            historyLabel.Content = "";

        }
        /// <summary>
        /// het instellen van de kapitaal slider
        /// </summary>
        private void Slider()
        {
            betSlider.TickFrequency = 1;
            betSlider.Maximum = kapitaal;
            betSlider.Minimum = CheckMinimunSlider();
            betSlider.Value = bet;
        }
        /// <summary>
        /// set het minimum van de slider valeu
        /// </summary>
        /// <returns>10% van kapitaal</returns>
        private int CheckMinimunSlider()
        {
            double betValeu = kapitaal / 10;
            bet = Convert.ToInt32(Math.Ceiling(betValeu));
            betLabel.Content = bet;
            return bet;
        }
        /// <summary>
        /// dit gaat na of er wel genoeg kaarten zijn, anders zal hij alle kaarten er terug instoppen
        /// </summary>
        private void CheckDeckCards()
        {
            
            if (deck.Count == 0)
            {
                cardCountLabel.Text = "shuffeling";
                BuildDeck();
            }
            cardCountLabel.Text = Convert.ToString(deck.Count);
        }

        /// <summary>
        /// dit start het spel, hier word alles geset voor de deal knop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            doubleDown = false;
            count = 0;
            round = 0;
            history.Clear();
            ClearBoard();
            BuildDeck();
            dealButton.IsEnabled = true;
            winConditionLabel.Visibility = Visibility.Visible;
            winConditionLabel.Content = "DEAL TO START";
            //add kapital and betting value
            // add slider
            kapitaal = 100;
            SetKapitalValeu();
            betSlider.Visibility = Visibility.Visible;
            Slider();
            CheckDeckCards();
            
            
        }
        /// <summary>
        /// heeft exact 1 kaart aan de speler, bustcheck is om te kijken als men niet over 21 is gegaan
        /// player statistics checked gaat alles na.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hitButton_Click(object sender, RoutedEventArgs e)
        {
            GetCard();
            PlayerStatistics();
            bustCheck(playerPoints);
        }
        /// <summary>
        /// start de spelbuert van de bank/computer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void standButton_Click(object sender, RoutedEventArgs e)
        {
            computerPlays();
            NewGameButton.IsEnabled = true;
            NewGameButton.Visibility = Visibility.Visible;


        }
        /// <summary>
        /// de deal knop start te timer en dealt de kaarten uit, het bord word ook
        /// gelcleared van de vorige game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dealButton_Click(object sender, RoutedEventArgs e)
        {
            round++;
            doubleDown = false;
            betSlider.Visibility = Visibility.Collapsed;
            kapitaal = kapitaal - bet;
            SetKapitalValeu();
            ClearBoard();
            timer.Start();
            //for (int p = 0; p < 2; p++)
            //{
            //    for (int i = 0; i < 2; i++)
            //    {
                    
            //        GetCard();

            //        switch (p)
            //        {
            //            case 0:
            //                PlayerStatistics();
            //                break;
            //            case 1:
            //                ComputerStatistics();
            //                break;
            //        }
            //    }
            //}
            hitButton.IsEnabled = true;
            standButton.IsEnabled = true;
            hitButton.Visibility = Visibility.Visible;
            standButton.Visibility = Visibility.Visible;
            NewGameButton.IsEnabled = false;
            NewGameButton.Visibility = Visibility.Collapsed;
            CheckIfDoubleDownPossible(); 
        }
        /// <summary>
        /// zet de voorbije 10 rondes in een messagebox voor te tonen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void historyLabel_Click(object sender, RoutedEventArgs e)
        {
            string rounds = "";
            for (int i = history.Count - 1; i >= 0; i--)
            {
                rounds += history.ToArray().ToList()[i];
                rounds += Environment.NewLine;
            }
            MessageBox.Show(rounds);
        }
        /// <summary>
        /// een mix van hit en stand, geeft 1 kaart en eindigt de buert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleDownButton_Click(object sender, RoutedEventArgs e)
        {
            doubleDown = true;
            DoubleDownTheBet();
            GetCard();
            PlayerStatistics();
            bustCheck(playerPoints);
            computerPlays();
            NewGameButton.IsEnabled = true;
            NewGameButton.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// bij double down, verdubbelt hij de bet
        /// </summary>
        private void DoubleDownTheBet()
        {
            kapitaal = kapitaal - bet;
            SetKapitalValeu();
            bet = bet + bet;
            betLabel.Content = bet;
        }
        /// <summary>
        /// hier checken we als doubledown mogelijk is
        /// </summary>
        private void CheckIfDoubleDownPossible()
        {
            if (kapitaal > (bet + bet))
            {
                DoubleDownButton.Visibility = Visibility.Visible;
                DoubleDownButton.IsEnabled = true;
            } else
            {
                DoubleDownButton.IsEnabled = false;
            }
        }
        /// <summary>
        /// toont de kapitalvaleu
        /// </summary>
        private void SetKapitalValeu()
        {
            totalMoneyLabel.Content = kapitaal;
        }
        /// <summary>
        /// hier gaan we per seconde een kaart delen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            GetCard();
            if (playerGetsCard)
            {
                PlayerStatistics();
                playerGetsCard = false;
            } else
            {
                ComputerStatistics();
                playerGetsCard = true;
            }
            count++;
            StopTimer();
        }
        /// <summary>
        /// bij het uitdelen van 4 kaarten gaan we deze timer stoppen
        /// </summary>
        private void StopTimer()
        {
            if (count == 4)
            {
                timer.Stop();
                count = 0;
                //MessageBox.Show("is four");
            }
        }
        /// <summary>
        /// plaatst  het deck op het bord
        /// </summary>
        private void SetDeckBackground()
        {
            BitmapImage cardImage = new BitmapImage(new Uri(new Uri(Directory.GetCurrentDirectory(), UriKind.Absolute), new Uri($@"../../CardImg/CardCover/CardCover.png", UriKind.Relative)));
            deckImage.Source = cardImage;
        }
        /// <summary>
        /// dit haalt de juiste foto source uit de folders afhankelijk van de gespeelde kaart
        /// </summary>
        /// <returns>source van de foto</returns>
        private ImageSource GetCardImg()
        {
            // card.SetBinding();
            // GetCardName() return example Heart 7
            //string path = "C:\Users\corre\Desktop\DOTNETPRO\BlackJack_main\CardImg\Heart\7_of_hearts.png";
            //card = 
            //string folderPath = Environment.GetFolderPath(Environment.CurrentDirectory.(@"/CardImg/Heart/7_of_hearts.png"));
            string face = card[0];
            string number = card[1];
            BitmapImage cardImage = new BitmapImage();
            cardImage.BeginInit();
            cardImage.UriSource = new Uri(new Uri(Directory.GetCurrentDirectory(), UriKind.Absolute), new Uri($@"../../CardImg/{face}/{number}.png", UriKind.Relative));
            if (doubleDown)
            {
                cardImage.Rotation = Rotation.Rotate90;
            }
            cardImage.EndInit();
            return cardImage;
        }
        /// <summary>
        /// dit plaatst en schuift de fotos op het bord
        /// </summary>
        /// <param name="player"> is het de bank of speler waar we een kaart toevoegen</param>
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
        /// <summary>
        ///dit gaat de getrokken kaart in een textveld steken
        /// daarna de punten berekenen
        /// als laatse ssturen we de kaart info door met indidificatie dat het de speler is
        /// </summary>
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
        /// <summary>
        /// zelfe als playerstatistics
        /// maar nu sturen we door dat het de bank is
        /// </summary>
        private void ComputerStatistics()
        {
            computerTextbox.AppendText(GetCardName());
            computerTextbox.AppendText(Environment.NewLine);
            computerPoints = CalculationOfCards(computerPoints, card[1].ToString());
            commputerLabel.Content = computerPoints.ToString();
            //computerCardImage.Source = GetCardImg();
            SetCardImg(0);
        }
        /// <summary>
        /// dit haalt de benaming van de kaart
        /// </summary>
        /// <returns>returns face and valeu</returns>
        private string GetCardName()
        {
            string cardName = "";
            for (int x = 0; x < card.Count; x++)
            {
                cardName += $"{card[x]} ";
            }
            return cardName;
        }
        /// <summary>
        /// checked als de bank minder heeft dan 17
        /// daarna gaan we een kaart trekken en deze tonen op het bord met statistics
        /// voor de zekeheid checken we dat er niemand gebust is
        /// anders checken we wie gewonnen heeft
        /// </summary>
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
        /// <summary>
        /// hij kijken we als we niet boven de 21 zitten
        /// </summary>
        /// <param name="points">we vragen de putne op van wie we checken als hij erover is</param>
        private void bustCheck(int points)
        {
            if (points > 21)
            {
                CheckWhoWon();
            }
        }
        /// <summary>
        /// hier kijken we hoeveel punten de speler of bank verdient heeft
        /// </summary>
        /// <param name="points">de al behaalde punten</param>
        /// <param name="card">de getrokken kaart waarde</param>
        /// <returns>nieuwe totale punten</returns>
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
        /// <summary>
        /// hier kijken we wie gewonnen heeft
        /// we resetten ook het meesten en geven de optie om een nieuw game te starten
        /// daarna za de nieuwe kapitaal berekent zijn.
        /// we gaan ook deze ronde opslaan
        /// de nieuwe kapital instellen en slider terug naar links zetten
        /// </summary>
        private void CheckWhoWon()
        {
            doubleDown = false;
            hitButton.IsEnabled = false;
            standButton.IsEnabled = false;
            NewGameButton.IsEnabled = true;
            NewGameButton.Visibility = Visibility.Visible;
            winConditionLabel.Visibility = Visibility.Visible;
            hitButton.Visibility = Visibility.Collapsed;
            standButton.Visibility = Visibility.Collapsed;
            DoubleDownButton.IsEnabled = false;
            if (playerPoints == computerPoints)
            {
                winConditionLabel.Content = "PUSH!";
                kapitaal = kapitaal + bet;
                dealButton.IsEnabled = true;
                betSlider.Visibility = Visibility.Visible;
            }
            else if (playerPoints > computerPoints && playerPoints < 22 || computerPoints > 21)
            {
                winConditionLabel.Content = "WON!";
                bet = bet + bet;
                kapitaal = kapitaal + bet;
                dealButton.IsEnabled = true;
                betSlider.Visibility = Visibility.Visible;
            } else
            {
                
                if (CheckIfbroke())
                {
                    winConditionLabel.Content = "BROKE";
                } else
                {
                    dealButton.IsEnabled = true;
                    winConditionLabel.Content = "LOST";
                    betSlider.Visibility = Visibility.Visible;
                }
            }
            BuildHistoryList();
            CheckIfDoubleDownPossible();
            SetKapitalValeu();
            Slider();
        }
        /// <summary>
        /// hier ropen we de classe historyobject op, deze dient om de ronde op te slaan om daarna in een list te steken
        /// queue zorgt voor een FIFO
        /// </summary>
        private void BuildHistoryList()
        {
            if (history.Count == 10)
            {
                history.Dequeue();
            }
            status = new HistoryObject(round, bet, playerPoints, computerPoints);
            historyLabel.Content = $"{status.count}: {status.bet} - {status.playerPoints}/{status.computerPoints}";
            history.Enqueue($"{status.count}: {status.bet} - {status.playerPoints}/{status.computerPoints}");
        }
        /// <summary>
        /// hier gaan we kijken of we wel nog verder mogen spelen
        /// </summary>
        /// <returns>een ja of nee naarmate hoeveel we nog hebben</returns>
        private bool CheckIfbroke()
        {
            if (kapitaal == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// hier gaan we een random kaart uit het deck halen
        /// </summary>
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
        /// <summary>
        /// de getrokken kaart verweideren we uit het deck
        /// </summary>
        /// <param name="card">de getrokken kaart</param>
        private void RemoveCard(int card)
        {
            deck.RemoveAt(card);
            //TestBuiltCardDeck();
            CheckDeckCards();
        }
        /// <summary>
        /// dit gaat het deck van 52 kaarten maken
        /// </summary>
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
        /// <summary>
        /// dit was gebruikt om te testen of wel degelijk alle kaarten gegenereerd werden
        /// </summary>
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
        /// <summary>
        /// bij het inladen gaan we een paar dingen klaar zetten
        /// lists declareren en timer starten voor local time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //deck = new List<string>(52) { "Spade00", "Spade02", "Spade03", "Spade04", "Spade05", "Spade06", "Spade07", "Spade08", "Spade09", "Spade10",
            //"SpadeJJ", "SpadeQQ", "SpadeKK"};
            deck = new List<List<string>>(52) {};
            card = new List<string>(2);
            history = new Queue<string>(10);
            //maak methode aan met dubbele for lussen om kaarten in list te steken
            //deck.Add(new List<string>(2) {"Spade", "Ace"});
            //deck[0][0];
            //BuildDeck(); move to deal
            // random kaart
            rnd = new Random();
            Timer();
            hitButton.IsEnabled = false;
            standButton.IsEnabled = false;
            dealButton.IsEnabled = false;
            SetDeckBackground();
        }

        /// <summary>
        /// dit start de timer voor local time
        /// </summary>
        private void Timer()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(Timer_Tick);
            timeKeeper = new DispatcherTimer();
            timeKeeper.Interval = new TimeSpan(0, 0, 1);
            timeKeeper.Tick += new EventHandler(TimeKeep_Tick);
            timeKeeper.Start();
        }
        /// <summary>
        /// bepaalde wat hij per tick gaat doen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeKeep_Tick(object sender, EventArgs e)
        {
            SetTime();
        }
        /// <summary>
        /// elke seconde laat hij de tijd zien in de status balk
        /// </summary>
        private void SetTime()
        {
            timeLabel.Content = $"{DateTime.Now.ToLongTimeString()}";
        }
        /// <summary>
        /// als de slider aangepast word dan zullen de valeus ook veranderen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void betSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            bet = Convert.ToInt32(betSlider.Value);
            betLabel.Content = bet;
            CheckIfDoubleDownPossible();
        }

        
    }
}
