using System;
using System.Collections;
using System.Collections.Generic;

namespace MedicalRecords
{
    // Define a Patient class that contains the necessary fields for storing patient information
    public class Patient
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string MedicalHistory { get; set; }
        public List<string> CurrentMedications { get; set; }

        public Patient(string name, int age, string medicalHistory, List<string> currentMedications)
        {
            Name = name;
            Age = age;
            MedicalHistory = medicalHistory;
            CurrentMedications = currentMedications;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // Define a Medic class that contains the necessary fields for storing medic information
    public class Medic
    {
        public string Name { get; set; }
        public string Speciality { get; set; }

        public Medic(string name, string speciality)
        {
            Name = name;
            Speciality = speciality;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // Define an Iterator for the Patient collection that allows you to loop through each patient record in turn
    public class PatientIterator : IEnumerator
    {
        private List<Patient> _patients;
        private int _position = -1;

        public PatientIterator(List<Patient> patients)
        {
            _patients = patients;
        }

        public bool MoveNext()
        {
            _position++;
            return (_position < _patients.Count);
        }

        public void Reset()
        {
            _position = -1;
        }

        public object Current
        {
            get { return _patients[_position]; }
        }
    }

    // Define an Iterator for the Medic collection that allows you to loop through each medic record in reverse order
    public class MedicIterator : IEnumerator
    {
        private List<Medic> _medics;
        private int _position;

        public MedicIterator(List<Medic> medics)
        {
            _medics = medics;
            _position = medics.Count;
        }

        public bool MoveNext()
        {
            _position--;
            return (_position >= 0);
        }

        public void Reset()
        {
            _position = _medics.Count;
        }

        public object Current
        {
            get { return _medics[_position]; }
        }
    }

    // Define a Visitor interface that contains a Visit() method that takes a Patient object as its argument
    public interface IVisitor
    {
        void Visit(Patient patient);
        void Visit(Medic medic);
    }

    // Implement one or more concrete Visitor classes that perform some action on each patient record
    public class NameVisitor : IVisitor
    {
        public void Visit(Patient patient)
        {
            Console.WriteLine("Patient name: {0}", patient.Name);
        }

        public void Visit(Medic medic)
        {
            Console.WriteLine("Medic name: {0}", medic.Name);
        }
    }

    public class DataVisitor : IVisitor
    {
        public void Visit(Patient patient)
        {
            Console.WriteLine("Name: {0}, Age: {1}, Medical History: {2}, Current Medications: {3}",
                patient.Name, patient.Age, patient.MedicalHistory, string.Join(",", patient.CurrentMedications));
        }

        public void Visit(Medic medic)
        {
            Console.WriteLine("Medic Name: {0}, Speciality: {1}", medic.Name, medic.Speciality);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var patients = new List<Patient>
            {
            new Patient("John Doe", 30, "None", new List<string> { "Aspirin" }),
            new Patient("Jane Smith", 40, "High blood pressure", new List<string> { "Lisinopril" }),
            new Patient("Bob Johnson", 50, "Type 2 diabetes", new List<string> { "Metformin" })
            };

            var medics = new List<Medic>
            {
            new Medic("Dr. John Smith", "Cardiology"),
            new Medic("Dr. Jane Johnson", "Oncology"),
            new Medic("Dr. Bob Williams", "Pediatrics")
            };

            // Create an instance of the PrintVisitor
            var nameVisitor = new DataVisitor();

            // Use the PatientIterator to iterate through each patient and accept the PrintVisitor
            var patientIterator = new PatientIterator(patients);
            while (patientIterator.MoveNext())
            {
                var patient = (Patient)patientIterator.Current;
                patient.Accept(nameVisitor);
            }

            // Use the MedicIterator to iterate through each medic in reverse order and accept the MedicVisitor
            var medicIterator = new MedicIterator(medics);
            while (medicIterator.MoveNext())
            {
                var medic = (Medic)medicIterator.Current;
                medic.Accept(new DataVisitor());
            }
        }
    }
}