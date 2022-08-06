// <copyright file="BusinessLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace StudyStipendCalcAPI.Services.StudentServices.CalculatorServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using StudyStipendCalcAPI.DTOs.StudentDtos;
    using StudyStipendCalcAPI.Models;

    public static class BusinessLogic
    { 


        public static List<string> GetModulNames(List<Student> allStud)
        {
            var res = (from student in allStud
                       where student.ModulCode.ElementAt(1) == 'B'
                       group student by student.ModulName into modulnames
                       select modulnames.Key.Replace(" ", string.Empty).ToLower()).Distinct().ToList();

            return res;
        }

        public static List<string> GetTHNames(List<Student> allStud)
        {
            var res = (from student in allStud
                       where student.ModulCode.ElementAt(1) == 'B'
                       group student by student.TelephelyName into ths
                       select ths.Key).ToList();

            return res;
        }

        public static Dictionary<string, List<Student>> GroupStud(List<Student> peoples, SemesterType semester)
        {
            Dictionary<string, List<Student>> res = new Dictionary<string, List<Student>>();
            if (semester == SemesterType.Spring)
            {
                switch (peoples[0].ModulCode[0])
                {
                    case 'R': res = GroupRkkTypeSpring(peoples); break;
                    case 'N': res = GroupNikTypeSpring(peoples); break;
                    case 'G': res = GroupKgkType(peoples); break;
                    case 'A': res = GroupAmkType(peoples); break;
                    case 'B': res = GroupBgkTypeSpring(peoples); break;
                    case 'K': res = GroupKvkTypeSpring(peoples); break;
                }
            }
            else
            {
                switch (peoples[0].ModulCode[0])
                {
                    case 'R': res = GroupRkkTypeAut(peoples); break;
                    case 'N': res = GroupNikTypeAut(peoples); break;
                    case 'G': res = GroupKgkType(peoples); break;
                    case 'A': res = GroupAmkType(peoples); break;
                    case 'B': res = GroupBgkTypeAut(peoples); break;
                }
            }

            return res;
        }

        private static Dictionary<string, List<Student>> GroupKvkTypeSpring(List<Student> peoples)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            var list = peoples.Where(p => p.FinishedSemester == 1).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list), list);
            }

            var list2 = peoples.Where(p => p.FinishedSemester >= 2).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list2.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list2), list2);
            }

            return grouppedStudents;
        }

        private static Dictionary<string, List<Student>> GroupBgkTypeAut(List<Student> peoples)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            var list = peoples.Where(p => p.Year == 1).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list), list);
            }

            var list2 = peoples.Where(p => p.Year == 2).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list2.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list2), list2);
            }

            var list3 = peoples.Where(p => p.Year == 3).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list3.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list3), list3);
            }

            var list4 = peoples.Where(p => p.Year > 3).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list4.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list4), list4);
            }

            return grouppedStudents;
        }

        private static Dictionary<string, List<Student>> GroupBgkTypeSpring(List<Student> peoples)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            var list = peoples.Where(p => p.Year == 1).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list), list);
            }

            var list2 = peoples.Where(p => p.Year == 2).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list2.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list2), list2);
            }

            var list3 = peoples.Where(p => p.Year == 3).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list3.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list3), list3);
            }

            var list4 = peoples.Where(p => p.Year == 4).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list4.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list4), list4);
            }

            var list5 = peoples.Where(p => p.Year > 4).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list5.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list5), list5);
            }

            return grouppedStudents;
        }

        private static Dictionary<string, List<Student>> GroupAmkType(List<Student> peoples)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            var list = peoples.Where(p => p.FinishedSemester == 1 || p.FinishedSemester == 2).OrderByDescending(q => q.StipendIndex).ToList();
            if (list.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list), list);
            }

            var list2 = peoples.Where(p => p.FinishedSemester == 3 || p.FinishedSemester == 4).OrderByDescending(q => q.StipendIndex).ToList();
            if (list2.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list2), list2);
            }

            if (peoples[0].ModulCode.ElementAt(1) != 'F')
            {
                var list3 = peoples.Where(p => p.FinishedSemester == 5 || p.FinishedSemester == 6).OrderByDescending(q => q.StipendIndex).ToList();
                if (list3.Count > 0)
                {
                    grouppedStudents.Add(GetSemesters(list3), list3);
                }

                var list4 = peoples.Where(p => p.FinishedSemester > 6).OrderByDescending(q => q.StipendIndex).ToList();
                if (list4.Count > 0)
                {
                    grouppedStudents.Add(GetSemesters(list4), list4);
                }
            }
            else
            {
                var list3 = peoples.Where(p => p.FinishedSemester > 4).OrderByDescending(q => q.StipendIndex).ToList();
                if (list3.Count > 0)
                {
                    grouppedStudents.Add(GetSemesters(list3), list3);
                }
            }

            return grouppedStudents;
        }

        private static Dictionary<string, List<Student>> GroupKgkType(List<Student> peoples)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            var list = peoples.Where(p => p.Year == 1).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list), list);
            }

            var list2 = peoples.Where(p => p.Year == 2).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list2.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list2), list2);
            }

            var list3 = peoples.Where(p => p.Year == 3).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list3.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list3), list3);
            }

            var list4 = peoples.Where(p => p.Year == 4).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list4.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list4), list4);
            }

            return grouppedStudents;
        }

        private static Dictionary<string, List<Student>> GroupRkkTypeAut(List<Student> peoples)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            var list = peoples.Where(p => p.Year == 1).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list), list);
            }

            var list2 = peoples.Where(p => p.Year == 2).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list2.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list2), list2);
            }

            var list3 = peoples.Where(p => p.Year == 3).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list3.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list3), list3);
            }

            return grouppedStudents;
        }

        private static Dictionary<string, List<Student>> GroupRkkTypeSpring(List<Student> peoples)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            var list = peoples.Where(p => p.Year == 1).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list), list);
            }

            var list2 = peoples.Where(p => p.Year == 2).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list2.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list2), list2);
            }

            var list3 = peoples.Where(p => p.Year == 3).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list3.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list3), list3);
            }

            var list4 = peoples.Where(p => p.Year == 4).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list4.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list4), list4);
            }

            return grouppedStudents;
        }

        private static Dictionary<string, List<Student>> GroupNikTypeAut(List<Student> peoples)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            var list = peoples.Where(p => p.FinishedSemester == 1 || p.FinishedSemester == 2).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list), list);
            }

            var list2 = peoples.Where(p => p.FinishedSemester == 3 || p.FinishedSemester == 4).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list2.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list2), list2);
            }

            var list3 = peoples.Where(p => p.FinishedSemester == 5 || p.FinishedSemester == 6).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list3.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list3), list3);
            }

            var list4 = peoples.Where(p => p.FinishedSemester > 6).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list4.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list4), list4);
            }

            return grouppedStudents;
        }

        private static Dictionary<string, List<Student>> GroupNikTypeSpring(List<Student> peoples)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            var list = peoples.Where(p => p.FinishedSemester == 1).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list), list);
            }

            var list2 = peoples.Where(p => p.FinishedSemester == 2 || p.FinishedSemester == 3).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list2.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list2), list2);
            }

            var list3 = peoples.Where(p => p.FinishedSemester == 4 || p.FinishedSemester == 5).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list3.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list3), list3);
            }

            var list4 = peoples.Where(p => p.FinishedSemester == 6 || p.FinishedSemester == 7).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list4.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list4), list4);
            }

            var list5 = peoples.Where(p => p.FinishedSemester > 7).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list5.Count > 0)
            {
                grouppedStudents.Add(GetSemesters(list5), list5);
            }

            return grouppedStudents;
        }
        public static string GetSemesters(List<Student> students)
        {
            string result = null;
            switch (students[0].ModulCode[0])
            {
                case 'R': result = RkkType(students); break;
                case 'N': result = NikType(students); break;
                case 'G': result = KgkType(students); break;
                case 'A': result = AmkType(students); break;
                case 'B': result = BgkType(students); break;
                case 'K': result = KvkType(students); break;
                default:
                    break;
            }

            return result;
        }

        private static string KvkType(List<Student> students)
        {
            string sem = string.Empty;
            var res = (from student in students
                       group student by student.Year into semesters
                       orderby semesters.Key
                       select semesters.Key).ToList();

            if (students[0].ModulCode.ElementAt(1) == 'B')
            {
                switch (students[0].StudentGrop)
                {
                    case "VI":
                        sem += "BSc VI ";
                        if (res.Count == 1)
                        {
                            sem += "1";
                        }
                        else
                        {
                            sem += "2";
                        }

                        break;
                    case "AUT": sem += "BSc GM "; break;
                    case "MAI": sem += "BSc ME "; break;
                    case "HTI": sem += "BSc GM "; break;
                    case "MTI": sem += "BSc ME "; break;
                    case "VEI": sem += "BSc VEI"; break;
                    default:
                        break;
                }
            }
            else if (students[0].ModulCode.ElementAt(1) == 'M')
            {
                sem += "MSc ";
            }
            else
            {
                sem += "Angol ";
            }

            return sem;
        }

        private static string KgkType(List<Student> students)
        {
            string sem = null;

            if (students[0].ModulCode[1] == 'B' || students[0].ModulCode[1] == 'F')
            {
                if (students[0].ModulName.ToLower().Contains("informatikus"))
                {
                    sem += "GI ";
                }
                else if (students[0].ModulName.ToLower().Contains("műszaki"))
                {
                    sem += "MM ";
                }
                else if (students[0].ModulName.ToLower().Contains("gazdálkodási"))
                {
                    sem += "GMF ";
                }
                else if (students[0].ModulName.ToLower().Contains("kereskedelem"))
                {
                    sem += "KMF ";
                }
            }
            else
            {
                sem += "VF ";
            }

            if (!sem.Contains("VF"))
            {
                var res = (from student in students
                           group student by student.Year into years
                           orderby years.Key
                           select years.Key).ToList();

                return sem += res[0].ToString() + ". évesek";
            }
            else
            {
                var res = (from student in students
                           group student by student.FinishedSemester into semesters
                           orderby semesters.Key
                           select semesters.Key).ToList();

                return sem += res[0].ToString() + " aktív félév után.";
            }
        }

        private static string NikType(List<Student> students)
        {
            string sem = null;
            var res = (from student in students
                       group student by student.FinishedSemester into semesters
                       orderby semesters.Key
                       select semesters.Key).ToList();

            if (students[0].ModulCode[1] == 'B')
            {
                if (students[0].ModulName.ToLower().Contains("üzem"))
                {
                    sem += "Bprof ";
                }
                else
                {
                    sem = "MI BSc ";
                }

                if (res.Count != 1)
                {
                    if (res.Count > 2)
                    {
                        return sem += "Képzési időn túliak";
                    }
                    else
                    {
                        foreach (var item in res)
                        {
                            sem += item.ToString() + "-";
                        }

                        return sem.Remove(sem.Length - 1, 1) + " aktív félév után";
                    }
                }
                else
                {
                    foreach (var item in res)
                    {
                        sem += item.ToString();
                    }

                    return sem += " aktív félév után";
                }
            }
            else
            {
                return sem = "MSc-n lévő hallgatók";
            }
        }

        private static string RkkType(List<Student> students)
        {
            string sem = string.Empty;
            var res = (from student in students
                       group student by student.Year into semesters
                       orderby semesters.Key
                       select semesters.Key).ToList();
            if (students[0].ModulCode[1] == 'B')
            {
                if (students[0].ModulName.ToLower().Contains("forma"))
                {
                    sem += "ITF ";
                }
                else if (students[0].ModulName.ToLower().Contains("környezet"))
                {
                    sem += "KÖM ";
                }
                else
                {
                    sem += "KIP ";
                }
            }
            else if (students[0].ModulCode[1] == 'F')
            {
                sem += "FOSZK ";
            }
            else
            {
                sem += "Msc ";
            }

            sem += res[0].ToString() + ". évesek";

            return sem;
        }

        private static string AmkType(List<Student> students)
        {
            string sem = string.Empty;
            var res = (from student in students
                       group student by student.FinishedSemester into semesters
                       orderby semesters.Key
                       select semesters.Key).ToList();

            if (students[0].ModulCode.ElementAt(1) == 'B')
            {
                switch (students[0].ModulCode.Substring(students[0].ModulCode.Length - 2, 2))
                {
                    case "FF": sem += "BSc FF "; break;
                    case "GM": sem += "BSc GM "; break;
                    case "MI": sem += "BSc MI "; break;
                    case "MM": sem += "BSc MM "; break;
                    case "VM": sem += "BSc VM "; break;
                    default:
                        break;
                }
            }
            else
            {
                sem += "Foszk MI";
            }

            if (res.Contains(1) || res.Contains(2))
            {
                sem += "1. évfolyam";
            }
            else if (res.Contains(3) || res.Contains(4))
            {
                sem += "2. évfolyam";
            }
            else if (students[0].ModulCode.ElementAt(1) == 'B')
            {
                if (res.Contains(5) || res.Contains(6))
                {
                    sem += "3. évfolyam";
                }
                else if (res[0] > 6)
                {
                    sem += "4. évfolyam";
                }
            }
            else
            {
                sem += "3. évfolyam";
            }

            return sem;
        }

        private static string BgkType(List<Student> students)
        {
            string sem = string.Empty;
            var res = (from student in students
                       group student by student.Year into semesters
                       orderby semesters.Key
                       select semesters.Key).ToList();

            if (students[0].ModulCode.ElementAt(1) == 'B')
            {
                switch (students[0].ModulCode.Substring(students[0].ModulCode.Length - 2, 2))
                {
                    case "BT": sem += "BSc BT "; break;
                    case "GM": sem += "BSc GM "; break;
                    case "ME": sem += "BSc ME "; break;
                    case "GA": sem += "BSc GM "; break;
                    case "MA": sem += "BSc ME "; break;
                    default:
                        break;
                }

                if (!(res.Count > 1) && res[0] <= 4)
                {
                    foreach (var item in res)
                    {
                        sem += item.ToString() + ". évesek";
                    }
                }
                else
                {
                    sem += "képzési időn túliak";
                }
            }
            else
            {
                sem += "MSc ME";
            }

            return sem;
        }
    }
}
