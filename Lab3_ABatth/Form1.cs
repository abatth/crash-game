using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Drawer_Lab3;
using GDIDrawer;
using System.Drawing;
using System.Diagnostics;

namespace Lab3_ABatth
{
    public partial class Form1 : Form
    {
        PicDrawer pdrawer = new PicDrawer(Properties.Resources.Crash, false); //create new pdrawer instance and set greyscale bool to false so picture is created with color 
        List<Car> _listCars = new List<Car>(); //create list of Car type 
        Car _newCar = null; //set Car variable to null, cant initialize because abstract class
        int _gameScore = 0; //variable to hold the score 
        Point _mouseClick; //variable to hold click by user 


        public Form1()
        {
            InitializeComponent();

            Car.Canvas = pdrawer; //set the class canvas to the pdrawer 

            NewCar.Tick += NewCar_Tick; //NewCar Timer Tick event handler
            Crash.Tick += Crash_Tick; //Crash Timer Tick event handler
            UI_btnPlay.Click += UI_btnPlay_Click; //Play Again Button Click event handler
        }

        //added button to Play Again, disabled at first start up
        private void UI_btnPlay_Click(object sender, EventArgs e) 
        {
            _gameScore = 0; //reset to 0
            _listCars.Clear(); //clear list of cars
            
            NewCar.Enabled = true; // re-enable the new car timer
            Crash.Enabled = true; // re-enable the crash game timer
            UI_btnPlay.Enabled = false; //once clicked set back to disabled 
        }

        //Timer to create new car - set to 4 second delay to avoid early collisions
        private void NewCar_Tick(object sender, EventArgs e)
        {

            //create random new car
            //values set the speed and direction of cars 
            switch (Car.Rand.Next(0, 8)) 
            {
                case 0:
                    _newCar = new VSedan(5); //VSedan going Down
                    break;
                case 1:
                    _newCar = new VSedan(-5); //VSedan going Up
                    break;
                case 2:
                    _newCar = new HAmbulance(7); //Ambulance going right
                    break;
                case 3:
                    _newCar = new HAmbulance(-7); //Ambulance going left
                    break;
                case 4:
                    _newCar = new PoliceCar(8); //Police car going right
                    break;
                case 5:
                    _newCar = new PoliceCar(-8);// Police car going left
                    break;
                case 6:
                    _newCar = new Mustang(9); //Mustang going Down
                    break;
                case 7:
                    _newCar = new Mustang(-9); //Mustang going Up
                    break; 
            }

            _listCars.Add(_newCar); //add the new car to the list 
        }


        //Timer of Game
        private void Crash_Tick(object sender, EventArgs e)
        {

            //if drawer is null return 
            if (pdrawer == null) return; 

            if (pdrawer.GetLastMouseLeftClick(out _mouseClick)) //if user clicks mouse on drawer
            {
                foreach(Car c in _listCars) //iterate through list of cars collection
                {
                    if (c.PointOnCar(_mouseClick) == true) //if that particular c is pointed on
                    {
                        c.ToggleSpeed(); //toggle the speed of the that car 
                    }
                }
            }

            _listCars.ForEach(c => c.Move()); //move all cars


            Car _tempCar; //create a temp car variable 

            foreach (Car c in _listCars.ToList()) //iterate through car list, add ToList() to avoid enumeration modified exception
            {
                if (_listCars.Contains(c))
                {
                    _tempCar = _listCars[_listCars.IndexOf(c)]; //make temp car compare to cars and see if they are equal thus collided
                    _gameScore += c.GetHitScore() + _tempCar.GetHitScore(); //gamescore recieves penalty based on gethitscores of both cars 
                    //remove both cars that collide
                    _listCars.RemoveAll(car => ReferenceEquals(car, c));
                    _listCars.RemoveAll(car => ReferenceEquals(car, _tempCar));
                }

                if (Car.OutOfBounds(c)) //if car is out of bounds
                {
                    _gameScore += c.GetSafeScore(); //gamescore increases based on the GetSafeScore returned value of car
                    _listCars.RemoveAll(car => ReferenceEquals(car, c)); //remove the car from the list 
                }
            }



            _labelScore.Text = $"Score : {_gameScore}"; //label on Form updates the Score
            _listCars.ForEach(c => (c as IAnimateable)?.Animate()); //invoke IAnimateable method Animate() for all Cars in list that support interface
            pdrawer.Clear(); //clear the drawer 
            _listCars.ForEach(c => c.ShowCar()); //show each car in the list

            pdrawer.AddText($"{_labelScore.Text}", 30, Color.Red); //add Score on Drawer as well 
            pdrawer.Render(); //render drawer


            if (_gameScore < 0) //if the game score is negative
            {
                pdrawer.Clear(); //clear the drawer
                pdrawer.AddText("Game Over", 30, Color.Red); //display Game Over on drawer
                pdrawer.Render(); //render the drawer so text shows

                NewCar.Enabled = false; //set the new car Timer to false so it stops
                Crash.Enabled = false; //set the Crash timer to false
                UI_btnPlay.Enabled = true; //enable the Play Again button so user can click to play again 
            }
        }
    }


    
}
