using System;
using System.Collections.Generic;
using System.Linq;

// To execute C#, please define "static void Main" on a class
// named Solution.

namespace Application
{
    class Solution
    {
        static void Main(string[] args)
        {
            //SETUP STEP OF THE PARKING LOT
            ParkingLot parkingLot = Setup();

            string menuOption = "0";
            string vacancyType = "0";


            while (!menuOption.Equals("-1"))
            {
                PrintMenu();
                menuOption = Console.ReadLine();
                Console.Clear();

                switch (menuOption)
                {
                    case "1":
                        Console.WriteLine("Total amount of parking spots: " + parkingLot.GetParkingSpotsAmount());
                        break;
                    case "2":
                        Console.WriteLine("Total amount of free parking spots: " + parkingLot.GetFreeParkingSpotsAmount());
                        break;
                    case "3":
                        Console.WriteLine("Is parking lot completely occupied: " + parkingLot.CheckIfParkingLotHasNoVacancy());
                        break;
                    case "4":
                        Console.WriteLine("Is parking lot completely vacant: " + parkingLot.CheckIfParkingLotIsCompletelyVacant());
                        break;
                    case "5":
                        Console.WriteLine("Please inform the type of parking spot you want to check for");
                        Console.WriteLine("1 - Big parking spot");
                        Console.WriteLine("2 - Car parking spot");
                        Console.WriteLine("3 - Motorcyle parking spot");
                        vacancyType = Console.ReadLine();
                        switch (vacancyType)
                        {
                            case "1":
                                Console.WriteLine(parkingLot.IsParkingSpotsOfCertainTypesNonVacant(ParkingSpaceType.Big));
                                break;
                            case "2":
                                Console.WriteLine(parkingLot.IsParkingSpotsOfCertainTypesNonVacant(ParkingSpaceType.Car));
                                break;
                            case "3":
                                Console.WriteLine(parkingLot.IsParkingSpotsOfCertainTypesNonVacant(ParkingSpaceType.Motorcyle));
                                break;
                            default:
                                Console.WriteLine("Invalid option!");
                                break;
                        }
                        break;
                    case "6":
                        Console.WriteLine(parkingLot.checkHowManyParkingSpotsAreOccupiedByVans());
                        break;
                    case "7":
                        parkingLot.AddMotorcycle();
                        break;
                    case "8":
                        parkingLot.AddCar();
                        break;
                    case "9":
                        parkingLot.AddVan();
                        break;
                    case "-1":
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        static void PrintMenu()
        {
            Console.WriteLine("Select an option, type -1 to stop execution:");
            Console.WriteLine("1 - Inform the total number of vacancies in the parking lot");
            Console.WriteLine("2 - Inform the number of vacant parking spots in the parking lot");
            Console.WriteLine("3 - Is the parking lot full?");
            Console.WriteLine("4 - Is the parking lot empty?");
            Console.WriteLine("5 - Inform if a determined type of parking spots is completely occupied");
            Console.WriteLine("6 - Inform how parking spots are vans occupying");
            Console.WriteLine("7 - Add motorcycle");
            Console.WriteLine("8 - Add car");
            Console.WriteLine("9 - Add van");
        }

        static void InstantiateParkingSpaces(int parkingSpacesAmount, ParkingSpaceType parkingSpaceType, List<ParkingSpace> parkingSpaces)
        {
            for (int index = 0; index < parkingSpacesAmount; index++)
            {
                var parkingSpace = new ParkingSpace(parkingSpaceType, ParkingSpaceStatus.Vacant);
                parkingSpace.VehicleType = VehicleType.None;
                parkingSpaces.Add(parkingSpace);
            }
        }

        static ParkingLot Setup()
        {
            // ALTERAR QUANTIDADES DE VAGAS INICIAIS AQUI

            const int BIG_PARKING_SPOTS_AMOUNT = 0;
            const int CAR_PARKING_SPOTS_AMOUNT = 3;
            const int MOTORCYCLE_PARKING_SPOTS_AMOUNT = 0;

            List<ParkingSpace> parkingSpaces = new List<ParkingSpace> { };
            InstantiateParkingSpaces(BIG_PARKING_SPOTS_AMOUNT, ParkingSpaceType.Big, parkingSpaces);
            InstantiateParkingSpaces(CAR_PARKING_SPOTS_AMOUNT, ParkingSpaceType.Car, parkingSpaces);
            InstantiateParkingSpaces(MOTORCYCLE_PARKING_SPOTS_AMOUNT, ParkingSpaceType.Motorcyle, parkingSpaces);

            ParkingLot parkingLot = new ParkingLot(parkingSpaces);
            return parkingLot;
        }
    }
}


public class ParkingLot
{
    public List<ParkingSpace> ParkingSpaces { get; set; }

    public ParkingLot(List<ParkingSpace> parkingSpaces)
    {
        ParkingSpaces = parkingSpaces;
    }

    public int GetParkingSpotsAmount()
    {
        return ParkingSpaces.Count;
    }

    public Boolean CheckIfParkingLotHasNoVacancy()
    {
        return ParkingSpaces.TrueForAll(parkingSpace => parkingSpace.ParkingSpaceStatus.Equals(ParkingSpaceStatus.Occupied));
    }

    public Boolean CheckIfParkingLotIsCompletelyVacant()
    {
        return ParkingSpaces.TrueForAll(parkingSpace => parkingSpace.ParkingSpaceStatus.Equals(ParkingSpaceStatus.Vacant));
    }

    public int GetFreeParkingSpotsAmount()
    {
        return ParkingSpaces.FindAll(parkingSpace => parkingSpace.ParkingSpaceStatus.Equals(ParkingSpaceStatus.Vacant)).Count;
    }

    public Boolean IsParkingSpotsOfCertainTypesNonVacant(ParkingSpaceType parkingSpaceType)
    {
        var getFreeParkingSpaces = ParkingSpaces.FindAll(parkingSpace => parkingSpace.ParkingSpaceStatus.Equals(ParkingSpaceStatus.Vacant) &&
                parkingSpace.ParkingSpaceType.Equals(parkingSpaceType));

        if (getFreeParkingSpaces.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public int checkHowManyParkingSpotsAreOccupiedByVans()
    {
        return ParkingSpaces.FindAll(parkingSpace => parkingSpace.ParkingSpaceStatus.Equals(ParkingSpaceStatus.Occupied) && parkingSpace.VehicleType.Equals(VehicleType.Van)).Count;
    }

    public void AddMotorcycle()
    {
        if (CheckIfParkingLotHasNoVacancy())
        {
            Console.WriteLine("Cannot add motorcycle due to no vacant parking spot");
        }
        else
        {
            var parkingSpot = ParkingSpaces.First(parkingSpace => parkingSpace.ParkingSpaceStatus.Equals(ParkingSpaceStatus.Vacant));
            parkingSpot.ParkingSpaceStatus = ParkingSpaceStatus.Occupied;
            parkingSpot.VehicleType = VehicleType.Motorcyle;
            Console.WriteLine("Motorcycle entered the parking lot!!");
        }
    }

    public void AddCar()
    {
        if (CheckIfParkingLotHasNoVacancy() || IsParkingSpotsOfCertainTypesNonVacant(ParkingSpaceType.Car) || IsParkingSpotsOfCertainTypesNonVacant(ParkingSpaceType.Big))
        {
            Console.WriteLine("Cannot add car due to no vacant parking spot");
        }
        else
        {
            var parkingSpot = ParkingSpaces.First(parkingSpace => parkingSpace.ParkingSpaceStatus.Equals(ParkingSpaceStatus.Vacant) &&
            (parkingSpace.ParkingSpaceType.Equals(ParkingSpaceType.Car) || parkingSpace.ParkingSpaceType.Equals(ParkingSpaceType.Big)));
            parkingSpot.ParkingSpaceStatus = ParkingSpaceStatus.Occupied;
            parkingSpot.VehicleType = VehicleType.Car;

            Console.WriteLine("Car entered the parking lot!");
        }
    }

    public void AddVan()
    {
        var amountOfFreeCarParkingSpots = ParkingSpaces.FindAll(parkingSpace => parkingSpace.ParkingSpaceStatus.Equals(ParkingSpaceStatus.Vacant) && parkingSpace.ParkingSpaceType.Equals(ParkingSpaceType.Car)).Count;
        var areAllBigParkingSpacesOccupied = IsParkingSpotsOfCertainTypesNonVacant(ParkingSpaceType.Big);

        if (CheckIfParkingLotHasNoVacancy() || (amountOfFreeCarParkingSpots < 3 && areAllBigParkingSpacesOccupied))
        {
            Console.WriteLine("Cannot add van due to insuficcient amount of vacant parking spots");
        }
        else
        {
            if (!areAllBigParkingSpacesOccupied)
            {
                var bigParkingSpot = ParkingSpaces.First(parkingSpace => parkingSpace.ParkingSpaceStatus.Equals(ParkingSpaceStatus.Vacant) &&
                parkingSpace.ParkingSpaceType.Equals(ParkingSpaceType.Big));

                bigParkingSpot.ParkingSpaceStatus = ParkingSpaceStatus.Occupied;
                bigParkingSpot.VehicleType = VehicleType.Van;

                Console.WriteLine("Van entered the parking lot!");
            }
            else
            {
                var carParkingSpots = ParkingSpaces.FindAll(parkingSpace => parkingSpace.ParkingSpaceStatus.Equals(ParkingSpaceStatus.Vacant) && parkingSpace.ParkingSpaceType.Equals(ParkingSpaceType.Car));
                for (int i = 0; i < 3; i++)
                {
                    carParkingSpots[i].ParkingSpaceStatus = ParkingSpaceStatus.Occupied;
                    carParkingSpots[i].VehicleType = VehicleType.Van;
                }

                Console.WriteLine("Van entered the parking lot!");
            }
        }
    }

}

public class ParkingSpace
{
    public ParkingSpaceType ParkingSpaceType { get; set; }

    public ParkingSpaceStatus ParkingSpaceStatus { get; set; }

    public VehicleType VehicleType { get; set; }

    public ParkingSpace(ParkingSpaceType parkingSpaceType, ParkingSpaceStatus parkingSpaceStatus)
    {
        ParkingSpaceType = parkingSpaceType;
        ParkingSpaceStatus = parkingSpaceStatus;
    }

}

public enum ParkingSpaceType
{
    Big,
    Car,
    Motorcyle
}

public enum ParkingSpaceStatus
{
    Vacant,
    Occupied
}

public enum VehicleType
{
    None,
    Van,
    Car,
    Motorcyle
}