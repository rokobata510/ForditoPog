using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Reflection;

namespace A7BSWN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            ListBox2.Items.Clear();
            var RulesArray = File.ReadAllLines(@"rules.txt");
            foreach (string s in RulesArray)
            {
                ListViewItem data = new ListViewItem();
                data.Content = s;
                ListBox2.Items.Add(data);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        class InvalidStateException : Exception
        {
            public InvalidStateException(Tuple<char, char> key) : base("(" + key.Item1 + ";" + key.Item2 + ") nem érvényes állapot")
            {
            }
        }

        public static class Rules
        {

            public static Dictionary<Tuple<char, char>, string> ruleset;

            public static bool ApplySelectedRule(Tuple<char, char> key, ref Stack<char> stack, ref int indexer, ref string inputString)
            {
                listOfAllPastStates.Add(inputString + "," + String.Join("", stack) + "," + String.Join("", listWhereTheSecondHalfOfTheInputIs));
                try
                {

                    if (ruleset[key].Contains("pop"))
                    {
                        stack.Pop();
                        inputString = inputString.Remove(0, 1);
                        indexer++;
                        return false;
                    }
                    if (ruleset[key].Contains('(') && ruleset[key].Contains(')'))
                    {
                        string[] split;
                        if (ruleset[key] == "((e),7)")
                        {
                            split = new string[] { "(e)", "7" };
                        }
                        else
                        {
                            split = ruleset[key].Replace("(", "").Replace(")", "").Split(',');
                        }

                        string InputSecondHalf = split[1];
                        stack.Pop();
                        foreach (char c in split[0].Reverse())
                        {
                            stack.Push(c);
                        }

                        listWhereTheSecondHalfOfTheInputIs.Add(InputSecondHalf);
                        return false;
                    }
                }
                catch (KeyNotFoundException)
                {
                    throw new InvalidStateException(key);
                }
                catch (Exception)
                {
                    throw new Exception("Ezt az errort nem kellene hogy dobja :(");
                }
                return true;


            }


        }

        public string input = "";

        public string Input
        {
            get { return input; }
            set
            {
                if (input != value)
                {
                    input = value;
                    OnPropertyChanged(nameof(Input));
                }
            }
        }
        public static List<string> listWhereTheSecondHalfOfTheInputIs = new List<string>();
        public List<string> ListWhereTheSecondHalfOfTheInputIs
        {
            get { return listWhereTheSecondHalfOfTheInputIs; }
            set
            {
                if (listWhereTheSecondHalfOfTheInputIs != value)
                {
                    listWhereTheSecondHalfOfTheInputIs = value;
                    OnPropertyChanged(nameof(ListWhereTheSecondHalfOfTheInputIs));
                }
            }
        }

        static public List<string> listOfAllPastStates = new List<string>();

        bool CheckIfValid(string input)
        {
            int indexer = 0;
            Stack<char> stack = new Stack<char>();
            stack.Push('#');
            stack.Push('e');
            input = AccentHandler(input);
            if (input.Length == 0)
            {
                return false;
            }
            if (input[input.Length - 1] != '#')
            {
                input += '#';
            }
            this.input = input;
            while (true)
            {
                try
                {
                    Tuple<char, char> IndexeloASzabalyMatrixhoz = new Tuple<char, char>(stack.Peek(), input[0]);
                    if (Rules.ApplySelectedRule(IndexeloASzabalyMatrixhoz, ref stack, ref indexer, ref input))
                    {
                        return true;
                    }
                }
                catch (InvalidStateException)
                {
                    return false;
                }

            }
        }

        string AccentHandler(string stringwithapostrophes) => stringwithapostrophes.Replace("e\'", "é").Replace("t'", "ť");

        void ImportRules()
        {
            Rules.ruleset = new Dictionary<Tuple<char, char>, string>();
            string[] lines = File.ReadAllLines(@"Rules.txt");
            foreach (string line in lines)
            {
                string[] split = line.Split(';');
                Rules.ruleset.Add(new Tuple<char, char>(split[0][0], split[0][1]), split[1]);
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            ImportRules();
            ListBox1.Items.Clear();
            if (CheckIfValid(input))
            {
                OnPropertyChanged(nameof(listOfAllPastStates));
                listWhereTheSecondHalfOfTheInputIs.ForEach(x => TextBox2.Text += x);
                foreach (string s in listOfAllPastStates)
                {
                    string[] split = s.Split(',');
                    ListViewItem data = new ListViewItem();
                    data.Content = "(" + split[0] + ")  (" + split[1] + ", " + split[2] + ")";
                    ListBox1.Items.Add(data);
                }
                ListBox1.Items.Add("elfogadva");
                MessageBox.Show("A szó elfogadva");
            }
            else
            {
                listWhereTheSecondHalfOfTheInputIs.ForEach(x => TextBox2.Text += x);
                foreach (string s in listOfAllPastStates)
                {
                    string[] split = s.Split(',');
                    ListViewItem data = new ListViewItem();
                    data.Content = "(" + split[0] + ")  (" + split[1] + ", " + split[2] + ")";
                    ListBox1.Items.Add(data);
                }
                ListBox1.Items.Add("elutasítva");
                MessageBox.Show("A szó elutasítva");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            bool? result = openFileDlg.ShowDialog();
            if (result == true)
            {
                var RulesArray = File.ReadAllLines(openFileDlg.FileName);
                ListBox2.Items.Clear();
                foreach (string s in RulesArray)
                {
                    ListViewItem data = new ListViewItem();
                    data.Content = s;
                    ListBox2.Items.Add(data);
                }
            }
        }
    }
}
