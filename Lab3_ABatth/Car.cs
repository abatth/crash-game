using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDIDrawer;
using System.Drawing;
using Drawer_Lab3;

namespace Lab3_ABatth
{
    abstract class Car //abstract base class, used for all derived Cars
    {
        private static PicDrawer _canvas;  //static PicDrawer reference
        public static PicDrawer Canvas //Manual property - normal get and set
        {
            get
            {
                return _canvas; //return _canvas 
            }

            set
            {
                if (_canvas != null) //if canvas not null
                {
                    _canvas.Close(); //close existing canvas
                }

                else
                {
                    _canvas = value; //assign _canvas to the value 
                }
            }
        }

        public static Random Rand { get; set; } = new Random(); //public static Random object as an automatic property 

        //readonly collections which will hold lane X or Y values.
        // Up & Down will hold X lane value
        // Left & Right will hold y lane value
        protected static readonly List<int> _downs = new List<int>(){ 170, 490 }; // Down
        protected static readonly List<int> _ups = new List<int>(){ 270, 590 }; // Up
        protected static readonly List<int> _left = new List<int>() { 164 }; //Left
        protected static readonly List<int> _right = new List<int>(){ 260 }; //Right

        protected int _xcoord; // : x coordinates 
        protected int _ycoord; // y coordinates 
        protected int _width;  //width of car
        protected int _height; //height of car
        protected int _speed; //speed of car

        protected bool MaxSpeed; //bool representing max speed and half speed 

        //CTOR accepting speed, width, and height
        public Car(int speed, int width, int height)
        {
            //initialize variables
            _speed = speed;
            _width = width;
            _height = height;
            MaxSpeed = true; //initialize so it starts at full speed 
        }


        public abstract Rectangle GetRect(); //Abstract method used in derived classes depending on type of Car they are


        //! Use NVI pattern, create a public ShowCar() and protected abstract VShowCar() set of methods to Show our Car-------
        public void ShowCar()
        {
            VShowCar();
        }

        protected abstract void VShowCar(); 


        //! Use NVI pattern, create a public Move() and protected abstract VShowCar() set of methods to Move our Car ------ 
        public void Move()
        {
            VMove();
        }

        protected abstract void VMove();



        public override bool Equals(object obj) //Override Equals
        {
            if (!(obj is Car car)) return false; //if incorrect type return 
            if (ReferenceEquals(this, car)) return false; //Return false if the objects being compared are the same references

            return this.GetRect().IntersectsWith(car.GetRect()); //return true if the GetRect() returned Rectangle arguments are overlapping
        }

        public override int GetHashCode() //override gethashcode and return 0
        {
            return 0; 
        }


        public bool PointOnCar(Point point) 
        {
            return GetRect().Contains(point); //return true if the Point is within the Car's Rectangle - use Rectangle.Contains()
        }


        //static helper predicate 
        public static bool OutOfBounds(Car car)
        {
            Rectangle _temp = new Rectangle(0, 0, _canvas.ScaledWidth, _canvas.ScaledHeight); //variable that holds in bounds (Drawer)
            return !car.GetRect().IntersectsWith(_temp); //return if it does not intersect with temp, thus it is out of bounds
        }

        //add a ToggleSpeed() method - it uses the MaxSpeed bool to toggle between half speed and full speed
        public void ToggleSpeed()
        {
            MaxSpeed = !MaxSpeed; 
        }

        //todo Enhancement methods
        //Score is based solely on absolute speed of the Car, so cars with default higher speeds have higher score
        //Penalty is based solely on speed as well, so score value just turned negative 
        public abstract int GetSafeScore(); //can be overriden for each vehicle to get appropriate score
        public abstract int GetHitScore(); //can be overriden for each vehicle to get appropriate penalty 
    }



    //create a new abstract class HorizontalCar derived from Car
    abstract class HorizontalCar : Car
    {
        //CTOR - arguments of speed, width, and height
        public HorizontalCar(int speed, int width, int height) : base(speed, width, height)
        {
            if (speed < 0) //if speed is negative we are going left
            {
                _xcoord = Canvas.ScaledWidth; //set the X coordinate to the right side of the screen
                int _random = Rand.Next(Car._left.Count); //create random index from left collection
                _ycoord = _left[_random]; //set Y to a random lane from the left collection 
            }

            if (speed > 0) //if speed is positive we are going right 
            {
                _xcoord = 0 - width; //set X to just off the left side 
                int _random = Rand.Next(Car._right.Count); //random index from right collection
                _ycoord = _right[_random]; //set Y to a random lane from the right collection
            }
        }

        //override VMove()
        //since going horizontally, increment our X by our speed
        protected override void VMove()
        {
            if (MaxSpeed) //if MaxSpeed is true
            {
                _xcoord += _speed; //increment x by the speed
            }

            else //MaxSpeed is not true, so going half speed
            {
                _xcoord += _speed / 2; //increment x by half of speed 
            }

        }
    }


    //derive new abstract class VerticalCar
    abstract class VerticalCar : Car
    {
        //Add CTOR - arguments of speed, width, height.
        public VerticalCar(int speed, int width, int height) : base(speed, width, height)
        {
            if (speed < 0) //if speed is negative we are going UP
            {
                _ycoord = Canvas.ScaledHeight; //set Y to bottom of screen 
                int _random = Rand.Next(Car._ups.Count); //random index from up list collection
                _xcoord = _ups[_random]; //set X to a random lane from up collection
            }

            if (speed > 0) //if speed is positive, going DOWN
            {
                _ycoord = 0 - height; //set Y to just above the top 
                int _random = Rand.Next(Car._downs.Count); //random index from down collection
                _xcoord = _downs[_random]; //set X to a random lane selection from down collection

            }
        }

        //Override VMove()
        //since moving vertically increment Y by our speed
        protected override void VMove()
        {
            if (MaxSpeed) //if MaxSpeed is true
            {
                _ycoord += _speed; //increment Y by speed
            }

            else //else if MaxSpeed is not true
            {
                _ycoord += _speed / 2; //increment Y by half speed 
            }
        }

    }

    //Derive a new concrete class called VSedan from Vertical Class
    class VSedan : VerticalCar
    {
        protected Color _carColor = RandColor.GetColor(); // car color initialized to a random color 

        //CTOR - argument of speed, width (default is 40), and height (default is 70)
        public VSedan(int speed, int width = 40, int height = 70) : base(speed, width, height)
        {

        }

        //Override GetRect()
        //Based on your size, generate your bounding Rectangle and return it
        public override Rectangle GetRect() 
        {
            return new Rectangle(_xcoord, _ycoord, _width, _height); //return new Rectangle 
        }

        //Override VShowCar()
        //Using your GetRec() and other decorations draw the sedan 
        protected override void VShowCar()
        {
            Rectangle _rec = GetRect(); //variable to hold Rectangle from GetRect()
            Canvas.AddRectangle(_rec, _carColor); //add the base shape with color of the car to the canvas

            if (_speed > 0) //if going Down
            {
                Canvas.AddRectangle(_rec.X, _rec.Y, 4, 4, Color.Red, 1, Color.Black); //brake light left
                Canvas.AddRectangle(_rec.X + _width - 4, _rec.Y, 4, 4, Color.Red, 1, Color.Black); //brake light right
                Canvas.AddRectangle(_rec.X, _rec.Y + _height - 4, 4, 4, Color.Yellow, 1, Color.Black); //add first headlight 
                Canvas.AddRectangle(_rec.X + _width - 4, _rec.Y + _height - 4, 4, 4, Color.Yellow, 1, Color.Black); //add second headlight
                Canvas.AddRectangle(_rec.X + 2, _rec.Y + _height - 40, _width - 4, 12, Color.LightGray); //windshield
            }

            else //going up
            {
                Canvas.AddRectangle(_rec.X, _rec.Y, 4, 4, Color.Yellow, 1, Color.Black); //first headlight
                Canvas.AddRectangle(_rec.X + _width - 4, _rec.Y, 4, 4, Color.Yellow, 1, Color.Black); //second headlight
                Canvas.AddRectangle(_rec.X, _rec.Y + _height - 4, 4, 4, Color.Red, 1, Color.Black); //first brake light
                Canvas.AddRectangle(_rec.X + _width - 4, _rec.Y + _height - 4, 4, 4, Color.Red, 1, Color.Black); //second brake light
                Canvas.AddRectangle(_rec.X + 2, _rec.Y + 30, _width - 4, 12, Color.LightGray); //windshield
            }
        }

        
        //override GetSafeScore()
        public override int GetSafeScore()
        {
            return Math.Abs(_speed); //score value is based on speed of car travelling
        }

        //override GetHitScore()
        public override int GetHitScore()
        {
            return GetSafeScore() * -1; //penalty is also based on what score would have been from the speed but negative
        }

    }

    //create new interface called IAnimateable
    interface IAnimateable
    {
        void Animate();  //create abstract method called Animate(), returns nothing and accepts nothing
    }

    //create a new concrete class called HAmbulance, derived from HorizontalCar and supporting IAnimateable interface
    class HAmbulance : HorizontalCar, IAnimateable
    {
        Color _siren = Color.Blue; //the ambulance's siren

        //CTOR - use 7 for speed, 90 for width, and 40 for height 
        public HAmbulance(int speed = 7, int width = 90, int height = 40) : base(speed, width, height)
        {

        }

        //override GetRect()
        public override Rectangle GetRect()
        {
            return new Rectangle(_xcoord, _ycoord, _width, _height); //create new rectangle for ambulance 
        }

        //override VShowCar()
        protected override void VShowCar()
        {
            Rectangle _rectangle = GetRect(); //variable to hold Rectangle from GetRect()
            Canvas.AddRectangle(_rectangle, Color.White); //add the ambulance rectangle in white to the canvas 
                  
            if (_speed > 0) //for ambulances going right
            {
                Canvas.AddRectangle(_rectangle.X + _width / 4, _rectangle.Y + _height / 2 - 8, 4, 16, Color.Red); //add shppe for ambulance symbol
                Canvas.AddRectangle(_rectangle.X + _width / 4 - 6, _rectangle.Y + _height / 2 - 2, 16, 4, Color.Red); //add shape for ambulance symbol
                Canvas.AddRectangle(_rectangle.X + _width - 15, _rectangle.Y, 5, _height, _siren); //add siren 
                Canvas.AddRectangle(_rectangle.X + _width - 2, _rectangle.Y, 2, 4, Color.Yellow, 1, Color.Black); //add headlight (top one)
                Canvas.AddRectangle(_rectangle.X + _width - 2, _rectangle.Y + _height - 4, 2, 4, Color.Yellow, 1, Color.Black); //add second headlight
                Canvas.AddRectangle(_rectangle.X, _rectangle.Y, 2, 4, Color.Red, 1, Color.Black); //add brake light
                Canvas.AddRectangle(_rectangle.X, _rectangle.Y + _height - 4, 2, 4, Color.Red, 1, Color.Black); //add brake light
            }

            else //ambulances going left 
            {
                Canvas.AddRectangle(_rectangle.X + _width * 3/4, _rectangle.Y + _height / 2 - 8, 4, 16, Color.Red); //add for ambulance symbol
                Canvas.AddRectangle(_rectangle.X + _width * 3 / 4 - 6, _rectangle.Y + _height / 2 - 2, 16, 4, Color.Red); //add for ambulance symbol
                Canvas.AddRectangle(_rectangle.X + 15, _rectangle.Y, 5, _height, _siren); //add siren
                Canvas.AddRectangle(_rectangle.X, _rectangle.Y, 2, 4, Color.Yellow, 1, Color.Black); //add headlight
                Canvas.AddRectangle(_rectangle.X, _rectangle.Y + _height - 4, 2, 4, Color.Yellow, 1, Color.Black); //add headlight
                Canvas.AddRectangle(_rectangle.X + _width - 2, _rectangle.Y, 2, 4, Color.Red, 1, Color.Black); //add brake light
                Canvas.AddRectangle(_rectangle.X + _width - 2, _rectangle.Y + _height - 4, 2, 4, Color.Red, 1, Color.Black); //add brake light
            }

        }

        //Animate() method to support IAnimateable
        //used for Ambulance class to toggle the siren color
        public void Animate()
        {
            if (_siren == Color.Blue) //if the siren is blue
            {
                _siren = Color.Red; //make it red
            }

            else //else if it is red
            {
                _siren = Color.Blue; //make it blue 
            }
        }

        //public override GetSafeScore()
        public override int GetSafeScore()
        {
            return Math.Abs(_speed); //score value is based solely on speed of car travelling
        }

        //public override GetHitScore()
        public override int GetHitScore()
        {
            return GetSafeScore() * -1;  //penalty is also based on what score would have been from the speed but negative
        }
    }


    //todo Enhancement Class
    //create PoliceCar class derived from HorizontalCar and also supporting IAnimateable
    class PoliceCar : HorizontalCar, IAnimateable
    {
        //create members for red and blue lights for policecar
        Color _redLight = Color.Red; 
        Color _blueLight = Color.Blue;

        //CTOR - speed of 10, width of 70, height of 45 
        public PoliceCar(int speed = 10, int width = 70, int height = 45) : base(speed, width, height)
        {

        }

        //override GetRect()
        public override Rectangle GetRect()
        {
            return new Rectangle(_xcoord, _ycoord, _width, _height); //create new Rectangle based on the size 
        }

        //override VShowCar()
        protected override void VShowCar()
        {
            Rectangle _rectangle = GetRect(); //set Rectangle variable to one created from GetRect()
            Canvas.AddRectangle(_rectangle, Color.DarkBlue); //add to canvas the policecar base shape and color is initialized to dark blue 


            if (_speed > 0) //if going right
            {
                Canvas.AddRectangle(_rectangle.X + _width - 2, _rectangle.Y, 2, 4, Color.Yellow, 1, Color.Black); //add headlight (top one)
                Canvas.AddRectangle(_rectangle.X + _width - 2, _rectangle.Y + _height - 4, 2, 4, Color.Yellow, 1, Color.Black); //add second headlight
                Canvas.AddRectangle(_rectangle.X, _rectangle.Y, 2, 4, Color.Red, 1, Color.Black); //add brake light
                Canvas.AddRectangle(_rectangle.X, _rectangle.Y + _height - 4, 2, 4, Color.Red, 1, Color.Black); //add second brake light
                Canvas.AddRectangle(_rectangle.X + _width / 2 - 5, _rectangle.Y, 4, _height / 2, _redLight); // add red siren
                Canvas.AddRectangle(_rectangle.X + _width / 2 - 5, _rectangle.Y + _height / 2, 4, _height / 2, _blueLight); //add blue siren 
                Canvas.AddRectangle(_rectangle.X + _width / 2 + 5, _rectangle.Y, 5, _height, Color.LightGray); //add windshield
            }

            else //if going left
            {
                Canvas.AddRectangle(_rectangle.X, _rectangle.Y, 2, 4, Color.Yellow, 1, Color.Black); //add headlight 
                Canvas.AddRectangle(_rectangle.X, _rectangle.Y + _height - 4, 2, 4, Color.Yellow, 1, Color.Black); //add second headlight
                Canvas.AddRectangle(_rectangle.X + _width - 2, _rectangle.Y, 2, 4, Color.Red, 1, Color.Black); //add brake light
                Canvas.AddRectangle(_rectangle.X + _width - 2, _rectangle.Y + _height - 4, 2, 4, Color.Red, 1, Color.Black); //add second brake light
                Canvas.AddRectangle(_rectangle.X + _width / 2 + 5, _rectangle.Y, 4, _height / 2, _redLight); // add red siren
                Canvas.AddRectangle(_rectangle.X + _width / 2 + 5, _rectangle.Y + _height / 2, 4, _height / 2, _blueLight); //add blue siren 
                Canvas.AddRectangle(_rectangle.X + _width / 2 - 5, _rectangle.Y, 5, _height, Color.LightGray); //add windshield
            }
        }

        //Animate() method to support IAnimateable
        //used for PoliceCar class to toggle between the redlight and bluelight
        public void Animate() 
        {
            if (_redLight == Color.Red) //if the _redLight is Red
            {
                _redLight = Color.Blue; //make it blue
                _blueLight = Color.Red; //and make the _blueLight variable set to Red Color
            }

            else //else if the _redLight is Blue
            {
                _redLight = Color.Red; //make it red
                _blueLight = Color.Blue; //make the _blueLight variable to Blue 
            }
        }

        //override GetSafeScore()
        public override int GetSafeScore()
        {
            return Math.Abs(_speed); //score is based on the absolute speed of the car 
        }

        public override int GetHitScore()
        {
            return GetSafeScore() * -1; //penalty is also based on what score would have been from the speed but negative
        }
    }

    //todo Enhancement Class
    //create  Mustang Class derived from VerticalCar
    class Mustang : VerticalCar
    {
        protected Color _mustangColor = RandColor.GetColor(); //set color to a random color

        //CTOR - the speed is 6, width is 44, and height is 70
        public Mustang(int speed = 6, int width = 44, int height = 70) : base(speed, width, height)
        { 
        }

        //override GetRect()
        public override Rectangle GetRect()
        {
            return new Rectangle(_xcoord, _ycoord, _width, _height); //get new Rectangle based on size of mustang 
        }

        //override VShowCar()
        protected override void VShowCar()
        {
            Rectangle _rec = GetRect(); //make variable hold Rectangle returned from GetRect()
            Canvas.AddRectangle(_rec, _mustangColor); //add the base mustang shape and color to canvas

            if (_speed > 0) //going Down
            {
                Canvas.AddRectangle(_rec.X, _rec.Y, 4, 4, Color.Red, 1, Color.Black); //brake light left
                Canvas.AddRectangle(_rec.X + _width - 4, _rec.Y, 4, 4, Color.Red, 1, Color.Black); //brake light right
                Canvas.AddRectangle(_rec.X, _rec.Y + _height - 4, 4, 4, Color.Yellow, 1, Color.Black); //headlight
                Canvas.AddRectangle(_rec.X + _width - 4, _rec.Y + _height - 4, 4, 4, Color.Yellow, 1, Color.Black); //headlight
                Canvas.AddRectangle((_rec.X + _width / 2) - 7, _rec.Y + _height - 22, 5, 22, Color.Black); //front left stripe
                Canvas.AddRectangle((_rec.X + _width / 2) + 4, _rec.Y + _height - 22, 5, 22, Color.Black); //front right stripe
                Canvas.AddRectangle(_rec.X + 2, _rec.Y + _height - 40, 40, 12, Color.LightGray); //windshield
            }

            else //going up 
            {
                Canvas.AddRectangle(_rec.X, _rec.Y, 4, 4, Color.Yellow, 1, Color.Black); //head light 
                Canvas.AddRectangle(_rec.X + _width - 4, _rec.Y, 4, 4, Color.Yellow, 1, Color.Black); //head light 
                Canvas.AddRectangle(_rec.X, _rec.Y + _height - 4, 4, 4, Color.Red, 1, Color.Black); //brake light
                Canvas.AddRectangle(_rec.X + _width - 4, _rec.Y + _height - 4, 4, 4, Color.Red, 1, Color.Black); //brake light
                Canvas.AddRectangle((_rec.X + _width / 2) - 7, _rec.Y, 5, 22, Color.Black); //left stripe
                Canvas.AddRectangle((_rec.X + _width / 2 + 4), _rec.Y, 5, 22, Color.Black); //right stripe
                Canvas.AddRectangle(_rec.X + 2, _rec.Y + 22, 40, 12, Color.LightGray); //windshield
            }
        }

        //override GetSafeScore()
        public override int GetSafeScore() 
        {
            return Math.Abs(_speed); //score is based on absolute speed of mustang
        }

        //override GetHitScore()
        public override int GetHitScore()
        {
            return GetSafeScore() * -1; //penalty is also based on what score would have been from the speed but negative
        }
    }
}
