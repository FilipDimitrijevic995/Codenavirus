using System;
using System.Collections.Generic;
using System.Linq;

namespace Codenavirus
{
    class Program
    {
        static void Main(string[] args)
        {
            string[,] world = new string[3, 3] { { "#", "#", "#" }, { "#", "#", "#" }, { "#", "#", "#" }, };
            int[] firstInfected = { 1, 1 };
            int[] result = CodenavirusEpidemic(world, firstInfected);

            Console.WriteLine();
            Console.Write("Result: [");
            for (int i = 0; i < result.Length; i++)
            {
                if (i < result.Length - 1)
                    Console.Write(result[i] + ",");
                else
                    Console.Write(result[i] + "]");
            }

            Console.ReadKey();
        }

        public static int[] CodenavirusEpidemic(string[,] world, int[] firstInfected)
        {
            bool spread = true;

            //this list saves all the infections in order to track when the person is healed
            List<InfectedLog> log = new List<InfectedLog>
            {
                new InfectedLog(firstInfected, 1)
            };

            //just printing the first day and converting # to H
            Console.WriteLine("Day 0: ");
            for (int row = 0; row < world.GetLength(0); row++)
            {
                Console.WriteLine();
                for (int column = 0; column < world.GetLength(1); column++)
                {
                    if (world[row, column].Equals("#"))
                    {
                        world[row, column] = "H";
                    }
                    Console.Write(world[row, column] + " ");
                }
                Console.WriteLine();
            }

            //changing the first infected from H to I
            world[firstInfected[0], firstInfected[1]] = "I";

            //print day 1 matrix with first infected
            Console.WriteLine();
            Console.WriteLine("Day 1:");
            for (int row = 0; row < world.GetLength(0); row++)
            {
                Console.WriteLine();
                for (int column = 0; column < world.GetLength(1); column++)
                {
                    Console.Write(world[row, column] + " ");
                }
                Console.WriteLine();
            }

            int day = 2;
            while (spread)
            {
                Console.WriteLine();
                Console.WriteLine("Day " + day + ":");
                bool infected = false;
                int[] lastInfected = log[log.Count() - 1].Place;
                int[] newInfection = new int[2];

                //checking if someone person got healed
                for (int i = 0; i < log.Count(); i++)
                {
                    //3 days have passed, set to R
                    if ((log[i].Day + 3) == day)
                    {
                        world[log[i].Place[0], log[i].Place[1]] = "R";
                    }
                }
                try
                {
                    if (world[lastInfected[0], lastInfected[1] + 1].Equals("H"))
                    {
                        world[lastInfected[0], lastInfected[1] + 1] = "I";
                        newInfection[0] = lastInfected[0];
                        newInfection[1] = lastInfected[1] + 1;
                        log.Add(new InfectedLog(newInfection, day));
                        infected = true;
                    }
                    else
                    {
                        //exception thrown just to get to catch
                        throw new SystemException();
                    }
                }
                catch (Exception right)
                {
                    try
                    {
                        if (world[lastInfected[0] - 1, lastInfected[1]].Equals("H") && !infected)
                        {
                            world[lastInfected[0] - 1, lastInfected[1]] = "I";
                            newInfection[0] = lastInfected[0] - 1;
                            newInfection[1] = lastInfected[1];
                            log.Add(new InfectedLog(newInfection, day));
                            infected = true;
                        }
                        else
                        {
                            //exception thrown just to get to catch
                            throw new SystemException();
                        }
                    }
                    catch (Exception top)
                    {
                        try
                        {
                            if (world[lastInfected[0], lastInfected[1] - 1].Equals("H") && !infected)
                            {
                                world[lastInfected[0], lastInfected[1] - 1] = "I";
                                newInfection[0] = lastInfected[0];
                                newInfection[1] = lastInfected[1] - 1;
                                log.Add(new InfectedLog(newInfection, day));
                                infected = true;
                            }
                            else
                            {
                                //exception thrown just to get to catch
                                throw new SystemException();
                            }
                        }
                        catch (Exception left)
                        {
                            try
                            {
                                if (world[lastInfected[0] + 1, lastInfected[1]].Equals("H") && !infected)
                                {
                                    world[lastInfected[0] + 1, lastInfected[1]] = "I";
                                    newInfection[0] = lastInfected[0] + 1;
                                    newInfection[1] = lastInfected[1];
                                    log.Add(new InfectedLog(newInfection, day));
                                    infected = true;
                                }
                            }
                            catch (Exception bottom) { }
                        }
                    }
                }

                //duration of the epidemic
                if (infected == true)
                    day++;
                else
                    spread = false;


                //print changed matrix with newly infected person
                for (int row = 0; row < world.GetLength(0); row++)
                {
                    Console.WriteLine();
                    for (int column = 0; column < world.GetLength(1); column++)
                    {
                        Console.Write(world[row, column] + " ");
                    }
                    Console.WriteLine();
                }
            }

            //count the result
            int numberOfInfected = 0;
            int numberOfRecovered = 0;
            int numberOfUninfected = 0;

            for (int row = 0; row < world.GetLength(0); row++)
            {
                for (int column = 0; column < world.GetLength(1); column++)
                {
                    if (world[row, column].Equals("H"))
                        numberOfUninfected++;
                    else if (world[row, column].Equals("I"))
                        numberOfInfected++;
                    else if (world[row, column].Equals("R"))
                        numberOfRecovered++;
                }
            }

            int[] result = { day, numberOfInfected, numberOfRecovered, numberOfUninfected };
            return result;
        }
    }
}
