# LibraryAPI
API рабочего места библиотекаря (C#, .NET Core, Entity Framework, MS SQL Server LocalDB)

//Описание методов API с примерами HTTP запросов с телом JSON

1. Работа с книгами

//добавить новую книгу
POST: api/Books
{
    "Name":"War and Peace",
    "Author":"Tolstoy L.N.",
    "Article":"221n5",
    "YearPublic":1867
}

//добавить экземпляр книги
POST: api/BookExamples
{
	"BookId":1,
	"IsAccess":true
}

//изменить данные о книге
PUT: api/Books/1
{
	"Id":1,
    "Name":"War and Peace",
    "Author":"Tolstoy L.N.",
    "Article":"254n1",
    "YearPublic":1867,
	"Lock":false
}

//удалить данные о книге (параметр Lock = true)
PUT: api/Books/1
{
    "Id":1,
    "Name":"Anna Karenina",
    "Author":"Tolstoy L.N.",
    "Article":"254n1",
    "YearPublic":1867,
	"Lock":true
}

//удалить экземпляр книги
DELETE: api/BookExamples/5

//получить данные о книге по ID
GET: api/Books/5

//получить список выданных книг
GET: api/BookExamples/given

//получить список доступных для выдачи книг
GET: api/BookExamples/available

//поиск книг(и) по наименованию или его части (substring)
GET: api/Books/name=substring


2. Работа с читателями

//добавить нового читателя
POST: api/Readers
{
    "FIO":"Antonov A.A.",
    "Birthday":"1990-01-05T00:00:00"
}

//изменить данные о читателе
PUT: api/Readers/1
{
	"Id":1,
	"FIO":"Antonov A.A.",
    "Birthday":"1990-02-05T00:00:00",
	"Lock":false
}

//удалить данные о читателе (параметр Lock = true)
PUT: api/Readers/1
{
	"Id":1,
	"FIO":"Antonov A.A.",
    "Birthday":"1990-02-05T00:00:00",
	"Lock":true
}

//выдать книгу читателю
POST: api/Issues/bookOut
{
	"Date_start":"2021-11-11T12:34:56", 
	"Date_end":null
	"BookExampleId":1,
	"ReaderId":2
}

//сдать книгу в библиотеку
PUT: api/Issues/bookIn/1
{
	"Id":1,
	"Date_start":"2021-11-11T12:34:56", 
	"Date_end":"2021-11-14T09:12:15",
	"BookExampleId":1,
	"ReaderId":2
}

//получить данные о читателе по ID со списком выданных книг
GET: api/Readers/2

//поиск читателя(-ей) по ФИО или его части (substring) со списком выданных книг
GET: api/Readers/name=substring