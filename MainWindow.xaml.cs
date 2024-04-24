using Microsoft.VisualBasic;
using System.Windows;
using System.Windows.Controls;

namespace TickTackToe
{

    public partial class MainWindow : Window
    {
        private bool isPlayerTurn = true;
        private string[,] gameState = new string[3, 3];
        private int playerWins = 0;
        private int opponentWins = 0;
        private string playerName = "Player";
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            SetPlayerName();
            UpdatePlayerWinsText();
            InitializeGameState();
        }

        private void SetPlayerName()
        {
            string inputName = Interaction.InputBox("Whats your name?:", "Name Please...", "...", -1, -1);
            if (!string.IsNullOrWhiteSpace(inputName))
            {
                playerName = inputName;
            }
        }

        private void InitializeGameState()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    gameState[i, j] = "";

            foreach (Button btn in Grid.Children.OfType<Button>()) 
            {
                btn.Content = "";
            }
        }
        private void UpdatePlayerWinsText()
        {
            playerWinsText.Text = $"{playerName} Wins: {playerWins}";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Content.ToString() == "" && isPlayerTurn)
            {
                button.Content = "X";
                UpdateGameState(button.Name, "X");

                if (CheckForWin("X"))
                {
                    playerWins++;
                    playerWinsText.Text = $"{playerName} Wins: {playerWins}";
                    MessageBox.Show("X Wins!");
                    InitializeGameState();
                }
                else if (!IsBoardFull())
                {
                    isPlayerTurn = false;
                    AITurn();
                }

                if (CheckForWin("O"))
                {
                    opponentWins++;
                    opponentWinsText.Text = $"Opponent Wins: {opponentWins}";
                    MessageBox.Show("O Wins!");
                    InitializeGameState();
                }
                else if (IsBoardFull())
                {
                    MessageBox.Show("Now that's a surprise...");
                    InitializeGameState();
                }
            }
        }

        private void AITurn()
        {
            List<Button> availableButtons = new List<Button>();
            foreach (Button btn in Grid.Children.OfType<Button>())
            {
                if (btn.Content.ToString() == "")
                {
                    availableButtons.Add(btn);
                }
            }

            if (availableButtons.Count > 0)
            {
                // its random for now, but i have an idea for exanding the AI where it first look for possible wins on the next turn, and if it cant find any, it checks if the player can win the next turn, and then blocks that win if it finds any
                int index = random.Next(availableButtons.Count);
                Button aiButton = availableButtons[index];
                aiButton.Content = "O";
                UpdateGameState(aiButton.Name, "O");
                isPlayerTurn = true;
            }
        }

        private bool IsBoardFull()
        {
            foreach (var value in gameState)
            {
                if (value == "") return false;
            }
            return true;
        }


        private void UpdateGameState(string buttonName, string playerSymbol)
        {
            int row = 0, column = 0;

            // map buttons to array
            switch (buttonName)
            {
                case "btnTopLeft": row = 0; column = 0; break;
                case "btnTopCenter": row = 0; column = 1; break;
                case "btnTopRight": row = 0; column = 2; break;
                case "btnLeft": row = 1; column = 0; break;
                case "btnCenter": row = 1; column = 1; break;
                case "btnRight": row = 1; column = 2; break;
                case "btnBottomLeft": row = 2; column = 0; break;
                case "btnBottomCenter": row = 2; column = 1; break;
                case "btnBottomRight": row = 2; column = 2; break;
            }
            gameState[row, column] = playerSymbol;
        }
        private bool CheckForWin(string playerSymbol)
        {
            // rows and columns
            for (int i = 0; i < 3; i++)
            {
                if ((gameState[i, 0] == playerSymbol && gameState[i, 1] == playerSymbol && gameState[i, 2] == playerSymbol) ||
                    (gameState[0, i] == playerSymbol && gameState[1, i] == playerSymbol && gameState[2, i] == playerSymbol))
                    return true;
            }

            // diagonals
            if ((gameState[0, 0] == playerSymbol && gameState[1, 1] == playerSymbol && gameState[2, 2] == playerSymbol) ||
                (gameState[0, 2] == playerSymbol && gameState[1, 1] == playerSymbol && gameState[2, 0] == playerSymbol))
                return true;

            return false;
        }
    }
}