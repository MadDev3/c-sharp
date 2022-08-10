namespace sharp_task
{
    using System;
    using System.IO;
    using System.Diagnostics;
    using Microsoft;
    public partial class Form1 : Form
    {

        class Human
        {
            public string Name { get; set; }
            public int Appeal { get; set; }
            public int Docs { get; set; }
            public int All { get; set; }
        }

        string file1 = "Тестовое задание - Обращения.txt";
        string file2 = "Тестовое задание - РКК.txt";
        string sorting = "по идентификатору";

        List<Human> people = new List<Human>();

        public Form1()
        {
            InitializeComponent();
            label2.Text = file1;
            label3.Text = file2;
            dataGridView1.AllowUserToAddRows = false;

            button1.Click += button1_Click;
            button2.Click += Button2_Click;
        }

        private void Button2_Click(object? sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Сначала запустите алгоритм");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            string path = "output.rtf";

            using (StreamWriter writer = new StreamWriter(path))
            {
                DateTime date = DateTime.Today;
                
                int all = 0;
                int docs = 0;
                int appeal = 0;
                foreach(Human human in people)
                {
                    appeal += human.Appeal;
                    docs += human.Docs;
                    all = appeal + docs;
                }
                writer.WriteLine("Не исполнено в срок " + all + " документов, из них: \n");
                writer.WriteLine("- количество неисполненных входящих документов: " + docs + "\n");
                writer.WriteLine("- Количество неисполненных обращений граждан: " + appeal + "\n");
                writer.WriteLine("Сортировка: " + sorting + "\n");
                writer.Write("№" + "\t");
                writer.Write("{0, 25}", "Отв. исп-ль" + "\t");
                writer.Write("{0, 8}", "РКК" + "\t");
                writer.Write("{0, 8}", "Обращ." + "\t");
                writer.Write("{0, 8}", "Общee" + "\t");
                writer.WriteLine();
                for (int i = 0; i < dataGridView1.RowCount ; i++)
                {
                    for(int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        string cellName = dataGridView1.Columns[j].Name;
                        if (cellName == "id")
                        {
                            writer.Write("{00}", dataGridView1.Rows[i].Cells[j].Value.ToString() + "\t");
                        }
                        else if(cellName == "docs" || cellName == "appeal" || cellName == "all")
                        {
                            writer.Write("{0, 8}", dataGridView1.Rows[i].Cells[j].Value.ToString() + "\t");
                        }
                        else
                        {
                            writer.Write("{0, 25}",dataGridView1.Rows[i].Cells[j].Value.ToString() + "\t");
                        }
                    }

                    writer.WriteLine();
                }
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("Дата составления: " + $"{date.ToString("D")}" + "\n");
                writer.Close();
            }

        }


        private async void button1_Click(object? sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using FileStream reader = File.OpenRead(file1);
            string[] lines = File.ReadAllLines(file1);
            string[] linesDocs = File.ReadAllLines(file2);

            List<string> names = new List<string>();
            List<Human> humans = new List<Human>();
            foreach(string line in lines)
            {
                string name = "";
                int dots = 0;
                string[] values = line.Split('\t');
                if (values[0] == "Климов Сергей Александрович")
                {
                    foreach (char q in values[1])
                    {
                        if (dots == 2)
                        {
                            break;
                        }
                        if (q == '.')
                        {
                            dots++;
                        }
                        name += q;
                    }
                    int index = names.FindIndex((x) => x == name);
                    if (index == -1)
                    {
                        humans.Add(new Human() { Name = name, Appeal = 1 });
                        names.Add(name);
                    }
                    else
                    {
                        Human search = humans.Find(item => item.Name == name);
                        search.Appeal++;
                    }
                }
                else
                {
                    string[] initials = values[0].Split(" ");
                    name = initials[0] + " " + initials[1][0] + "." + initials[2][0] + ".";

                    int index = names.FindIndex((x) => x == name);
                    if(index == -1)
                    {
                        humans.Add(new Human() { Name = name, Appeal = 1});
                        names.Add(name);
                    }
                    else
                    {
                        Human search = humans.Find(item => item.Name == name);
                        search.Appeal++;
                    }
                }
            }

            foreach (string line in linesDocs)
            {
                string name = "";
                int dots = 0;
                string[] values = line.Split('\t');
                if (values[0] == "Климов Сергей Александрович")
                {
                    foreach (char q in values[1])
                    {
                        if (dots == 2)
                        {
                            break;
                        }
                        if (q == '.')
                        {
                            dots++;
                        }
                        name += q;
                    }
                    int index = names.FindIndex((x) => x == name);
                    if (index == -1)
                    {
                        humans.Add(new Human() { Name = name, Docs = 1 });
                        names.Add(name);
                    }
                    else
                    {
                        Human search = humans.Find(item => item.Name == name);
                        search.Docs++;
                    }
                }
                else
                {
                    string[] initials = values[0].Split(" ");
                    name = initials[0] + " " + initials[1][0] + "." + initials[2][0] + ".";

                    int index = names.FindIndex((x) => x == name);
                    if (index == -1)
                    {
                        humans.Add(new Human() { Name = name, Docs = 1 });
                        names.Add(name);
                    }
                    else
                    {
                        Human search = humans.Find(item => item.Name == name);
                        search.Docs++;
                    }
                }

            }

            people = humans;
            dataGridView1.Rows.Clear();

            for (int i = 0; i < humans.Count; i++)
            {
                dataGridView1.Rows.Add(i + 1, humans[i].Name, humans[i].Docs, humans[i].Appeal, humans[i].Appeal + humans[i].Docs);
            }
            sw.Stop();
            label1.Text = sw.ElapsedMilliseconds.ToString() + " ms";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 1)
            {
                sorting = "по имени";
            }
            else if(e.ColumnIndex == 2)
            {
                sorting = "по количеству документов";
            }
            else if(e.ColumnIndex == 3)
            {
                sorting = "по количеству обращений";
            }
            else if(e.ColumnIndex == 4)
            {
                sorting = "по общему количеству";
            }
            else
            {
                sorting = "по идентификатору";
            }
        }
    }
}