// <copyright file="ScholarshipCalculator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace StudyStipendCalcAPI.Services.StudentServices.CalculatorServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StudyStipendCalcAPI.DTOs.StudentDtos;

    public class ScholarshipCalculator
        {
        public static List<StudentDto> Calculator(List<StudentDto> students, int input, double minÖsztöndijMutató, int minPrice, int maxPrice)
        {
            var avgÖsztöndíjMutató = students.Average(p => p.StipendIndex);
            var finalAVG = avgÖsztöndíjMutató > minÖsztöndijMutató ? avgÖsztöndíjMutató : minÖsztöndijMutató;

            var betterThanAVG = from student in students
                                where student.StipendIndex >= avgÖsztöndíjMutató
                                select student;

            var weakerThanAVG = from student in students
                                where student.StipendIndex < avgÖsztöndíjMutató
                                select student;

            var studentWithMaxÖM = (from student in students
                                    orderby student.StipendIndex descending
                                    select student).FirstOrDefault();

            var studentWithMinÖM = (from student in students
                                    orderby student.StipendIndex
                                    select student).FirstOrDefault();

            var sumOfPeople = students.Count();

            var jogosultCount = (from student in students
                                 where student.StipendIndex > finalAVG
                                 select student).Count();

            var avgÖsztöndíj = (input * sumOfPeople) / jogosultCount;

            foreach (var item in betterThanAVG)
            {
                 BetterCalculator(item, avgÖsztöndíjMutató, studentWithMaxÖM);
            }

            foreach (var item in weakerThanAVG)
            {
                 WeakerCalculator(item, avgÖsztöndíjMutató, studentWithMinÖM);
            }

            var avgJogosultGroupIndex = (from student in students
                                   where student.StipendIndex > finalAVG
                                   select student).Average(p => p.GroupIndex);

            var a = (maxPrice - avgÖsztöndíj) / (2 - avgJogosultGroupIndex);
            var b = avgÖsztöndíj - (a * avgJogosultGroupIndex);

            var betterStudentsThanAVG = (from student in students
                                         where student.StipendIndex >= finalAVG
                                         select student).ToList();
            foreach (var item in betterStudentsThanAVG)
            {
                CalculatePrice(item, a, b);
            }

            foreach (var item in students)
            {
                if (!betterStudentsThanAVG.Contains(item))
                {
                    item.StipendAmmount = 0;
                }
            }

            return students;
        }

            private static void BetterCalculator(StudentDto s, double avg, StudentDto studentWithMAXÖM)
            {
                s.GroupIndex = Math.Round(((s.StipendIndex - avg) / (studentWithMAXÖM.StipendIndex - avg)) + 1,4);
            }

            private static void WeakerCalculator(StudentDto s, double avg, StudentDto studentWithMINÖM)
            {
                s.GroupIndex = Math.Round(((-1 * (s.StipendIndex - avg)) / (studentWithMINÖM.StipendIndex - avg)) + 1,4);
            }

            private static void CalculatePrice(StudentDto s, double a, double b)
            {
                double összeg = ((b + (a * s.GroupIndex)) / 100) * 100;
            double rounded = Math.Round(összeg / 50, MidpointRounding.ToEven);
            s.StipendAmmount = Convert.ToInt32(rounded * 50);

            // s.Ösztöndíjösszeg = Math.Round(Convert.ToInt32(((b + (a * s.GroupIndex)) / 100) * 100))
            }
        }
    }
