using System;
using System.Threading;

class SmoothAnalogClock
{
    static void Main()
    {
        Console.WriteLine("=== 12x12 Smooth Analog Alarm Clock ===");

        // Get alarm input
        Console.Write("Set alarm hour (1-12): ");
        int alarmHour = int.Parse(Console.ReadLine());

        Console.Write("Set alarm minute (0-59): ");
        int alarmMinute = int.Parse(Console.ReadLine());

        Console.Write("Set AM or PM: ");
        string amPm = Console.ReadLine().Trim().ToUpper();
        if (amPm != "AM" && amPm != "PM") amPm = "AM";

        while (true)
        {
            Console.Clear();
            DateTime now = DateTime.Now;

            string currentAmPm = now.Hour >= 12 ? "PM" : "AM";
            int hour12 = now.Hour % 12;
            if (hour12 == 0) hour12 = 12;

            // Check if alarm is active
            bool alarmActive = (hour12 == alarmHour && now.Minute == alarmMinute && currentAmPm == amPm);

            DrawSmoothClock(hour12, now.Minute, now.Second, alarmActive);

            Console.WriteLine($"\nCurrent Time: {hour12:D2}:{now.Minute:D2}:{now.Second:D2} {currentAmPm}");
            Console.WriteLine($"Alarm set for {alarmHour:D2}:{alarmMinute:D2} {amPm}");

            // Beep and flash effect
            if (alarmActive)
            {
                Console.Beep();
                Thread.Sleep(500); // short pause to emphasize flashing
            }
            else
            {
                Thread.Sleep(1000);
            }
        }
    }

    static void DrawSmoothClock(int hour, int minute, int second, bool flashHands)
    {
        int size = 12;
        string[,] grid = new string[size, size];

        // Fill grid with empty spaces
        for (int r = 0; r < size; r++)
            for (int c = 0; c < size; c++)
                grid[r, c] = "  ";

        int centerX = size / 2;
        int centerY = size / 2;

        // Place numbers
        string[] numbers = { "12", "1 ", "2 ", "3 ", "4 ", "5 ", "6 ", "7 ", "8 ", "9 ", "10", "11" };
        (int r, int c)[] positions = {
            (0, centerX), (1, size - 2), (3, size - 1), (centerY, size - 1),
            (size - 3, size - 1), (size - 2, size - 2), (size - 1, centerX),
            (size - 2, 1), (size - 3, 0), (centerY, 0), (3, 0), (1, 1)
        };
        for (int i = 0; i < numbers.Length; i++)
            grid[positions[i].r, positions[i].c] = numbers[i];

        // Convert hour, minute, second to angles
        double hourAngle = ((hour % 12) + minute / 60.0 + second / 3600.0) * 30;
        double minuteAngle = (minute + second / 60.0) * 6;
        double secondAngle = second * 6;

        // Draw hands in the grid
        string[,] handGrid = (string[,])grid.Clone();
        PlotHand(handGrid, centerX, centerY, hourAngle, 4, 'H');
        PlotHand(handGrid, centerX, centerY, minuteAngle, 5, 'M');
        PlotHand(handGrid, centerX, centerY, secondAngle, 5, 'S');

        // Print the grid with colored hands if alarm is active
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                string cell = handGrid[r, c];
                if (flashHands && cell.Trim() == "H")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("H ");
                    Console.ResetColor();
                }
                else if (flashHands && cell.Trim() == "M")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("M ");
                    Console.ResetColor();
                }
                else if (flashHands && cell.Trim() == "S")
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("S ");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(cell);
                }
            }
            Console.WriteLine();
        }
    }

    static void PlotHand(string[,] grid, int cx, int cy, double angleDeg, int length, char symbol)
    {
        double angleRad = (Math.PI / 180) * (angleDeg - 90);
        for (int i = 1; i <= length; i++)
        {
            int x = cx + (int)Math.Round(i * Math.Cos(angleRad));
            int y = cy + (int)Math.Round(i * Math.Sin(angleRad));
            if (x >= 0 && x < grid.GetLength(1) && y >= 0 && y < grid.GetLength(0))
            {
                if (grid[y, x].Trim() == "")
                    grid[y, x] = symbol + " ";
            }
        }
    }
}
