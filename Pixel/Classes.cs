using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Pixel
{
    public enum Zal { Малый, Средний, Большой };
    public enum AddDelChange { Add, Del, Change };
#pragma warning disable CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning disable CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
    public class Bron : IComparable<Bron>
#pragma warning restore CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
#pragma warning restore CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
    {
        DateTime datetime;
        TimeSpan? timeend;
        int row_gogle;
        public string Date
        {
            get => datetime.Date.ToShortDateString();
            set
            {
                var t = datetime.TimeOfDay;
                datetime = DateTime.Parse($"{value} {t}");
            }
        }
        public Zal Zal { get; set; }
        public string DayWeek
        {
            get
            {
                string[] dw = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс" };
                return dw[(int)datetime.DayOfWeek]; //может тут с нуля
            }
            private set { }
        }
        public string StartTime
        {
            get => string.Format($"{datetime.TimeOfDay.ToString().Remove(5)}");
            set
            {
                var t = datetime.Date.ToShortDateString();
                datetime = DateTime.Parse($"{t} {value}");
            }
        }
        public string EndTime
        {
            get => timeend == null ? string.Empty : timeend.ToString().Remove(5);
            set
            {
                if (value != null)
                {
                    timeend = new TimeSpan(int.Parse($"{value[0]}{value[1]}"), int.Parse($"{value[3]}{value[4]}"), 0);
                }
            }
        }
        public string GuestCount { get; set; }
        public string PredOpl { get; set; }
        public Guest Guest { get; set; }
        public string Comment { get; set; }

        public float CountofHrs()
        {
            var t = timeend.Value.Subtract(datetime.TimeOfDay);
            if (t.Minutes < 16)
                return t.Hours;
            else if (t.Minutes < 45)
            {
                return (float)(t.Hours + 0.5);
            }
            return t.Hours + 1;
        }

        /// <summary>
        /// общая сумма
        /// </summary>
        public int Sum()
        {
            //выбрать зал из товаро
            return 100;
        }
        /// <summary>
        ///  Компаратор для сортировки в DataGrid
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Bron other)
        {
            if (datetime > other.datetime) return 1;
            else if (datetime < other.datetime) return -1;
            //else if (datetime.TimeOfDay < other.datetime.TimeOfDay) return -1;
            //else if (datetime.TimeOfDay > other.datetime.TimeOfDay) return 1;
            return 0;
        }
        public DateTime GetDateTime()
        {
            return datetime;
        }
        public int GetRowGoogle()
        {
            return row_gogle;
        }
        public void SetRowGoogle(int s)
        {
            row_gogle = s;
        }
        public static bool operator ==(Bron one, Bron two)
        {
            return one.datetime == two.datetime && one.Guest == two.Guest && one.Zal == two.Zal;
        }
        public static bool operator !=(Bron one, Bron two)
        {
            return !(one.datetime == two.datetime && one.Guest == two.Guest && one.Zal == two.Zal);
        }
    }
#pragma warning disable CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning disable CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
    public class Guest
#pragma warning restore CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
#pragma warning restore CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime LastVisit { get; set; }
        public DateTime VisitCount { get; set; }
        public string Comment { get; set; }
        public override string ToString()
        {
            return Name + " " + Phone;
        }
        public static bool operator ==(Guest one, Guest two)
        {
            bool[] otn = { one is null, two is null };
            if (otn[0] || otn[1] == false)
                return one.Name == two.Name && one.Phone == two.Phone;
            if (otn[0] && otn[1]) return true;
            return false;
        }
        public static bool operator !=(Guest one, Guest two)
        {
            return !(one.Name == two.Name && one.Phone == two.Phone);
        }
    }
    public class Admin
    {
        public DateTime DateStart { get; set; }
        public string Name { get; set; }
        //public photo
        //password
        public override string ToString()
        {
            return Name;
        }

    }
    public class SuperAdmin : Admin
    {
        public void GiveSuper(ref Admin two)
        {
            two = (SuperAdmin)two; //проверить этот момент
        }
        //public void Change(что менять: цены, списки) и тут ли это вообще делать (наверное, нет)

        //работа с листом Admines:
        public void NewAdmin()
        {

        }
        public void DeleteAdmin()
        {

        }
    }
    /// <summary>
    /// Платные товары и услуги
    /// Регулируется в окне sudo
    /// </summary>
    public class Tovar
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public Tovar(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }
        public override string ToString()
        {
            return Name + " " + Cost;
        }
    }
    public class Prodaza
    {
        public Tovar Tovar { get; set; }
        public string SpOpl { get; set; }
        public string DateTime { get; set; }
        //public string Zal { get; set; }

        public Prodaza(Tovar t, string spOPl, string dt)
        {
            Tovar = t;
            SpOpl = spOPl;
            DateTime = dt;
            // Time = time;
        }

        public override string ToString()
        {
            return Tovar.Name + " " + Tovar.Cost + DateTime;
        }

    }

    public class Otchet
    {
        public List<Prodaza> AllProdaz;
        public List<Prodaza> OtchetPoProdazamProductov { get; set; }
        public List<Prodaza> OtchetPoProdazamZalov { get; set; }
        public Dictionary<string, int> SposobOpl { get; set; }
        public int Viruchka { get; set; }
        public int DayZp { get; set; }
        public int Rashod { get; set; }

        public Otchet()
        {
            OtchetPoProdazamProductov = new List<Prodaza>();
            OtchetPoProdazamZalov = new List<Prodaza>(); ;
            SposobOpl = new Dictionary<string, int>();
            SposobOpl.Add("nal", 0);
            SposobOpl.Add("per", 0);
            SposobOpl.Add("evo", 0);
            DayZp = 0;
            Rashod = 0;
        }
        //public Admin Admin { get; set; }
        public void UnionProdaz()
        {
            AllProdaz = OtchetPoProdazamProductov.Union(OtchetPoProdazamZalov).ToList();
        }
        public void RasschetZp(int cloak, int tarif, int inoe)
        {
            DayZp = cloak * tarif + inoe;
        }
        public void RasschetViruchki()
        {
            Viruchka = SposobOpl["per"] + SposobOpl["evo"] + SposobOpl["nal"] - DayZp - Rashod;
        }
        //список товаров и сумма
    }
    public class DaySalary
    {
        public float HrsWithGuests;
        public float HrsWithoutGuests;
        public int WithPerHr;//эти 2 поля задаются супер
        public int WithoutPerHr;
        public int Dop;
        public float Itog
        {
            get
            {
                return HrsWithGuests * WithPerHr + HrsWithoutGuests * WithoutPerHr + Dop;
            }
            private set
            {

            }
        }
    }
}
