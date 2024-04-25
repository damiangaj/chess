using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
using ChessCore;
using System.Net;


namespace ChessGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private readonly Image[,] piecePngs = new Image[8, 8];
        private readonly Rectangle[,] lights = new Rectangle[8, 8];
        public event EventHandler NewGameStarted;

        private readonly Dictionary<Position, Move> detect = new Dictionary<Position, Move>();

        private Tour tour;
        private Position sel = null;
        ServerHost serverHost;
        ServerClient serverClient;


        public MainWindow()
        {
            serverHost = null;
            serverClient = null;
            InitializeComponent();
            OpenStartMenu();
        }


        private void Start()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Image image = new Image();
                    piecePngs[i, j] = image;
                    PieceGrid.Children.Add(image);

                    Rectangle light = new Rectangle();
                    lights[i, j] = light;
                    Highlight.Children.Add(light);
                }
            }

            tour = new Tour(Player.White, Board.Start());
            GenerateBoard(tour.Board);
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private void GenerateBoard(Board board)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece piece = board[i, j];
                    piecePngs[i, j].Source = Pngs.TakePng(piece);
                }
            }
        }
        private void BoardGrid_MauseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMenuActive())
            {
                return;
            }

            Point point = e.GetPosition(BoardGrid);
            Position pos = ToPosition(point);

            if (sel == null)
            {
                FromPosition(pos);
            }
            else
            {
                ToPosition(pos);
            }
        }

        private Position ToPosition(Point point)
        {
            double size = BoardGrid.ActualWidth / 8;
            int y = (int)(point.Y / size);
            int x = (int)(point.X / size);
            return new Position(y, x);
        }


        private void FromPosition(Position pos)
        {
            IEnumerable<Move> moves = tour.ValidMove(pos);

            if (moves.Any())
            {
                sel = pos;
                DetectMove(moves);
                OnLight();
            }

        }

        private void ToPosition(Position pos)
        {
            sel = null;
            OffLight();
            if (detect.TryGetValue(pos, out Move move))
            {
                if (move.Type == Moves.Replacement)
                {
                    HandleReplacement(move.From, move.To);
                }
                else
                {
                    Handle(move);
                }
            }
        }

        private void HandleReplacement(Position from, Position to)
        {
            piecePngs[to.X, to.Y].Source = Pngs.TakePng(tour.CurrentPlayer, PieceVariant.Pawn);
            piecePngs[from.X, from.Y].Source = null;

            ChangingPawnMenu changingMenu = new ChangingPawnMenu(tour.CurrentPlayer);
            MenuBox.Content = changingMenu;

            changingMenu.PieceChoosed += type =>
            {
                MenuBox.Content = null;
                Move changingMove = new PawnReplacement(from, to, type);
                Handle(changingMove);
            };
        }

        private void Keyboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsMenuActive() && e.Key == Key.Escape)
            {
                PauseMenu();
            }
        }

        private void OpenStartMenu()
        {
            StartMenu startMenu = new StartMenu();
            MenuBox.Content = startMenu;

            startMenu.ChoosedOption += option =>
            {
                if (option == Option.Start)
                {
                    MenuBox.Content = null;
                    Start();
                }
                if (option == Option.Exit)
                {
                    Exit();
                }
                if (option == Option.HostMenu)
                {
                    MenuBox.Content = null;
                    HostMenu();
                }
                if (option == Option.JoinMenu)
                {
                    MenuBox.Content = null;
                    JoinMenu();
                }
            };
        }

        private void PauseMenu()
        {
            Restart pauseMenu = new Restart();
            MenuBox.Content = pauseMenu;

            pauseMenu.ChoosedOption += option =>
            {
                if (option == Option.Continue)
                {
                    MenuBox.Content = null;
                }

                if (option == Option.Restart)
                {
                    MenuBox.Content = null;
                    RestartGame();
                }
            };
        }

        private void HostMenu()
        {
            HostGame hostGame = new HostGame();
            
            MenuBox.Content = hostGame;
            hostGame.ChoosedOption += option =>
            {
                if (option == Option.Start)
                {
                    serverHost.StartGame();
                }

                if (option == Option.Back)
                {
                    MenuBox.Content = null;
                    OpenStartMenu();

                }

                if (option == Option.StartHosting)
                {
                    serverHost = new ServerHost();
                    serverHost.StartHosting(hostGame);
                    serverHost.NewGameStarted += ServerClient_NewGameStarted;
                }

                if (option == Option.StopHosting)
                {

                    serverHost.StopHosting();
                    serverHost = null;
                }
            };

        }

        private void JoinMenu()
        {
            JoinGame joinGame = new JoinGame();
            MenuBox.Content = joinGame;
            serverClient = new ServerClient();
            serverClient.NewGameStarted += ServerClient_NewGameStarted;
            joinGame.ChoosedOption += option =>
            {
                if (option == Option.Back)
                {
                    serverClient.Disconnect();
                    MenuBox.Content = null;
                    serverClient = null;

                    OpenStartMenu();
                }
                if (option == Option.Start)
                {
                    serverClient.Connect(joinGame);
                }


            };
        }

        private void Handle(Move move)
        {
            tour.MakeMove(move);
        
            GenerateBoard(tour.Board);
            if (tour.IsGameEnded())
            {
                GameEndingMenu();
            }

        }
        private void DetectMove(IEnumerable<Move> moves)
        {
            detect.Clear();
            foreach (Move mov in moves)
            {
                detect[mov.To] = mov;
            }
        }

        private void OnLight()
        {
            Color color = Color.FromArgb(150, 126, 255, 126);

            foreach (Position to in detect.Keys)
            {

                lights[to.X, to.Y].Fill = new SolidColorBrush(color);
            }
        }

        private void OffLight()
        {


            foreach (Position to in detect.Keys)
            {

                lights[to.X, to.Y].Fill = Brushes.Transparent;
            }

        }

        private bool IsMenuActive()
        {
            return MenuBox.Content != null;
        }

        private void GameEndingMenu()
        {
            GameEndingMenu gameEndingMenu = new GameEndingMenu(tour);
            MenuBox.Content = gameEndingMenu;

            gameEndingMenu.ChoosedOption += option =>
            {
                if (option == Option.Restart)
                {
                    MenuBox.Content = null;
                    RestartGame();
                }
            };
        }

        private void RestartGame()
        {
            sel = null;
            OffLight();
            detect.Clear();
            tour = new Tour(Player.White, Board.Start());
            GenerateBoard(tour.Board);
        }

        private void ServerClient_NewGameStarted(object sender, EventArgs e)
        {
            MenuBox.Visibility = Visibility.Collapsed;
            Start();
        }


    }
}
