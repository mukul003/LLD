using System;
using System.Collections.Generic;
using System.Linq;

public class TimeScheduler
{
    public static DateTime GetNextScheduledTime(DateTime inputDateTime)
    {
        // Define the daily scheduled times.
        // We'll use a list of TimeSpan objects for easier comparison.
        List<TimeSpan> scheduledTimes = new List<TimeSpan>
        {
            new TimeSpan(23, 30, 0), // 11:30 PM
            new TimeSpan(3, 30, 0),  // 3:30 AM
            new TimeSpan(6, 30, 0)   // 6:30 AM
        };

        // Sort the scheduled times to ensure correct order for finding the "next" time.
        // This is crucial because 3:30 AM and 6:30 AM are "next" after 11:30 PM.
        scheduledTimes.Sort();

        // Extract the time part of the input DateTime.
        TimeSpan inputTimeOfDay = inputDateTime.TimeOfDay;

        // Find the next scheduled time on the *same day* if available.
        foreach (TimeSpan scheduledTime in scheduledTimes)
        {
            if (scheduledTime > inputTimeOfDay)
            {
                return new DateTime(inputDateTime.Year, inputDateTime.Month, inputDateTime.Day,
                                    scheduledTime.Hours, scheduledTime.Minutes, scheduledTime.Seconds);
            }
        }

        // If no scheduled time is found on the same day (meaning input time is after all today's schedules),
        // then the next scheduled time will be the *first* scheduled time on the *next day*.
        DateTime nextDay = inputDateTime.AddDays(1);
        TimeSpan firstScheduledTimeOfNextDay = scheduledTimes.First();

        return new DateTime(nextDay.Year, nextDay.Month, nextDay.Day,
                            firstScheduledTimeOfNextDay.Hours, firstScheduledTimeOfNextDay.Minutes, firstScheduledTimeOfNextDay.Seconds);
    }

    public static void Main(string[] args)
    {
        // --- Test Cases ---

        // Test Case 1: Input time is 1st July 2025 6:30 AM, expecting 1st July 2025 11:30 PM
        DateTime input1 = new DateTime(2025, 7, 1, 6, 30, 0);
        DateTime nextTime1 = GetNextScheduledTime(input1);
        Console.WriteLine($"Input: {input1:MMMM dd, yyyy hh:mm tt} -> Next Scheduled Time: {nextTime1:MMMM dd, yyyy hh:mm tt}");
        Console.WriteLine($"Expected: July 01, 2025 11:30 PM\n");

        // Test Case 2: Input time is 1st July 2025 11:30 PM, expecting 2nd July 2025 3:30 AM
        DateTime input2 = new DateTime(2025, 7, 1, 23, 30, 0);
        DateTime nextTime2 = GetNextScheduledTime(input2);
        Console.WriteLine($"Input: {input2:MMMM dd, yyyy hh:mm tt} -> Next Scheduled Time: {nextTime2:MMMM dd, yyyy hh:mm tt}");
        Console.WriteLine($"Expected: July 02, 2025 03:30 AM\n");

        // Test Case 3: Input time is 2nd July 2025 3:30 AM, expecting 2nd July 2025 6:30 AM
        DateTime input3 = new DateTime(2025, 7, 2, 3, 30, 0);
        DateTime nextTime3 = GetNextScheduledTime(input3);
        Console.WriteLine($"Input: {input3:MMMM dd, yyyy hh:mm tt} -> Next Scheduled Time: {nextTime3:MMMM dd, yyyy hh:mm tt}");
        Console.WriteLine($"Expected: July 02, 2025 06:30 AM\n");

        // Test Case 4: Input time is 2nd July 2025 10:00 PM (after 6:30 AM but before 11:30 PM), expecting 2nd July 2025 11:30 PM
        DateTime input4 = new DateTime(2025, 7, 2, 22, 0, 0);
        DateTime nextTime4 = GetNextScheduledTime(input4);
        Console.WriteLine($"Input: {input4:MMMM dd, yyyy hh:mm tt} -> Next Scheduled Time: {nextTime4:MMMM dd, yyyy hh:mm tt}");
        Console.WriteLine($"Expected: July 02, 2025 11:30 PM\n");

        // Test Case 5: Input time is 2nd July 2025 7:00 AM (after 6:30 AM but before 11:30 PM), expecting 2nd July 2025 11:30 PM
        DateTime input5 = new DateTime(2025, 7, 2, 7, 0, 0);
        DateTime nextTime5 = GetNextScheduledTime(input5);
        Console.WriteLine($"Input: {input5:MMMM dd, yyyy hh:mm tt} -> Next Scheduled Time: {nextTime5:MMMM dd, yyyy hh:mm tt}");
        Console.WriteLine($"Expected: July 02, 2025 11:30 PM\n");

        // Test Case 6: Input time is 2nd July 2025 02:00 AM (before 3:30 AM), expecting 2nd July 2025 3:30 AM
        DateTime input6 = new DateTime(2025, 7, 2, 2, 0, 0);
        DateTime nextTime6 = GetNextScheduledTime(input6);
        Console.WriteLine($"Input: {input6:MMMM dd, yyyy hh:mm tt} -> Next Scheduled Time: {nextTime6:MMMM dd, yyyy hh:mm tt}");
        Console.WriteLine($"Expected: July 02, 2025 03:30 AM\n");
    }
}

/*
Explanation
This C# program provides a TimeScheduler class with a static method GetNextScheduledTime that calculates the next scheduled date and time based on your defined daily timings.
How it Works:
 * scheduledTimes List:
   * It initializes a List<TimeSpan> named scheduledTimes to store your daily timings: 11:30 PM, 3:30 AM, and 6:30 AM. TimeSpan is ideal here as it represents a duration of time, allowing us to compare just the time part of a DateTime object.
   * The times are sorted in ascending order. This is crucial because it allows us to easily find the next available time by iterating through the sorted list.
 * Extracting inputTimeOfDay:
   * inputDateTime.TimeOfDay extracts only the time component (e.g., 06:30:00 from "2025-07-01 06:30:00") from the provided inputDateTime.
 * Finding Next Time on the Same Day:
   * The program iterates through the scheduledTimes.
   * For each scheduledTime, it checks if it's greater than inputTimeOfDay. If it is, this means we've found the next scheduled time that occurs later on the same day.
   * A new DateTime object is constructed using the year, month, and day of the inputDateTime and the hours, minutes, and seconds of the found scheduledTime. This ensures the date remains the same.
 * Finding Next Time on the Next Day:
   * If the loop completes without finding any scheduledTime greater than inputTimeOfDay (meaning the inputDateTime is already past all the scheduled times for the current day), the program determines that the next scheduled time must be on the following day.
   * It calculates nextDay by adding one day to inputDateTime using inputDateTime.AddDays(1).
   * The firstScheduledTimeOfNextDay is simply the first element in our sorted scheduledTimes list (which will be 3:30 AM).
   * Finally, a new DateTime object is constructed using the nextDay's year, month, and day, combined with the hours, minutes, and seconds of firstScheduledTimeOfNextDay.
How to Use:
You can call the GetNextScheduledTime method and pass any DateTime object as an argument. The method will return the calculated next scheduled DateTime. The Main method includes several test cases to demonstrate its usage and verify its correctness against the requirements.
*/
