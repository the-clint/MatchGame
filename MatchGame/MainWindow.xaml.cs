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

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;
        int totalMatchesToBeFound;
        List<string> animalEmoji = new List<string>()
            {
                "🐙","🐙",
                "🐟","🐟",
                "🦈","🦈",
                "🐻","🐻",
                "🦦","🦦",
                "🐒","🐒",
                "🐧","🐧",
                "🦎","🦎",
            };
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick +=Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if(matchesFound == totalMatchesToBeFound)
            {
                timer.Stop();
                timeTextBlock.Text += " - Play again?";
            }
        }

        private void SetUpGame()
        {
            Random random = new Random();
            totalMatchesToBeFound = animalEmoji.Count() / 2;

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if(textBlock.Name != "timeTextBlock")
                {
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastEmojiClicked;
        bool findingMatch = false;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock emoji = sender as TextBlock;
            if (findingMatch == false)
            {
                emoji.Visibility = Visibility.Hidden;
                lastEmojiClicked = emoji;
                findingMatch = true;
            } else if (emoji.Text == lastEmojiClicked.Text)
            {
                emoji.Visibility = Visibility.Hidden;
                matchesFound++;
                findingMatch = false;
            } else
            {
                lastEmojiClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(matchesFound == totalMatchesToBeFound)
            {
                SetUpGame();
            }
        }
    }
}
