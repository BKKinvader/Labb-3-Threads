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
            // Skapa threads för bilar
            Thread car1Thread = new Thread(() => RaceCar("Mercedes", Thread.CurrentThread));
            Thread car2Thread = new Thread(() => RaceCar("BMW", Thread.CurrentThread));
            Thread car3Thread = new Thread(() => RaceCar("Lambo", Thread.CurrentThread));

            // Starta race
            car1Thread.Start();
            car2Thread.Start();
            car3Thread.Start();
            
            // Avsluta inte för än alla threads blir klara 
            car1Thread.Join();
            car2Thread.Join();
            car3Thread.Join();

            Console.WriteLine($"Congrats {winner} Win!");

        }

        //Sätta normalspeed och vinnaren
        //Statisk variabel
        public static int speed = 120;
        public static string winner = "";



        
        //Tar in bilens namn och vilken thread
        public static void RaceCar(string carName, Thread thread)
        {
            int carSpeed = speed;
            Console.WriteLine($"{carName} started");

            //Timer
            DateTime startTime = DateTime.Now;
            DateTime nextCheckTime = startTime.AddSeconds(10);
            bool timer30Seconds = false;

            //10 km race
            for (int i = 0; i <= 10; i++)
            {

                Console.WriteLine($"{carName}: {i} km");

                if (DateTime.Now >= nextCheckTime)
                {
                    nextCheckTime = DateTime.Now.AddSeconds(10);
                    timer30Seconds = true;
                }

                //Besiktning
                if (timer30Seconds)
                {
                    Console.Clear();
                    Console.WriteLine($"Car Check 10 sec {carName}");
                    //Kolla om bilen har punka
                    if (HasPuncture())
                    {
                        Console.WriteLine($"!!! {carName} got a puncture! !!!");
                        //Stoppa bilen 20 sekunder
                        Thread.Sleep(20000);
                        Console.WriteLine($"<<< {carName} is back on track >>>>");
                    }


                    if (OutOfGas())
                    {
                        Console.WriteLine($"!!! {carName} ran out of gas! !!!");
                        Thread.Sleep(30000);
                        Console.WriteLine($"<<< {carName} is back on track >>>");
                    }


                    if (BirdOnWindshield())
                    {
                        Console.WriteLine($"!!! A bird landed on {carName}'s windshield! !!!");
                        Thread.Sleep(10000);
                        Console.WriteLine($"<<< {carName} is back on track >>>");
                    }


                    if (EngineFailure())
                    {
                        Console.WriteLine($"!!! {carName}'s engine has failed! !!!");
                        //-1 km / h
                        carSpeed--;
                        if (carSpeed <= 0)
                        {
                            Console.WriteLine($"--- {carName} can't move anymore!----");
                            break;
                        }
                        Console.WriteLine($"*** {carName} is now moving at {carSpeed} km/h ***");
                    }

                    timer30Seconds = false;
                }



                //Namnge vinnaren genom att sätta static variabel namn
                if (i == 10 && winner == "")
                {
                    //Sätta namn på vinnaren
                    winner = carName;
                }

                Console.WriteLine();
                Thread.Sleep(2000);
            }

            Console.WriteLine($"{carName} finished");
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






    }
}