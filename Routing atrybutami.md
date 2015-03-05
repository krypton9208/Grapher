# Program. dla internetu technologii ASP.NET: 
## 1. Routing przez atrybuty - 
####ATTRIBUTE ROUTING - O CO CHODZI?
Attribute routing (bo tak właśnie brzmi oryginalna nazwa tego mechanizmu) wprowadza do odrobinę już skostniałych reguł routingu powiew świeżości. Chyba można śmiało stwierdzić, że od momentu powstania samego routingu nie pojawiła się w nim tak istotna nowość, która nie oszukujmy się, była bardzo wyczekiwana w tym przypadku.
Dotychczas wszystkie reguły znajdowały się w centralnym miejscu. Najpierw był to plik Global.asax, a później zostały one wydelegowane do dokumentu RouteConfig.cs. Najważniejszym elementem tego pliku jest oczywiście metoda RegisterRoutes, która domyślnie wygląda mniej więcej tak:
```sh
public static void RegisterRoutes(RouteCollection routes)
{
    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
 
    routes.MapRoute(
        name: "Default",
        url: "{controller}/{action}/{id}",
        defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
    );
}
```
Nowe podejście pozwala na swego rodzaju decentralizację. Od teraz możemy tworzyć odpowiednie reguły przypisując je bezpośrednio do kontrolerów oraz metod. W tym celu wykorzystywane tytułowe atrybuty, czyli mechanizm dobrze znany w ASP.NET MVC, wykorzystywany choćby do Data annotations.
Dzięki takiemu podejściu możemy w 100% określić sposób wywołania określonego kontrolera i/lub akcji. Niestety wprowadzenie mechanizmu atrybutów dla wybranego elementu, automatycznie wyłącza go z konwencjonalnych reguł routingu. Jest to w pewnym sensie niebezpieczne, ponieważ nowy mechanizm musi zostać wcześniej aktywowany w metodzie RegisterRoutes. Jeśli tego nie uczynimy to możemy doprowadzić do sytuacji, że pewien określony kod będzie w ogóle nieosiągalny.
####ATTRIBUTE ROUTING - AKTYWACJA MECHANIZMU
Tak jak wspomniałem w poprzednim akapicie, aby skorzystać z nowego mechanizmu musimy go najpierw włączyć. Kod odpowiedzialny za tą operację najlepiej umieścić na początku metody RegisterRoutes:

```sh
public static void RegisterRoutes(RouteCollection routes)
{
    routes.MapMvcAttributeRoutes();
     
    // konwencjonalne reguły
}
```
Przechodząc na nowy mechanizm, warto mimo wszystko pozostawić domyślną regułę, która powinna obsłużyć ewentualnie nieobsłużone przez atrybuty metody. W wielu przypadkach jest to dużo lepsze wyjście niż sztuczne wpisywanie atrybutów przy każdej metodzie/kontrolerze, w momencie gdy normalnie obsłużyła by je domyślna reguła.
####ATTRIBUTE ROUNTING W PRAKTYCE
Po aktywacji mechanizmu, możemy przystąpić do pisania reguł w nowy sposób. Jest to ogółem bardzo proste i sprowadza się do zdefiniowania wartości atrybutu Route przy elemencie, dla którego ścieżkę chcemy określić. Dla każdego tego typu atrybutu możemy określić również nazwę, ale większość osób trzyma się konwencji ograniczającej się do podania tylko wspomnianej wyżej reguły określonej w API jako parametr Template. Spójrzcie na poniższy listing kodu (dla uproszczenia pomijam i będę pomijał usingi):
```sh
public class HelloController : Controller
{
    [Route("Test")]
    public string Index()
    {
        return "Hello World";
    }
 
    public string SayAge(int age)
    {
        return "Your age is " + age;
    }
}
```
Dodaliśmy nowy atrybut do metody Index w kontrolerze Hello. Korzystając ze starego sposobu i domyślnej reguły, moglibyśmy się dostać do tej metody wykorzystując ścieżkę:
```sh
http://host/Hello/Index/
```
Ponieważ jednak postanowiliśmy skorzystać z nowego wynalazku, adres uprościł się do postaci:
```sh
http://host/Test/
```
Jednocześnie tak jak wspominałem wcześniej, automatycznie wyłączyliśmy dostęp starego typu do tego zasobu, co łatwo możecie sprawdzić w trybie debugowania. Nie ruszaliśmy metody About i ta będzie dostępna po staremu:
```sh
http://host/Hello/About/
```
Ale tylko przy założeniu, że nie skasowaliście domyślnej reguły, o czym wspominałem Wam wcześniej w tym tekście;-)
####SEGMENTY I OGRANICZENIA
Routing na bazie atrybutów obsługuje oczywiście segmenty, które dobrze znacie z klasycznych reguł. Nie ma również problemu z umieszczaniem predefiniowanych zmiennych oraz obsługą parametrów wybranej metody:
```sh
[Route("{controller}/Age/{age}")]
public string SayAge(int age)
{
    return "Your age is " + age;
}
```
Tak skonstruowana reguła pozwoli na definiowanie wywołań w postaci:
```sh
http://host/Hello/Age/xyz/
```
Gdzie xyz to wasz wiek, który zostanie przetworzony dalej przez metodę (w tym przypadku po prostu wyświetli się on na ekranie razem z resztą zdefiniowanego wewnątrz stringu). Jak pewnie zauważyliście zastosowaliśmy tutaj:
Zmienną mówiącą o użyciu aktualnego kontrolera
Zmienioną nazwę metody - zamiast SayAge - Age
Wiek w postaci zmiennej age
Niestety nasze rozwiązanie ma pewną wadę. Choć metoda wyraźnie przyjmuje parametr typu int, to takich ograniczeń w żaden sposób nie stawia już nasza reguła routingu. Jeśli w takiej sytuacji użytkownik spróbuje np. takiego wywołania:
```sh
http://host/Hello/Age/Zenek/
```
Na ekranie może pojawić się bardzo brzydki błąd. Na szczęście możemy to szybko i łatwo zmienić, wprowadzając ograniczenie dla typu int do naszego parametru:
```sh
[Route("{controller}/Age/{age:int}")]
public string SayAge(int age)
{
    return "Your age is " + age;
}
```
Dzięki takiemu rozwiązaniu, metoda zostanie wywołana tylko w sytuacji gdy użytkownik rzeczywiście poda wartość o odpowiadającym nam typie. Jeśli tego nie zrobi, to na ekranie pojawi się błąd 404 - chyba że zdefiniujemy inną regułą dla tego typu sytuacji awaryjnych.
####ROUTE PREFIX
Jeśli chcielibyście by cały kontroler korzystał z określonej nazwy, możecie zdefiniować atrybut typu RoutePrefix, który będzie stanowił prefiks dla wszystkich reguł (metod) wewnątrz tej klasy:
```sh
[RoutePrefix("Hello")]
public class HelloController : Controller
{
    [Route("~/Test")]
    public string Index()
    {
        return "Hello World";
    }
 
    [Route("Age/{age:int}")]
    public string SayAge(int age)
    {
        return "Your age is " + age;
    }
}
```
Dzięki temu atrybutowi nie będziecie musieli podawać nazwy kontrolera przy każdej z jego metod, co widać przy okazji ścieżki do metody SayAge (jej wywołanie nie zmieniło się). RoutePrefix można oczywiście obejść i jest to bardzo przydatne w sytuacji gdy jakaś konkretna metoda ma być dostępna spod innej ścieżki. W takim celu wystarczyć zastosować znak tyldy, który standardowo odwołuje się do głównego katalogu aplikacji.
Ostatecznie dostęp do obu metod uzyskamy za pomocą następujących ścieżek:
```sh
http://host/Test/
```
```sh
http://host/Hello/Age/xyz/
```
####PODSUMOWANIE
Attribute routing daje nowe tchnienie mechanizmowi routingu w ASP.NET MVC. Dzięki temu podejściu łatwiej jest uzyskać konkretny, specyficzny efekt. W moim odczuciu nie warto jednak popadać w hura optymizm, ponieważ starsze rozwiązanie działa bardziej globalnie i w wielu prostych aplikacjach będzie wystarczające. Włączenie mechanizmu atrybutów sprawa zaś, że musimy teraz uważać na każde miejsce aplikacji, by przypadkiem czasem nie zablokować sobie dostęp do określonej funkcjonalności.

####MATERIAŁY

* [Pro ASP.NET MVC 5 - Adam Freeman]
* [Blogs MSDN]


[Pro ASP.NET MVC 5 - Adam Freeman]:http://www.amazon.com/Pro-ASP-NET-MVC-Adam-Freeman/dp/1430265299/
[Blogs MSDN]:http://blogs.msdn.com/b/webdev/archive/2013/10/17/attribute-routing-in-asp-net-mvc-5.aspx
