using System;

namespace MyELib.Clients.Console.Modules
{
    public static class InputModule
    {
        public static Guid InputGuid()
        {
            System.Console.Write("Введите Guid: ");
            if (Guid.TryParse(System.Console.ReadLine(), out Guid inputGuid))
                return inputGuid;
            return Guid.Empty;
        }

        public static CreateLibraryDto InputLibrary()
        {
            System.Console.Write("Введите название: ");
            var name = System.Console.ReadLine() ?? "Безымянная библиотека";
            System.Console.WriteLine("Добавьте документы: ---ПРОПУСКАЕТСЯ---");
            return new CreateLibraryDto { Name = name, DocumentIds = [] };
        }

        public static UpdateLibraryDto InputUpdateLibrary()
        {
            string name = string.Empty;

            while (true)
            {
                OutputModule.PrintUpdateMenu();
                switch (System.Console.ReadLine())
                {
                    case "1":
                        System.Console.Write("Введите новое название: ");
                        name = System.Console.ReadLine() ?? "Безымянная библиотека";
                        break;
                    case "2":
                        break;
                    case "3":
                        return new UpdateLibraryDto { Name = name, DocumentIds = [] };
                    default:
                        break;
                }
            }
        }
    }
}
