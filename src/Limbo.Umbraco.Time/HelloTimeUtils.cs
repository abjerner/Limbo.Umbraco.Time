﻿using System;

namespace Limbo.Umbraco.Time {

    internal static class HelloTimeUtils {

        /// <summary>
        /// Gets whether <code>first</code> and <code>second</code> represents the same day.
        /// </summary>
        /// <param name="first">The first date.</param>
        /// <param name="second">The second date.</param>
        /// <returns>Returns <code>true</code> if <code>first</code> and <code>second</code> represents the same day, otherwise <code>false</code>.</returns>
        public static bool IsSameDay(DateTime first, DateTime second) {
            return first.Year == second.Year && first.Month == second.Month && first.Day == second.Day;
        }

        /// <summary>
        /// Gets whether <code>first</code> and <code>second</code> represents the same day.
        /// </summary>
        /// <param name="first">The first date.</param>
        /// <param name="second">The second date.</param>
        /// <returns>Returns <code>true</code> if <code>first</code> and <code>second</code> represents the same day, otherwise <code>false</code>.</returns>
        public static bool IsSameDay(DateTimeOffset first, DateTimeOffset second) {
            return first.Year == second.Year && first.Month == second.Month && first.Day == second.Day;
        }

        /// <summary>
        /// Gets whether the specified <code>date</code> is today.
        /// </summary>
        /// <param name="date">The date date.</param>
        /// <returns>Returns <code>true</code> if <code>date</code> is today, otherwise <code>false</code>.</returns>
        public static bool IsToday(DateTime date) {
            return IsSameDay(date, DateTime.Today);
        }

        /// <summary>
        /// Gets whether the specified <code>date</code> is today.
        /// </summary>
        /// <param name="date">The date date.</param>
        /// <returns>Returns <code>true</code> if <code>date</code> is today, otherwise <code>false</code>.</returns>
        public static bool IsToday(DateTimeOffset date) {
            return IsSameDay(date, DateTimeOffset.Now);
        }

        /// <summary>
        /// Gets whether the specified <code>date</code> is tomorrow.
        /// </summary>
        /// <param name="date">The date date.</param>
        /// <returns>Returns <code>true</code> if <code>date</code> is tomorrow, otherwise <code>false</code>.</returns>
        public static bool IsTomorrow(DateTime date) {
            return IsSameDay(date, DateTime.Today.AddDays(1));
        }

        /// <summary>
        /// Gets whether the specified <code>date</code> is tomorrow.
        /// </summary>
        /// <param name="date">The date date.</param>
        /// <returns>Returns <code>true</code> if <code>date</code> is tomorrow, otherwise <code>false</code>.</returns>
        public static bool IsTomorrow(DateTimeOffset date) {
            return IsSameDay(date, DateTimeOffset.Now.AddDays(1));
        }

    }

}