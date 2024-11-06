namespace StudentPortal.Models
{
    public class ScheduleConflictChecker
    {
        public bool HasConflict(Schedule newSchedule, List<Schedule> existingSchedules)
        {
            var newScheduleDays = newSchedule.GetDaysAsSet();

            foreach (var existingSchedule in existingSchedules)
            {
                var existingDays = existingSchedule.GetDaysAsSet();

                // Check if there is any overlapping day
                if (newScheduleDays.Overlaps(existingDays))
                {
                    // Check for overlapping time if days overlap
                    if (IsTimeConflict(newSchedule, existingSchedule))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsTimeConflict(Schedule schedule1, Schedule schedule2)
        {
            // Check if the schedules are in the same room
            if (schedule1.roomnum != schedule2.roomnum)
            {
                return false; // No conflict if they're in different rooms
            }

            // Check if the schedules have any overlapping days
            if (!HasCommonDays(schedule1.days, schedule2.days))
            {
                return false; // No conflict if there are no common days
            }

            // Check if the time intervals overlap
            return schedule1.starttime < schedule2.endtime && schedule1.endtime > schedule2.starttime;
        }

        // Helper method to determine if there are any overlapping days between two schedules
        private bool HasCommonDays(string days1, string days2)
        {
            var daysSet1 = new HashSet<char>(days1);
            var daysSet2 = new HashSet<char>(days2);

            daysSet1.IntersectWith(daysSet2);
            return daysSet1.Count > 0; // Returns true if there's any common day
        }
    }
}
