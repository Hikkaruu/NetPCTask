# Recruitment task for NET PC sp. z o.o
<br>

The database is hosted on neon.tech

## Installation of Required Applications

### 1. VisualStudio (Backend C# .NET API)
Download and install [Visual Studio](https://visualstudio.microsoft.com/pl/downloads/)

### 2. NodeJS (Frontend React)
Download and install [NodeJS](https://nodejs.org/en/download/prebuilt-installer)

## Download and Compilation:

### 1. Clone the repository:
```bash
git clone https://github.com/Hikkaruu/NetPCTask.git
```
### 2.1. Method 1
  - Run the <b>start.bat</b> file. If it's missing (due to a false positive from antivirus software), change the extension of <b>start.txt</b> to <b>start.bat</b> and run it.
  - The <b>backend</b> should start at:
    ```bash
    http://localhost:5288/swagger/index.html
    ```
  - The <b>frontend</b> should start at:
    ```bash
    http://localhost:3000/
    ```
### 2.2 Method 2
If for any reason the application doesn't start using "Method 1":

<b>Backend</b>
- Open the project using Visual Studio, or
- Using the console, navigate to the <b>Back\NetPCTask</b> directory and run the command:
  ```bash
  dotnet run
  ```
<b>Frontend</b>
- Using the console, navigate to the <b>Front\netpctask</b> directory and run the commands:
   ```bash
  npm install
  ```
  ```bash
  npm start
  ```
