var libraryMapper = new LibraryMapper();
var libraryRepository = new LibraryInMemoryRepository();
var libraryService = new LibraryService(libraryRepository, libraryMapper);

while (true)
{
	OutputModule.PrintMenu();
	OutputModule.PrintMenuChoice();
    switch (Console.ReadLine())
	{
		case "1":
			var collection = await libraryService.GetAllAsync(CancellationToken.None);
			foreach (var lib in collection)
                OutputModule.PrintLibrary(lib);
            break;
		case "2":
            var id = InputModule.InputGuid();
			var library = await libraryService.GetByIdAsync(id, CancellationToken.None);
			if (library is null) 
				break;
			OutputModule.PrintLibrary(library);
            break;
		case "3":
			var dto = InputModule.InputLibrary();
			await libraryService.CreateAsync(dto, CancellationToken.None);
			break;
		case "4":
			var updId = InputModule.InputGuid();
			var updLibrary = await libraryService.GetByIdAsync(updId, CancellationToken.None);
			var newLibrary = InputModule.InputUpdateLibrary();
			await libraryService.UpdateAsync(newLibrary, updId, CancellationToken.None);
			break;
		case "5":
			var delId = InputModule.InputGuid();
			await libraryService.DeleteAsync(delId, CancellationToken.None);
			break;
		default:
			return;
	}
}