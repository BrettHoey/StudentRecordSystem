using System;
using System.Collections.Generic;
using System.IO;

namespace StudentRecords
{
    // Student class
    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Grade { get; set; }

        public Student(int id, string name, int age, string grade)
        {
            ID = id;
            Name = name;
            Age = age;
            Grade = grade;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name}, Age: {Age}, Grade: {Grade}";
        }

        // student data to string
        public string Serialize()
        {
            return $"{ID},{Name},{Age},{Grade}";
        }

        // undo string to student object
        public static Student Deserialize(string data)
        {
            var parts = data.Split(',');
            return new Student(int.Parse(parts[0]), parts[1], int.Parse(parts[2]), parts[3]);
        }
    }

    // Node class definition
    public class Node
    {
        public Student Student { get; set; }
        public Node Next { get; set; }

        public Node(Student student)
        {
            Student = student;
            Next = null;
        }
    }

    // LinkedList class definition
    public class LinkedList
    {
        private Node head;

        public LinkedList()
        {
            head = null;
        }

        public void AddStudent(Student student)
        {
            Node newNode = new Node(student);
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                Node current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
        }

        public void DeleteStudent(int id)
        {
            if (head == null) return;

            if (head.Student.ID == id)
            {
                head = head.Next;
                return;
            }

            Node current = head;
            while (current.Next != null && current.Next.Student.ID != id)
            {
                current = current.Next;
            }

            if (current.Next != null)
            {
                current.Next = current.Next.Next;
            }
        }

        public void UpdateStudent(int id, Student newStudent)
        {
            Node current = head;
            while (current != null)
            {
                if (current.Student.ID == id)
                {
                    current.Student.Name = newStudent.Name;
                    current.Student.Age = newStudent.Age;
                    current.Student.Grade = newStudent.Grade;
                    return;
                }
                current = current.Next;
            }
        }

        public Student SearchStudentById(int id)
        {
            Node current = head;
            while (current != null)
            {
                if (current.Student.ID == id)
                {
                    return current.Student;
                }
                current = current.Next;
            }
            return null;
        }

        public List<Student> SearchStudentByName(string name)
        {
            List<Student> students = new List<Student>();
            Node current = head;
            while (current != null)
            {
                if (current.Student.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    students.Add(current.Student);
                }
                current = current.Next;
            }
            return students;
        }

        public void DisplayAll()
        {
            Node current = head;
            while (current != null)
            {
                Console.WriteLine(current.Student.ToString());
                current = current.Next;
            }
        }

        // Save all students to file
        public void SaveToFile(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                Node current = head;
                while (current != null)
                {
                    writer.WriteLine(current.Student.Serialize());
                    current = current.Next;
                }
            }
        }

        // Load students from file
        public void LoadFromFile(string filename)
        {
            if (!File.Exists(filename)) return;

            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Student student = Student.Deserialize(line);
                    AddStudent(student);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            LinkedList studentList = new LinkedList();
            string filename = "students.txt";

            // Load existing student data from file
            studentList.LoadFromFile(filename);

            while (true)
            {
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Delete Student");
                Console.WriteLine("3. Update Student");
                Console.WriteLine("4. Search Student by ID");
                Console.WriteLine("5. Search Student by Name");
                Console.WriteLine("6. Display All Students");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter ID: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.Write("Enter Name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter Age: ");
                        int age = int.Parse(Console.ReadLine());
                        Console.Write("Enter Grade: ");
                        string grade = Console.ReadLine();
                        studentList.AddStudent(new Student(id, name, age, grade));
                        break;
                    case 2:
                        Console.Write("Enter ID of student to delete: ");
                        int deleteId = int.Parse(Console.ReadLine());
                        studentList.DeleteStudent(deleteId);
                        break;
                    case 3:
                        Console.Write("Enter ID of student to update: ");
                        int updateId = int.Parse(Console.ReadLine());
                        Console.Write("Enter new Name: ");
                        string newName = Console.ReadLine();
                        Console.Write("Enter new Age: ");
                        int newAge = int.Parse(Console.ReadLine());
                        Console.Write("Enter new Grade: ");
                        string newGrade = Console.ReadLine();
                        studentList.UpdateStudent(updateId, new Student(updateId, newName, newAge, newGrade));
                        break;
                    case 4:
                        Console.Write("Enter ID of student to search: ");
                        int searchId = int.Parse(Console.ReadLine());
                        Student student = studentList.SearchStudentById(searchId);
                        if (student != null)
                        {
                            Console.WriteLine(student.ToString());
                        }
                        else
                        {
                            Console.WriteLine("Student not found.");
                        }
                        break;
                    case 5:
                        Console.Write("Enter Name of student to search: ");
                        string searchName = Console.ReadLine();
                        List<Student> students = studentList.SearchStudentByName(searchName);
                        if (students.Count > 0)
                        {
                            foreach (Student s in students)
                            {
                                Console.WriteLine(s.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("Student not found.");
                        }
                        break;
                    case 6:
                        studentList.DisplayAll();
                        break;
                    case 7:
                        // Save student data to file before exiting
                        studentList.SaveToFile(filename);
                        return;
                }
            }
        }
    }
}
