using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace tictactoe
{
    public partial class Form1 : Form
    {
        
        public enum Player
        {
            X, O
        }

        public enum GameMode
        {
            SinglePlayer,
            MultiPlayer
        }

        Player currentPlayer;
        GameMode currentGameMode;
        List<Button> buttons;
        Random rand = new Random();

        // Replaced multiple separate counters with PlayerInfo objects
        PlayerInfo playerXInfo;
        PlayerInfo playerOInfo;

        bool gameOver = false;
        bool isAITurning = false; 

        public Form1()
        {
            InitializeComponent();
            AskGameMode();
            resetGame();
        }

        private void AskGameMode()
        {
            DialogResult result = MessageBox.Show(
                "Escolha o modo de jogo:\n\nSim = Singleplayer (contra a IA)\nNão = Multiplayer (2 jogadores)",
                "Modo de Jogo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            currentGameMode = result == DialogResult.Yes ? GameMode.SinglePlayer : GameMode.MultiPlayer;

            if (currentGameMode == GameMode.MultiPlayer)
            {
                playerXInfo = new PlayerInfo("Jogador X", "X");
                playerOInfo = new PlayerInfo("Jogador O", "O");

                label1.Text = $"{playerXInfo.Name} - {playerXInfo.Wins}";
                label2.Text = $"{playerOInfo.Name} - {playerOInfo.Wins}";
                label1.ForeColor = Color.Blue;
                label2.ForeColor = Color.Red;
                this.Text = "Tic Tac Toe - Vez do Jogador X";
            }
            else
            {
                playerXInfo = new PlayerInfo("Player", "X");
                playerOInfo = new PlayerInfo("AI", "O");

                label1.Text = $"{playerXInfo.Name} - {playerXInfo.Wins}";
                label2.Text = $"{playerOInfo.Name} - {playerOInfo.Wins}";
                label1.ForeColor = Color.Green;
                label2.ForeColor = Color.Red;
                this.Text = "Tic Tac Toe - Singleplayer";
            }
        }

        private void playerClick(object sender, EventArgs e)
        {
            if (gameOver) return;
            if (currentGameMode == GameMode.SinglePlayer && (currentPlayer == Player.O || isAITurning))
            {
                MessageBox.Show("Aguarde a vez do adversário!", "Não é sua vez", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var button = (Button)sender;

            if (currentGameMode == GameMode.SinglePlayer)
            {
                // Player in singleplayer always plays with X
                button.Text = playerXInfo.Symbol;
                button.Enabled = false;
                button.BackColor = Color.Cyan;
                buttons.Remove(button);

                if (!CheckWinCondition(playerXInfo.Symbol))
                {
                    if (buttons.Count > 0 && !gameOver)
                    {
                        currentPlayer = Player.O;
                        isAITurning = true; 
                        AImoves.Start();
                    }
                    else if (buttons.Count == 0 && !gameOver)
                    {
                        MessageBox.Show("Empate!", "Fim de Jogo");
                        resetGame();
                    }
                }
            }
            else
            {
                if (button.Text == "?")
                {
                    button.Text = currentPlayer.ToString();
                    button.Enabled = false;
                    button.BackColor = currentPlayer == Player.X ? Color.LightBlue : Color.LightCoral;
                    buttons.Remove(button);

                    string symbol = currentPlayer.ToString();

                    if (!CheckWinCondition(symbol))
                    {
                        if (buttons.Count == 0)
                        {
                            MessageBox.Show("Empate!", "Fim de Jogo");
                            resetGame();
                        }
                        else
                        {
                            currentPlayer = currentPlayer == Player.X ? Player.O : Player.X;
                            this.Text = $"Tic Tac Toe - Vez do Jogador {currentPlayer}";
                        }
                    }
                }
            }
        }

        private void AImove(object sender, EventArgs e)
        {
            AImoves.Stop();

            if (gameOver || buttons == null || buttons.Count == 0)
            {
                isAITurning = false;
                return;
            }

            int index = GetSmartAIMove();

            if (index == -1)
            {
                index = rand.Next(buttons.Count);
            }

            MakeAIMove(index);
        }

        private int GetSmartAIMove()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Button btn = buttons[i];
                string originalText = btn.Text;

                btn.Text = playerOInfo.Symbol;
                if (CheckWinCondition(playerOInfo.Symbol, true))
                {
                    btn.Text = originalText;
                    return i;
                }
                btn.Text = originalText;
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                Button btn = buttons[i];
                string originalText = btn.Text;

                btn.Text = playerXInfo.Symbol;
                if (CheckWinCondition(playerXInfo.Symbol, true))
                {
                    btn.Text = originalText;
                    return i;
                }
                btn.Text = originalText;
            }

            Button centerButton = GetButtonAtPosition(1, 1);
            if (centerButton != null && buttons.Contains(centerButton))
            {
                return buttons.IndexOf(centerButton);
            }

            // 4. Tenta cantos
            List<Button> corners = GetCornerButtons();
            var availableCorners = corners.Where(c => buttons.Contains(c)).ToList();
            if (availableCorners.Count > 0)
            {
                return buttons.IndexOf(availableCorners[rand.Next(availableCorners.Count)]);
            }

            return -1;
        }

        private Button GetButtonAtPosition(int row, int col)
        {
            var buttonMap = new Dictionary<(int, int), Button>
            {
                {(0, 0), button1}, {(0, 1), button2}, {(0, 2), button3},
                {(1, 0), button4}, {(1, 1), button5}, {(1, 2), button6},
                {(2, 0), button7}, {(2, 1), button8}, {(2, 2), button9}
            };

            return buttonMap.TryGetValue((row, col), out Button btn) ? btn : null;
        }

        private List<Button> GetCornerButtons()
        {
            return new List<Button> { button1, button3, button7, button9 };
        }

        private void MakeAIMove(int index)
        {
            var btn = buttons[index];
            btn.Enabled = false;
            btn.Text = playerOInfo.Symbol;
            btn.BackColor = Color.DarkSalmon;
            buttons.RemoveAt(index);

            if (!CheckWinCondition(playerOInfo.Symbol))
            {
                currentPlayer = Player.X;
                if (buttons.Count == 0 && !gameOver)
                {
                    MessageBox.Show("Empate!", "Fim de Jogo");
                    resetGame();
                }
            }

            isAITurning = false;
        }

        private bool CheckWinCondition(string symbol, bool simulate = false)
        {
            if ((button1.Text == symbol && button2.Text == symbol && button3.Text == symbol) ||
                (button4.Text == symbol && button5.Text == symbol && button6.Text == symbol) ||
                (button7.Text == symbol && button8.Text == symbol && button9.Text == symbol) ||
                (button1.Text == symbol && button4.Text == symbol && button7.Text == symbol) ||
                (button2.Text == symbol && button5.Text == symbol && button8.Text == symbol) ||
                (button3.Text == symbol && button6.Text == symbol && button9.Text == symbol) ||
                (button1.Text == symbol && button5.Text == symbol && button9.Text == symbol) ||
                (button3.Text == symbol && button5.Text == symbol && button7.Text == symbol))
            {
                if (!simulate)
                {
                    AImoves.Stop();
                    gameOver = true;
                    isAITurning = false; 

                    PlayerInfo winnerInfo = symbol == playerXInfo.Symbol ? playerXInfo : playerOInfo;

                    winnerInfo.Wins++;

                    // Update UI labels according to mode
                    if (currentGameMode == GameMode.SinglePlayer)
                    {
                        label1.Text = $"{playerXInfo.Name} - {playerXInfo.Wins}";
                        label2.Text = $"{playerOInfo.Name} - {playerOInfo.Wins}";
                    }
                    else
                    {
                        label1.Text = $"{playerXInfo.Name} - {playerXInfo.Wins}";
                        label2.Text = $"{playerOInfo.Name} - {playerOInfo.Wins}";
                    }

                    string winner = currentGameMode == GameMode.SinglePlayer
                        ? (symbol == playerXInfo.Symbol ? playerXInfo.Name : playerOInfo.Name)
                        : (symbol == playerXInfo.Symbol ? "Jogador X" : "Jogador O");

                    MessageBox.Show($"{winner} venceu!", "Fim de Jogo");

                    System.Windows.Forms.Timer delayTimer = new System.Windows.Forms.Timer();
                    delayTimer.Interval = 1000;
                    delayTimer.Tick += (s, args) => {
                        delayTimer.Stop();
                        resetGame();
                    };
                    delayTimer.Start();
                }
                return true;
            }
            return false;
        }

        private void restartGame(object sender, EventArgs e)
        {
            AImoves.Stop();
            gameOver = false;
            isAITurning = false; 
            resetGame();
        }

        private void resetGame()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button && control.Tag?.ToString() == "play")
                {
                    Button btn = (Button)control;
                    btn.Enabled = true;
                    btn.Text = "?";
                    btn.BackColor = SystemColors.Control;
                }
            }

            loadbuttons();
            gameOver = false;
            isAITurning = false; 
            currentPlayer = Player.X;

            if (currentGameMode == GameMode.MultiPlayer)
            {
                this.Text = "Tic Tac Toe - Vez do Jogador X";
            }
            else
            {
                this.Text = "Tic Tac Toe - Singleplayer";
            }

            // Refresh score labels in case PlayerInfo changed
            if (playerXInfo != null && playerOInfo != null)
            {
                label1.Text = $"{playerXInfo.Name} - {playerXInfo.Wins}";
                label2.Text = $"{playerOInfo.Name} - {playerOInfo.Wins}";
            }
        }

        private void loadbuttons()
        {
            buttons = new List<Button> {
                button1, button2, button3,
                button4, button5, button6,
                button7, button8, button9
            };
        }
    }
}