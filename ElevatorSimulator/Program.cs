
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;

// The ElevatorSimulator models an elevator as a finite state machine accepting a sequence of floor commands.

// This model has the restriction that it only accepts floor commands and not elevator car commands.

// The elevator simulator algorithm works as follows: The first command determines the next floor stop and elevator direction.
// The elevator stops at any intervening floors with a floor command having the same direction as the elevator.
// Once a command has been completed (and the elevator has stopped at the required floor) the command is removed from the floor command list.
// Commands are executed until the floor command list is empty.

namespace ElevatorSimulator
{
	class Program
	{
		static void Main()
		{
			const int numberFloors = 10;
			const int initialFloor = 1;
			Elevator elevator = new Elevator(numberFloors, initialFloor);
			List<FloorCommand> floorCommandList = CreateElevatorCommandList();

			Console.WriteLine("Elevator simulator\n");
			Console.WriteLine("NumberFloors = {0}\nInitialFloor = {1}\n", elevator.NumberFloors, elevator.InitialFloor);

			elevator.RunCommands(floorCommandList);
			elevator.DisplayFloorStops();

			Console.ReadKey();
		}

		static List<FloorCommand> CreateElevatorCommandList()
		{
			List<FloorCommand> floorCommands = new List<FloorCommand>
			{
				new FloorCommand(9, ButtonDirection.Down),
				new FloorCommand(6, ButtonDirection.Up),
				new FloorCommand(3, ButtonDirection.Up),
				new FloorCommand(7, ButtonDirection.Up),
				new FloorCommand(5, ButtonDirection.Down),
			};

			return floorCommands;
		}
	}

	////////////////////////////////////////////////////////////////////////////////

	public enum ElevatorDirection
	{
		Up,
		Down
	}

	public enum ButtonDirection
	{
		Up,
		Down
	}

	////////////////////////////////////////////////////////////////////////////////

	public class Elevator
	{
		#region Constructors and Properties

		private readonly List<int> _floorsVisited = new List<int>();

		public Elevator(int numberFloors, int initialFloor)
		{
			NumberFloors = numberFloors;
			InitialFloor = initialFloor;
			FinalFloor = initialFloor;
		}

		public int NumberFloors { get; private set; }
		public int InitialFloor { get; private set; }
		public int FinalFloor { get; private set; }

		#endregion

		#region Public Methods

		public void RunCommands(List<FloorCommand> floorCommandList)
		{
			// command list must be non-null
			if (floorCommandList == null)
			{
				throw new ArgumentNullException("floorCommandList", "Floor command list cannot be null");
			}

			int initialFloor = InitialFloor;
			_floorsVisited.Add(initialFloor);

			// exit if command list is empty
			if (floorCommandList.Count == 0)
			{
				FinalFloor = initialFloor;
				return;
			}

			do
			{
				// get the first command from the floor command list 
				FloorCommand floorCommand = floorCommandList.First();
				// remove first command from floor command list 
				floorCommandList.Remove(floorCommand);

				int targetFloor = floorCommand.Floor;
				ButtonDirection buttonDirection = floorCommand.ButtonDirection;
				Console.WriteLine("Floor command: \n\tfloor = {0}, \n\tbuttonDirection = {1}", targetFloor, buttonDirection);

				// check if command is invalid
				bool invalidAdjacentCommands = targetFloor == initialFloor;
				bool invalidCommandAtBottomFloor = targetFloor == NumberFloors && buttonDirection == ButtonDirection.Down;
				bool invalidCommandAtTopFloor = targetFloor == NumberFloors && buttonDirection == ButtonDirection.Up;

				if (invalidCommandAtBottomFloor || invalidCommandAtTopFloor || invalidAdjacentCommands)
				{
					throw new InvalidOperationException();
				}

				// set elevator direction
				ElevatorDirection elevatorDirection = (targetFloor > initialFloor) ? ElevatorDirection.Up : ElevatorDirection.Down;
				ButtonDirection interveningButtonDirection = (elevatorDirection == ElevatorDirection.Up) ? ButtonDirection.Up : ButtonDirection.Down;

				// An intervening floor stop must be strictly between the initial and target floors and have the same direction as the elevator
				List<FloorCommand> interveningFloorCommands = floorCommandList.Where(command => (command.Floor > initialFloor && command.Floor < targetFloor) 
																								 && (command.ButtonDirection == interveningButtonDirection))
																			  .OrderBy(command => command.Floor).ToList();

				foreach (FloorCommand command in interveningFloorCommands)
				{
					// add floor to visted floors list
					_floorsVisited.Add(command.Floor);
					floorCommandList.Remove(command);
				}

				// set elevator state (current floor) to new target floor
				initialFloor = targetFloor;
				FinalFloor = targetFloor;
				_floorsVisited.Add(targetFloor);

			} while (floorCommandList.Count > 0);
		}

		public void DisplayFloorStops()
		{
			Console.WriteLine("\nFloor stops:");

			foreach (int floor in _floorsVisited)
			{
				Console.WriteLine(floor);
			}
		}

		#endregion
	}

	////////////////////////////////////////////////////////////////////////////////

	public class FloorCommand
	{
		public FloorCommand(int floor, ButtonDirection buttonDirection)
		{
			Floor = floor;
			ButtonDirection = buttonDirection;
		}

		public int Floor { get; set; }
		public ButtonDirection ButtonDirection { set; get; }
	}
}
