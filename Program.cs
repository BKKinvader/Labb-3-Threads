using System.Diagnostics;

namespace Labb_3_Threads
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartRace();
        }

        public static void StartRace()
        {
            // Skapa Bilar med Namn index och thread
            Thread car1Thread = new Thread(() => RaceCar("Mercedes", 0, Thread.CurrentThread));
            Thread car2Thread = new Thread(() => RaceCar("BMW", 1, Thread.CurrentThread));
            Thread car3Thread = new Thread(() => RaceCar("Lambo", 2, Thread.CurrentThread));


            // Starta race
            car1Thread.Start();
            car2Thread.Start();
            car3Thread.Start();
            
            // Avsluta inte för än alla threads blir klara 
            car1Thread.Join();
            car2Thread.Join();
            car3Thread.Join();
            Console.Clear();
            Console.WriteLine($"Congrats {winner} Win!");
            Console.ReadKey();

        }

        
        //Statisk variabel
        public static int speed = 120;
        public static string winner = "";
        public static int[] carPositions = new int[] { 0, 2, 4 }; // Bilens position i consolen och consoleLock
        public static object consoleLock = new object();


        public static void RaceCar(string carName, int carIndex, Thread thread)
        {
            double distance = 0; // distance in km
            string accidentMessage = "";
            int carSpeed = speed;
            Console.WriteLine($"{carName} started");
            Thread.Sleep(1000);
            Console.Clear();


            //Timer för problem
            DateTime startTime = DateTime.Now;
            DateTime nextCheckTime = startTime.AddSeconds(10);
            bool timer30Seconds = false;


           


            //10 km = 10 loop
            while (distance <= 10)
            {
                UpdateCarPosition(carName, carIndex, distance, carSpeed, accidentMessage);

                if (DateTime.Now >= nextCheckTime)
                {
                    nextCheckTime = DateTime.Now.AddSeconds(10);
                    timer30Seconds = true;
                }

                //Problem intröffar
                if (timer30Seconds)
                {
                   
                    //Kolla om bilen har punka
                    if (HasPuncture())
                    {


                        
                        accidentMessage = $" :{carName} got a puncture!";
                        UpdateCarPosition(carName, carIndex, distance, carSpeed, accidentMessage);
                        //Stoppa bilen 20 sekunder
                        Thread.Sleep(20000);
                        accidentMessage = $" : {carName} is back on track";
                        UpdateCarPosition(carName, carIndex, distance, carSpeed, accidentMessage);
                        
                    }


                    if (OutOfGas())
                    {
                       
                        accidentMessage =  $" :{carName} ran out of gas! ";
                        UpdateCarPosition(carName, carIndex, distance, carSpeed, accidentMessage);
                        Thread.Sleep(30000);
                        accidentMessage = $" :{carName} is back on track";
                        UpdateCarPosition(carName, carIndex, distance, carSpeed, accidentMessage);
                        

                    }


                    if (BirdOnWindshield())
                    {


                        // Clear the previous accident message

                        accidentMessage = $" :A bird landed on {carName}'s windshield!";
                        UpdateCarPosition(carName, carIndex, distance, carSpeed, accidentMessage);
                        Thread.Sleep(10000);
                        accidentMessage = $" :{carName} is back on track";
                        UpdateCarPosition(carName, carIndex, distance, carSpeed, accidentMessage);

                    }


                    if (EngineFailure())
                    {
                        carSpeed--;
                        accidentMessage = $" :{carName} engine has failed and now moving at {carSpeed} km/h";
                        Thread.Sleep(252);
                        UpdateCarPosition(carName, carIndex, distance, carSpeed, accidentMessage);
                        if (carSpeed <= 0)
                        {
                            Console.Write($"--- {carName} can't move anymore!----");
                            break;
                        }
                    }


                    timer30Seconds = false;
                }
                //Distans räknaren "high level matte"
                distance += carSpeed / (60.0 * 60.0 * 1000.0);

                if (distance >= 10 && winner == "")
                {
                    //Sätta namn på vinnaren
                    winner = carName;
                }
                            
            }

            DisplayFinishedMessage(carName, carIndex);
        }


        //Alla problem i procentuellt 
        public static bool HasPuncture()
        {
            // Returnera true med 10% chans
            return new Random().Next(100) < 10;
        }

        public static bool OutOfGas()
        {
            // Returnera true med 2% chans
            return new Random().Next(100) < 2;
        }

        public static bool BirdOnWindshield()
        {
            // Returnera true med  10% chans
            return new Random().Next(100) < 10;
        }

        public static bool EngineFailure()
        {
            // Returnera true med 20% chans
            return new Random().Next(100) < 20;
        }



        //Bilens position i console
        public static void UpdateCarPosition(string carName, int carIndex, double distance, int carSpeed, string accidentMessage)
        {
            lock (consoleLock)
            {
                int prevCursorTop = Console.CursorTop;
                int prevCursorLeft = Console.CursorLeft;

                // Hide the cursor
                Console.CursorVisible = false;

                Console.SetCursorPosition(0, carPositions[carIndex]);

                // Clear the entire line before displaying the new message
                Console.Write(new string(' ', Console.WindowWidth - 1));
                Console.SetCursorPosition(0, carPositions[carIndex]);

                Console.WriteLine($"{carName}: {distance:0.00} km, {carSpeed} km/h {accidentMessage}");

                // Restore the cursor position
                Console.SetCursorPosition(prevCursorLeft, prevCursorTop);

                // Show the cursor
                Console.CursorVisible = true;
            }
        }



        //Uppdatera finnish 
        public static void DisplayFinishedMessage(string carName, int carIndex)
        {
            lock (consoleLock)
            {
                int prevCursorTop = Console.CursorTop;
                int prevCursorLeft = Console.CursorLeft;               
                Console.SetCursorPosition(0, carPositions[carIndex]);              
                Console.Write(new string(' ', Console.WindowWidth - 1));
                Console.SetCursorPosition(0, carPositions[carIndex]);
                // Skriv finished meddelande utan att console.clear
                Console.WriteLine($"{carName} finished ");
                Console.SetCursorPosition(prevCursorLeft, prevCursorTop);
            }
        }



    }
}