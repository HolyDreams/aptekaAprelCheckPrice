using AngleSharp;
using CheckPrice;

var client = new HttpClient();
//int maxPage = 999;
string category = "losing_weight/weight_loss";
int page = 1;

List<CatalogList> results = new List<CatalogList>();
//Убрал проверку по всем страницам из за блокировки.
//for (int i = 1; i <= maxPage; i++)

var html = client.GetAsync("https://apteka.ru/category/" + category + "/?page=" + page).Result.Content.ReadAsStringAsync().Result;

var config = Configuration.Default;
using var context = BrowsingContext.New(config);
using var doc = await context.OpenAsync(req => req.Content(html));

var Card = doc.QuerySelectorAll(".CardsGrid > div:not(.CardsGrid__adv)");
//if (maxPage == 999)
//    maxPage = int.Parse(doc.QuerySelector(".Paginator").Children.Where(a => a.ClassList.Contains("pager-v3-item")).Last().TextContent);
foreach (var item in Card)
{
    double?[] prices = Methods.CheckPrice(item);

    results.Add(new CatalogList()
    {
        Name = item.QuerySelector(".catalog-card__name").TextContent,
        Company = item.QuerySelector(".catalog-card__vendor > .emphasis") == null ? "" : item.QuerySelector(".catalog-card__vendor > .emphasis").TextContent,
        DiscontPrice = prices[0],
        Price = prices[1],
    });
}

if (results.Count > 0)
{
    var sql = new SQLite();
    sql.SQLiteInsert(results, category);
    Console.WriteLine("Данные успешно занесены в базу данных!");
}
else
    Console.WriteLine("Ничего не найдено!");