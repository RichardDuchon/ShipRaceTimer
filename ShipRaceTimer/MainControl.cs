using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Threading;
using System.Timers;
using System.Media;

namespace ShipRaceTimer
{
    public class MainControl : ChangeTracker
    {
        DispatcherTimer timer;
        int displayMinitNumber;
        int displaySecondsFirstNumber;
        int displaySecondsSecondNumber;
        bool timerIsTicking = true;
        SoundPlayer tick = new SoundPlayer(ShipRaceTimer.Properties.Resources.Tick);
        SoundPlayer siren = new SoundPlayer(ShipRaceTimer.Properties.Resources.Siren);
        SoundPlayer countDownEnd = new SoundPlayer(ShipRaceTimer.Properties.Resources.CountDownEnd);


        private int[] _colorOfMinitsNumber;
        public int[] colorOfMinitsNumber
        {
            get
            {
                return _colorOfMinitsNumber;
            }
            set
            {
                _colorOfMinitsNumber = value;
                change("colorOfMinitsNumber");

            }
        }

        private int[] _colorOfSecondsFirstNumber;
        public int[] colorOfSecondsFirstNumber
        {
            get
            {
                return _colorOfSecondsFirstNumber;
            }
            set
            {
                _colorOfSecondsFirstNumber = value;
                change("colorOfSecondsFirstNumber");

            }
        }

        private int[] _colorOfSecondsSecondNumber;
        public int[] colorOfSecondsSecondNumber
        {
            get
            {
                return _colorOfSecondsSecondNumber;
            }
            set
            {
                _colorOfSecondsSecondNumber = value;
                change("colorOfSecondsSecondNumber");

            }
        }
        public int[][] colorOfNumber =
        {
            new int[]{1, 1, 1, 1, 1, 1, 1}, //green
            new int[]{2, 2, 2, 2, 2, 2, 2}, //yellow
            new int[]{3, 3, 3, 3, 3, 3, 3}, //red
            new int[]{4, 4, 4, 4, 4, 4, 4}  //black
        };


        private bool[] _minitsNumber;
        public bool[] minitsNumber
        {
            get
            {
                return _minitsNumber;
            }
            set
            {
                _minitsNumber = value;
                change("minitsNumber");
            }
        }

        private bool[] _secondsFirstNumber;
        public bool[] secondsFirstNumber
        {
            get
            {
                return _secondsFirstNumber;
            }
            set
            {
                _secondsFirstNumber = value;
                change("secondsFirstNumber");
            }
        }

        private bool[] _secondsSecondtNumber;
        public bool[] secondsSecondNumber
        {
            get
            {
                return _secondsSecondtNumber;
            }
            set
            {
                _secondsSecondtNumber = value;
                change("secondsSecondNumber");
            }
        }

        public bool[][] sevenSegmentNumber =
       {
            //numbers are positions in array
            //0 _
            //5|_|1
            //  6

            //4|_|2
            // 3
            new bool[]{true, true, true, true, true, true, false}, //zero
            new bool[]{false, true, true, false, false, false, false}, //one
            new bool[]{true, true, false, true, true, false, true}, //two
            new bool[]{true, true, true, true, false, false, true}, //three
            new bool[]{false, true, true, false, false, true, true}, //four
            new bool[]{true, false, true, true, false, true, true}, //five
            new bool[]{true, false, true, true, true, true, true}, //six
            new bool[]{true, true, true, false, false, false, false}, //seven
            new bool[]{true, true, true, true, true, true, true}, //eight
            new bool[]{true, true, true, false, false, true, true}, //nine
            new bool[]{false, false, false, false, false, false, false} //nothing

        };

        private bool[] _modeArray = new bool[] { true, false, false };
        public bool[] ModeArray
        {
            get { return _modeArray; }
        }

        public int SelectedMode
        {
            get { return Array.IndexOf(_modeArray, true); }
        }

        private ICommand _clickCommand;
        public ICommand clickComand
        {
            get
            {
                return _clickCommand = new Comands(showNumber);
            }
        }

        private ICommand _stopTime;
        public ICommand StopTime
        {
            get
            {
                return _stopTime = new Comands(stopTimer);
            }
        }

        public bool CanExecute
        {
            get { return true; }
        }



        public void stopTimer()
        {
            timer.Stop();
            timerIsTicking = true;
        }

        public void showNumber()
        {


            if (timerIsTicking)
            {
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Tick += timer_tick;
                timer.Start();


                if (SelectedMode == 0)
                {
                    displayMinitNumber = 5;
                }
                else if (SelectedMode == 1)
                {
                    displayMinitNumber = 3;
                }
                else if (SelectedMode == 2)
                {
                    displayMinitNumber = 1;
                }

                displaySecondsSecondNumber = 0;
                displaySecondsFirstNumber = 0;

                colorOfMinitsNumber = colorOfNumber[2];
                colorOfSecondsFirstNumber = colorOfNumber[0];
                colorOfSecondsSecondNumber = colorOfNumber[0];

                secondsSecondNumber = sevenSegmentNumber[10];
                secondsFirstNumber = sevenSegmentNumber[10];
                minitsNumber = sevenSegmentNumber[10];

            }
            timerIsTicking = false;
        }


        private void timer_tick(object sender, EventArgs e)
        {


            secondsSecondNumber = sevenSegmentNumber[displaySecondsSecondNumber];
            secondsFirstNumber = sevenSegmentNumber[displaySecondsFirstNumber];
            minitsNumber = sevenSegmentNumber[displayMinitNumber];


            if (displaySecondsSecondNumber > 0)
            {
                displaySecondsSecondNumber--;
                tick.Play();

                if (displayMinitNumber == 0 && displaySecondsFirstNumber == 0)
                    colorOfSecondsFirstNumber = colorOfNumber[3];



            }
            else if (displaySecondsFirstNumber > 0)
            {
                tick.Play();

                displaySecondsFirstNumber--;

                if (displayMinitNumber == 0 && displaySecondsFirstNumber == 0)
                {
                    colorOfSecondsSecondNumber = colorOfNumber[1];
                    colorOfSecondsFirstNumber = colorOfNumber[1];

                }

                displaySecondsSecondNumber = 9;
            }
            else if (displayMinitNumber > 0)
            {
                siren.Play();
                displayMinitNumber--;

                if (displayMinitNumber == 0)
                {
                    colorOfMinitsNumber = colorOfNumber[3];
                }
                displaySecondsSecondNumber = 9;
                displaySecondsFirstNumber = 5;
            }
            else
            {
                colorOfSecondsSecondNumber = colorOfNumber[2];

                countDownEnd.Play();
                timer.Stop();
                timerIsTicking = true;

            }
        }
    }
}
