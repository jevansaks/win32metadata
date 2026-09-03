# Header Report: calendarapis.h

## Partitions
`Intl`

## Ownership audit (producer-site-only policy)
- All 7 functions (`GetCalendarSupportedDateRange`, `GetCalendarDateFormatEx`, `ConvertSystemTimeToCalDateTime`, `UpdateCalendarDayOfWeek`, `AdjustCalendarDate`, `ConvertCalDateTimeToSystemTime`, `IsCalendarLeapYear`) operate on the plain `CALDATETIME`/`SYSTEMTIME` data structs only. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (calendar-date-struct-only API, no handle).
