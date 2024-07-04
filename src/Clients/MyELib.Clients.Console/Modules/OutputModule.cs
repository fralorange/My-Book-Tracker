namespace MyELib.Clients.Console.Modules
{
    public static class OutputModule
    {
        public static void PrintMenu()
        {
            System.Console.WriteLine("Введите вариант использования: ");
            System.Console.WriteLine("1 - Просмотреть все библиотеки;");
            System.Console.WriteLine("2 - Получить библиотеку по уникальному идентификатору;");
            System.Console.WriteLine("3 - Создать новую библиотеку;");
            System.Console.WriteLine("4 - Редактировать существующую библиотеку;");
            System.Console.WriteLine("5 - Удалить существующую библиотеку;");
            System.Console.WriteLine("Любой другой символ - выход из приложения");
        }

        public static void PrintUpdateMenu()
        {
            System.Console.WriteLine("Введите вариант использования для редактирования библиотеки: ");
            System.Console.WriteLine("1 - Редактировать название");
            System.Console.WriteLine("2 - Редактировать документы (пропускается)");
            System.Console.WriteLine("3 - Подтвердить выбор");
        }

        public static void PrintMenuChoice()
        {
            System.Console.Write("Выбор: ");
        }

        public static void PrintLibrary(LibraryDto lib)
        {
            System.Console.WriteLine($"Идентификатор: {lib.Id};\nНазвание: {lib.Name};\nДокументы: {lib.Documents?.Select(doc => doc.Id.ToString())}");
        }
    }
}
