using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Physics.Sandbox
{
    [Obsolete("Move to Core")]
    public static class Textures
    {
        private static BitmapImage playerTexture;

        public static BitmapImage Player
        {
            get { return playerTexture; }
            set { playerTexture = value; }
        }

        private static BitmapImage brickTexture;

        public static BitmapImage Brick
        {
            get { return brickTexture; }
            set { brickTexture = value; }
        }

        private static BitmapImage spikeTexture;

        public static BitmapImage Spikes
        {
            get { return spikeTexture; }
            set { spikeTexture = value; }
        }

        private static BitmapImage starTexture;

        public static BitmapImage Star
        {
            get { return starTexture; }
            set { starTexture = value; }
        }

        private static BitmapImage enemyTexture;

        public static BitmapImage Enemy
        {
            get { return enemyTexture; }
            set { enemyTexture = value; }
        }

        private static BitmapImage jumperTexture;

        public static BitmapImage Jumper
        {
            get { return jumperTexture; }
            set { jumperTexture = value; }
        }

        private static BitmapImage chargerTexture;

        public static BitmapImage Charger
        {
            get { return chargerTexture; }
            set { chargerTexture = value; }
        }

        private static BitmapImage doorTexture;

        public static BitmapImage Door
        {
            get { return doorTexture; }
            set { doorTexture = value; }
        }

        private static BitmapImage lockedDoorTexture;

        public static BitmapImage LockedDoor
        {
            get { return lockedDoorTexture; }
            set { lockedDoorTexture = value; }
        }

        private static BitmapImage checkpointTexture;

        public static BitmapImage Checkpoint
        {
            get { return checkpointTexture; }
            set { checkpointTexture = value; }
        }

        public static void Load()
        {
            playerTexture = new BitmapImage();
            playerTexture.BeginInit();
            playerTexture.UriSource = new Uri(@"/Physics.Sandbox;component/Assets/man.png", UriKind.Relative);
            playerTexture.EndInit();

            brickTexture = new BitmapImage();
            brickTexture.BeginInit();
            brickTexture.UriSource = new Uri(@"/Physics.Sandbox;component/Assets/brick.png", UriKind.Relative);
            brickTexture.EndInit();

            spikeTexture = new BitmapImage();
            spikeTexture.BeginInit();
            spikeTexture.UriSource = new Uri(@"/Physics.Sandbox;component/Assets/spikes.png", UriKind.Relative);
            spikeTexture.EndInit();

            starTexture = new BitmapImage();
            starTexture.BeginInit();
            starTexture.UriSource = new Uri(@"/Physics.Sandbox;component/Assets/star.png", UriKind.Relative);
            starTexture.EndInit();

            enemyTexture = new BitmapImage();
            enemyTexture.BeginInit();
            enemyTexture.UriSource = new Uri(@"/Physics.Sandbox;component/Assets/enemy.png", UriKind.Relative);
            enemyTexture.EndInit();

            jumperTexture = new BitmapImage();
            jumperTexture.BeginInit();
            jumperTexture.UriSource = new Uri(@"/Physics.Sandbox;component/Assets/jumper.png", UriKind.Relative);
            jumperTexture.EndInit();

            chargerTexture = new BitmapImage();
            chargerTexture.BeginInit();
            chargerTexture.UriSource = new Uri(@"/Physics.Sandbox;component/Assets/charger.png", UriKind.Relative);
            chargerTexture.EndInit();

            doorTexture = new BitmapImage();
            doorTexture.BeginInit();
            doorTexture.UriSource = new Uri(@"/Physics.Sandbox;component/Assets/door.png", UriKind.Relative);
            doorTexture.EndInit();

            lockedDoorTexture = new BitmapImage();
            lockedDoorTexture.BeginInit();
            lockedDoorTexture.UriSource = new Uri(@"/Physics.Sandbox;component/Assets/lockedDoor.png", UriKind.Relative);
            lockedDoorTexture.EndInit();

            checkpointTexture = new BitmapImage();
            checkpointTexture.BeginInit();
            checkpointTexture.UriSource = new Uri(@"/Physics.Sandbox;component/Assets/checkpoint.png", UriKind.Relative);
            checkpointTexture.EndInit();
        }
    }
}
