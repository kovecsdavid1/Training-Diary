# GEMINI Project Analysis: TrainingDiary

## Project Overview

This is a cross-platform **Training Diary** application built with **.NET MAUI** (Multi-platform App UI). The project is written in C# and uses XAML for defining the user interface.

Its purpose is to allow users to log, view, and manage their physical training sessions on Android, iOS, macOS, and Windows devices from a single codebase.

### Core Technologies

*   **.NET MAUI (on .NET 9):** The primary framework for building the cross-platform application.
*   **C#:** The main programming language for the application logic.
*   **XAML:** Used for declaratively building the user interface.
*   **SQLite:** The application uses a local SQLite database (`trainings.db3`) to store training data on the device. The `sqlite-net-pcl` library is used for database operations.
*   **CommunityToolkit.Mvvm:** This library is used to implement the MVVM pattern, with features like `[ObservableProperty]` and `[RelayCommand]` for data binding and command handling.

### Architecture

The application follows the **Model-View-ViewModel (MVVM)** design pattern, which promotes a clean separation of concerns:

*   **Models:** (`TrainingDiary/Models/`) The `Training.cs` class defines the core data structure for a training session, including properties like name, type, intensity, and duration.
*   **Views:** (`TrainingDiary/Views/`) These are the UI pages defined in XAML files (e.g., `MainPage.xaml`, `EditTrainingPage.xaml`). They are responsible for the presentation layer.
*   **ViewModels:** (`TrainingDiary/ViewModels/`) These classes contain the presentation logic and state of the application. They expose data to the Views via data binding and handle user actions.
*   **Services:** (`TrainingDiary/Services/`) A service layer is used for tasks like database access. The `ITrainingDatabase` interface defines the contract for data operations, and `SQLiteTrainingDatabase` provides the concrete implementation.

## Building and Running

This project can be built and run using the `dotnet` command-line interface or an IDE like Visual Studio or JetBrains Rider.

### Using an IDE (Recommended)

1.  Open the `TrainingDiary.sln` solution file in Visual Studio or Rider.
2.  Select the desired target platform (e.g., Android Emulator, local Windows machine).
3.  Press the "Run" button to build and deploy the application.

### Using the .NET CLI

You can build and run the project from the command line.

**Build the project:**
```bash
dotnet build TrainingDiary.sln
```

**Run on a specific platform:**
To run the application, you must specify a target framework.

*   **To run on Android:**
    ```bash
    # Ensure an emulator is running or a device is connected
    dotnet build -t:Run -f net9.0-android
    ```
*   **To run on Windows:**
    ```bash
    dotnet build -t:Run -f net9.0-windows10.0.19041.0
    ```
*   **To run on iOS (requires a macOS machine with Xcode):**
    ```bash
    # Ensure a simulator is running or a device is connected
    dotnet build -t:Run -f net9.0-ios
    ```

## Development Conventions

*   **MVVM Pattern:** All new features should follow the MVVM pattern. UI logic resides in Views (XAML), while business/presentation logic is in ViewModels.
*   **Data Binding:** Use data binding in XAML to connect Views to ViewModel properties. The `CommunityToolkit.Mvvm` attributes (`[ObservableProperty]`) are used to reduce boilerplate code.
*   **Dependency Injection:** Services like the database should be accessed via interfaces (e.g., `ITrainingDatabase`). While the project is structured for dependency injection, the services and viewmodels are not yet registered in `MauiProgram.cs`. This should be configured for a production-ready application.
*   **Async Operations:** All database and potentially long-running operations are asynchronous and should be awaited properly using `async/await`.
*   **Navigation:** Application navigation is handled by the AppShell (`AppShell.xaml`) and programmatic routing using `Shell.Current.GoToAsync(...)`.
