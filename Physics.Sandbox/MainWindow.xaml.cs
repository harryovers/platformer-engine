using Platform.Core;
using Platform.Entities;
using Platform.Entities.Sprites;
using Platform.Entities.Sprites.Bots;
using Platform.Entities.Sprites.Deathboxes;
using Platform.Entities.Sprites.PickUps;
using Platform.Entities.Sprites.Warps;
using Platform.Levels;
using Platform.Physics;
using System;
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

namespace Physics.Sandbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private object updateLock { get; set; }
        //private string[] levels = new[] { "0-0", "0-1", "0-2" };
        private string currentLevel = null;
        private string checkpointLevel = "3-5"; //"0-0";
        private KeyboardState KeyboardState = new KeyboardState();
        private KeyboardState PreviousKeyboardState = new KeyboardState();
        private World world = new World(0, 9.80);
        //private List<Body> _bodies = new List<Body>();
        private Level _level;
        public static Timer drawTimer, updateTimer;
        private bool drawing = true;
        Random rand = new Random();
        //int damage = 0;
        DateTime _lastUpdate;
        //private bool attacking = false;
        //private double attackDistance = 7.0;
        //private double attackSpeed = 0.25;
        //private double attackHeight = 5;
        //private int playerDirection = 1;
        //private bool jumping = false, jumpingSecond = false, jumpColideTestFirst = false;

        public MainWindow()
        {
            InitializeComponent();

            LoadTextures();
            LoadLevel(checkpointLevel, null);

            this.KeyDown += MainWindow_KeyDown;
            this.KeyUp += MainWindow_KeyUp;

            Start();
        }

        private void AddBodyTextures()
        {
            foreach (var body in world._bodies)
            {
                var sprite = body as Sprite;

                if (sprite is Player)
                {
                    sprite.Texture = Textures.Player;
                }
                else if (sprite is Bot)
                {
                    if (sprite is Spider)
                    {
                        sprite.Texture = Textures.Enemy;
                    }
                    else if (sprite is Jumper)
                    {
                        sprite.Texture = Textures.Jumper;
                    }
                    else if (sprite is Charger)
                    {
                        sprite.Texture = Textures.Charger;
                    }
                }
                else if (sprite is Platform.Entities.Sprites.Statics.Block)
                {
                    sprite.Brush = Brushes.Cyan;
                }
                else if (sprite is PickUp)
                {
                    if (sprite is Star)
                    {
                        sprite.Texture = Textures.Star;
                    }
                    else if (sprite is Checkpoint)
                    {
                        sprite.Texture = Textures.Checkpoint;
                    }
                }
                else if (sprite is Spikes)
                {
                    sprite.Texture = Textures.Spikes;
                }
                else if (sprite is Warp)
                {
                    if (sprite is Door)
                    {
                        var door = sprite as Door;

                        if (door.IsLocked)
                        {
                            door.Texture = Textures.LockedDoor;
                        }
                        else
                        {
                            door.Texture = Textures.Door;
                        }
                    }
                }
                else if (sprite is Killbox)
                {
                    sprite.Brush = Brushes.GreenYellow;
                }
            }
        }

        private void LoadLevel(string level, string previousLevel)
        {
            world._bodies.Clear();
            _level = new Level(level, previousLevel);
            currentLevel = level;
            _level.LoadLevelBodies(world);
            AddBodyTextures();
        }

        void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            lock (KeyboardState)
            {
                if (KeyboardState.Contains(e.Key))
                {
                    KeyboardState.Remove(e.Key);
                }
            }
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            lock (KeyboardState)
            {
                if (!KeyboardState.Contains(e.Key))
                {
                    KeyboardState.Add(e.Key);
                }
            }
        }

        private void CheckKeyState()
        {
            lock (KeyboardState)
            {
                PreviousKeyboardState.Clear();
                foreach (var key in KeyboardState)
                {
                    PreviousKeyboardState.Add(key);
                }
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            drawing = false;

            base.OnClosing(e);
        }

        public void Start()
        {
            ThreadStart ts = new ThreadStart(Run);
            Thread t = new Thread(ts);
            t.Start();
        }

        private void Run()
        {
            _lastUpdate = DateTime.Now;

            TimerCallback drawCallback = new TimerCallback(Draw);
            drawTimer = new Timer(drawCallback, null, 0, 1000 / 15);

            updateLock = new object();

            TimerCallback updateCallback = new TimerCallback(Update);
            updateTimer = new Timer(updateCallback, null, 0, 1000 / 120);

            //while (true)
            //{
            //    Update(null);
            //}
        }

        private void CheckPlayerStatus()
        {
            if (_level != null && _level.Player != null && _level.Player.Status != null)
            {
                switch (_level.Player.Status.State)
                {
                    case PlayerState.Dead:
                        LoadLevel(checkpointLevel, null);
                        break;
                    case PlayerState.Warp:
                        var newLevel = _level.Player.Status.Data;
                        LoadLevel(newLevel, currentLevel);
                        break;
                    case PlayerState.Checkpoint:
                        checkpointLevel = currentLevel;
                        break;
                }
            }
        }

        private void Update(object reset)
        {
            lock (updateLock)
            {
                TimeSpan delta = DateTime.Now - _lastUpdate;
                _lastUpdate = DateTime.Now;

                world.Step(delta);

                foreach (var body in world._bodies)
                {
                    var sprite = body as Sprite;

                    lock (KeyboardState)
                    {
                        sprite.Update(delta, KeyboardState, PreviousKeyboardState);
                    }
                }

                CheckKeyState();
                CheckPlayerStatus();
            }
        }

        private void LoadTextures()
        {
            Textures.Load();
        }

        private void Draw(object reset)
        {
            if (drawing)
            {
                Dispatcher.Invoke(() =>
                {
                    canvas.Children.Clear();
                    canvas.Background = Brushes.Black;
                    statusCanvas.Children.Clear();
                    statusCanvas.Background = Brushes.DarkRed;

                    lock (updateLock)
                    {
                        foreach (var body in world._bodies)
                        {
                            var sprite = body as Sprite;

                            sprite.Draw(canvas, statusCanvas);
                        }
                    }
                });
            }
        }
    }
}
