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

            DrawSmoothClock(hour12, now.Minute, now.Second);

            Console.WriteLine($"\nCurrent Time: {hour12:D2}:{now.Minute:D2}:{now.Second:D2} {currentAmPm}");
            Console.WriteLine($"Alarm set for {alarmHour:D2}:{alarmMinute:D2} {amPm}");

            // Alarm check
            if (hour12 == alarmHour && now.Minute == alarmMinute && currentAmPm == amPm)
            {
                Console.WriteLine("\n*** ALARM! ***");
                Console.Beep();
                Thread.Sleep(60000); // wait a minute to avoid repeating
            }

            Thread.Sleep(1000);
        }
    }

    static void DrawSmoothClock(int hour, int minute, int second)
    {
        int size = 12;
        string[,] grid = new string[size, size];

        // Fill grid with spaces
        for (int r = 0; r < size; r++)
            for (int c = 0; c < size; c++)
                grid[r, c] = " ";

        int centerX = size / 2;
        int centerY = size / 2;

        // Place clock numbers (strings)
        grid[0, centerX] = "12";
        grid[1, size - 2] = "1";
        grid[3, size - 1] = "2";
        grid[centerY, size - 1] = "3";
        grid[size - 3, size - 1] = "4";
        grid[size - 2, size - 2] = "5";
        grid[size - 1, centerX] = "6";
        grid[size - 2, 1] = "7";
        grid[size - 3, 0] = "8";
        grid[centerY, 0] = "9";
        grid[3, 0] = "10";
        grid[1, 1] = "11";

        // Convert hour, minute, second to angles
        double hourAngle = ((hour % 12) + minute / 60.0 + second / 3600.0) * 30; // degrees
        double minuteAngle = (minute + second / 60.0) * 6;
        double secondAngle = second * 6;

        // Draw hands
        PlotHand(grid, centerX, centerY, hourAngle, 4, 'H');    // hour hand
        PlotHand(grid, centerX, centerY, minuteAngle, 5, 'M');  // minute hand
        PlotHand(grid, centerX, centerY, secondAngle, 5, 'S');  // second hand

        // Print grid
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                Console.Write(grid[r, c] + " ");
            }
            Console.WriteLine();
        }
    }

    static void PlotHand(string[,] grid, int cx, int cy, double angleDeg, int length, char symbol)
    {
        double angleRad = (Math.PI / 180) * (angleDeg - 90); // Adjust so 0 degrees is up
        for (int i = 1; i <= length; i++)
        {
            int x = cx + (int)Math.Round(i * Math.Cos(angleRad));
            int y = cy + (int)Math.Round(i * Math.Sin(angleRad));

            // Stay inside the grid
            if (x >= 0 && x < grid.GetLength(1) && y >= 0 && y < grid.GetLength(0))
                grid[y, x] = symbol.ToString();
        }
    }
}
