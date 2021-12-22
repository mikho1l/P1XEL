using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace Pixel
{
    public static class GoogleAPI
    {
        static string ClientSecret = "client_secret.json";
        static readonly string[] ScopesSheets = { SheetsService.Scope.Spreadsheets };
        static readonly string AppName = "PixelTest";
        static readonly string SpreadSheetId = "1usT4OkiU-ipim3RU3_yQB03Rjz734s3TFIsrvIVHjKE";
        static readonly string[] Months = { "0", "Январь", "Февраль", "Март", "Апрель", "Май",
        "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
        };
        public static Dictionary<string, int> TabsIDs = new Dictionary<string, int>
        {
            {"10.2021", 1802178858},
            {"11.2021", 1432592172},
            { "12.2021", 587762026}
        }; //добавить в окно настроек изменение этого словаря суперадмином
        public static List<Bron> GetDataGoogle(SheetsService service, DateTime From, DateTime To)
        {
            var res = new List<Bron>();
            for (var cur = From; (cur.Year <= To.Year) && (cur.Month != To.AddMonths(1).Month); cur = cur.AddMonths(1))
            {
                string range = string.Format($"'{Months[cur.Month]} {cur.Year % 100}'!A2:I");
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(SpreadSheetId, range);
                ValueRange response = request.Execute();
                int i = 1;
                foreach (var value in response.Values)
                {
                    var dt = DateTime.Parse(value[0].ToString());
                    var check = value.Count > 7 && value[7] != null && value[7].ToString().Length > 5;
                    if (dt <= To && dt >= From && check)
                    {
                        var v2 = value[2].ToString();
                        var v3 = value[3].ToString();
                        var haveendtime = v3.Length > 6 && char.IsDigit(v3[6]);
                        string html = value[7].ToString();
                        MatchCollection summ = Regex.Matches(html, @"(\d+)");
                        string[] s = Regex.Split(html, @"(\d+)");
                        string Name = s.Length != 0 ? s[0] : "";
                        string num = summ.Count != 0 ? summ[0].ToString() : "";
                        var br = new Bron
                        {
                            Date = string.Format($"{value[0]}.{From.Year}"),
                            Zal = v2 == "Большой" ? Zal.Большой : (v2 == "Средний" ?
                            Zal.Средний : Zal.Малый),
                            StartTime = string.Format($"{v3[0]}{v3[1]}:{v3[3]}{v3[4]}"),
                            EndTime = haveendtime ? string.Format($"{v3[6]}{v3[7]}:{v3[9]}{v3[10]}") : null,
                            GuestCount = value[4].ToString(),
                            PredOpl = value[6].ToString(),
                            Guest = new Guest { Name = Name, Phone = num } //регулярку на values[7]
                        };
                        if (value.Count > 8) br.Comment = value[8].ToString();
                        br.SetRowGoogle(i);
                        res.Add(br);
                    }
                    i++;
                }
            }
            return res;
        }
        public static void SetDataGoogle(SheetsService service, AddDelChange adc, ref Bron bron)
        {
            if (adc != AddDelChange.Add)
            {

            }
            else
            {
                //не работает, если в другом месяце
                string range = string.Format($"'{Months[bron.GetDateTime().Month]} {bron.GetDateTime().Year % 100}'!A2:I");
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(SpreadSheetId, range);
                ValueRange response = request.Execute();
                bron.SetRowGoogle(response.Values.Count);
            }
            BatchUpdateSpreadsheetRequest busr = new BatchUpdateSpreadsheetRequest
            {
                Requests = new List<Request> { bron.ToRequest() }
            };
            service.Spreadsheets.BatchUpdate(busr, SpreadSheetId).Execute();
        }
        public static UserCredential GetUserCredentials()
        {
            using (var stream = new FileStream(ClientSecret, FileMode.Open, FileAccess.Read))
            {
                var credpath = Path.Combine(Directory.GetCurrentDirectory(), "sheetsCreds.json");
                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets, ScopesSheets, "user", CancellationToken.None,
                    new FileDataStore(credpath, true)).Result;
            }
        }
        public static SheetsService GetService(UserCredential credential)
        {
            return new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = AppName
            });
        }
    }
    public static class BronRequest
    {
        public static Request ToRequest(this Bron b)
        {

            var values = new List<CellData>();
            #region AddingEveryField
            values.Add(new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = b.Date
                }
            });
            values.Add(new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = b.DayWeek
                }
            });
            values.Add(new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = b.Zal.ToString()
                }
            });
            values.Add(new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = string.Format($"{b.StartTime}-{b.EndTime}")
                }
            });
            values.Add(new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = b.GuestCount
                }
            });
            values.Add(new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = b.Sum().ToString()
                }
            });
            values.Add(new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = b.PredOpl
                }
            });
            values.Add(new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = string.Format($"{b.Guest.Name} {b.Guest.Phone}")
                }
            });
            values.Add(new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = b.Comment
                }
            });
            #endregion
            return new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Start = new GridCoordinate
                    {
                        SheetId = GoogleAPI.TabsIDs[string.Format($"{b.GetDateTime().Month}.{b.GetDateTime().Year}")],
                        RowIndex = b.GetRowGoogle(),
                        ColumnIndex = 0
                    },
                    Rows = new List<RowData> { new RowData { Values = values } },
                    Fields = "userEnteredValue"
                }
            };
        }
    }
}
