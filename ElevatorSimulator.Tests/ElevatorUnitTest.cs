
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElevatorSimulator.Tests
{
	[TestClass]
	public class ElevatorUnitTests
	{
		[TestMethod]
		public void MoveUpMoveDownTest()
		{
			// arrange
			const int numberFloors = 10;
			const int initialFloor = 1;
			Elevator elevator = new Elevator(numberFloors, initialFloor);
			List<FloorCommand> floorCommandList = new List<FloorCommand>
			{
				new FloorCommand(2, ButtonDirection.Down),
				new FloorCommand(1, ButtonDirection.Up)
			};

			// act
			elevator.RunCommands(floorCommandList);

			// assert
			const int expectedFinalFloor = initialFloor;
			int finalFloor = elevator.FinalFloor;
			Assert.AreEqual(expectedFinalFloor, finalFloor);
		}

		[TestMethod]
		public void MoveDownMoveUpTest()
		{
			// arrange
			const int numberFloors = 10;
			const int initialFloor = 2;
			Elevator elevator = new Elevator(numberFloors, initialFloor);
			List<FloorCommand> floorCommandList = new List<FloorCommand>
			{
				new FloorCommand(1, ButtonDirection.Up),
				new FloorCommand(2, ButtonDirection.Down)
			};

			// act
			elevator.RunCommands(floorCommandList);

			// assert
			const int expectedFinalFloor = initialFloor;
			int finalFloor = elevator.FinalFloor;
			Assert.AreEqual(expectedFinalFloor, finalFloor);
		}

		[TestMethod]
		public void ComplexSequenceTest()
		{
			// arrange
			const int numberFloors = 10;
			const int initialFloor = 1;
			Elevator elevator = new Elevator(numberFloors, initialFloor);
			List<FloorCommand> floorCommandList = new List<FloorCommand>()
			{
				new FloorCommand(9, ButtonDirection.Down),
				new FloorCommand(6, ButtonDirection.Up),
				new FloorCommand(3, ButtonDirection.Up),
				new FloorCommand(7, ButtonDirection.Up),
				new FloorCommand(5, ButtonDirection.Down),
			};

			// act
			elevator.RunCommands(floorCommandList);

			// assert
			const int expectedFinalFloor = 5;
			int finalFloor = elevator.FinalFloor;
			Assert.AreEqual(expectedFinalFloor, finalFloor);
		}


		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullCommandListThrowsExceptionTest()
		{
			// arrange
			const int numberFloors = 10;
			const int initialFloor = 1;
			Elevator elevator = new Elevator(numberFloors, initialFloor);
			const List<FloorCommand> floorCommandList = null;

			// act
			elevator.RunCommands(floorCommandList);
		}

		[TestMethod]
		public void EmptyCommandListDoesNotChangeElevatorStateTest()
		{
			// arrange
			const int numberFloors = 10;
			const int initialFloor = 1;
			Elevator elevator = new Elevator(numberFloors, initialFloor);
			List<FloorCommand> floorCommandList = new List<FloorCommand>();

			// act
			elevator.RunCommands(floorCommandList);

			// assert
			const int expectedFinalFloor = initialFloor;
			int finalFloor = elevator.FinalFloor;
			Assert.AreEqual(expectedFinalFloor, finalFloor);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void MoveDownOnBottomFloorThrowsExceptionTest()
		{
			// arrange
			const int numberFloors = 10;
			const int initialFloor = 1;
			Elevator elevator = new Elevator(numberFloors, initialFloor);
			List<FloorCommand> floorCommandList = new List<FloorCommand>
			{
				new FloorCommand(1, ButtonDirection.Down)
			};

			// act
			elevator.RunCommands(floorCommandList);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void MoveUpOnTopFloorThrowsExceptionTest()
		{
			// arrange
			const int numberFloors = 10;
			const int initialFloor = 10;
			Elevator elevator = new Elevator(numberFloors, initialFloor);
			List<FloorCommand> floorCommandList = new List<FloorCommand>
			{
				new FloorCommand(10, ButtonDirection.Up)
			};

			// act
			elevator.RunCommands(floorCommandList);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TwoAdjacentCommandsWithSameFloorThrowsExceptionTest()
		{
			// arrange
			const int numberFloors = 10;
			const int initialFloor = 1;
			Elevator elevator = new Elevator(numberFloors, initialFloor);
			List<FloorCommand> floorCommandList = new List<FloorCommand>
			{
				new FloorCommand(3, ButtonDirection.Up),
				new FloorCommand(3, ButtonDirection.Up)
			};

			// act
			elevator.RunCommands(floorCommandList);
		}
	}
}
