# COW Interpreter

Интерпретатор эзотерического языка программирования **COW** (диалект Brainfuck), написанный на C# (.NET).

Проект реализует лексический анализ (парсинг) команд COW и их исполнение виртуальной машиной.

## Требования

Для работы с проектом вам понадобятся:

*   [.NET SDK](https://dotnet.microsoft.com/download) (версии 6.0 или выше, рекомендуется .NET 9.0)
*   Инструмент для генерации отчетов о покрытии (для запуска тестов с отчетом):
    ```bash
    dotnet tool install -g dotnet-reportgenerator-globaltool
    ```

## Установка и сборка

1.  **Клонирование репозитория:**
    ```bash
    git clone https://github.com/ваш-юзернейм/cow-interpreter.git
    cd cow-interpreter
    ```

2.  **Восстановление зависимостей и сборка:**
    ```bash
    dotnet restore
    dotnet build
    ```

## Использование

Для запуска интерпретатора используйте команду `dotnet run`, передав путь к файлу с кодом `.cow` в качестве аргумента.

**Синтаксис:**
```bash
dotnet run --project src/Brainfuck.App/Brainfuck.App.csproj [путь_к_файлу.cow]
```

**Пример:**
Если у вас есть файл `hello.cow`:
```bash
dotnet run hello.cow
```

Если запустить без аргументов, программа выведет справку:
```text
Moooo COW Interpreter ooooM

Файл '' не найден!
Использование: dotnet run [имя_файла.cow]
```

## Тестирование и покрытие кода (Code Coverage)

Проект использует библиотеку **xUnit** для модульного (Unit) и интеграционного тестирования, а также **Moq** для создания заглушек (mocks).

### Запуск тестов с генерацией отчета

Для запуска всех тестов, сбора метрик покрытия кода и генерации HTML-отчета используйте следующую команду (для Linux/macOS):

```bash
dotnet test --collect:"XPlat Code Coverage" && reportgenerator -reports:"./TestResults/*/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html && xdg-open coveragereport/index.html
```

**Что делает эта команда:**
1.  `dotnet test --collect:"XPlat Code Coverage"` — запускает тесты и собирает данные о покрытии в формате XML.
2.  `reportgenerator ...` — преобразует XML-отчеты в удобный HTML-сайт в папке `coveragereport`.
3.  `xdg-open ...` — автоматически открывает отчет в браузере по умолчанию (на Linux).

> **Примечание для Windows:**
> Вместо `xdg-open` используйте `start` или просто откройте файл `coveragereport/index.html` вручную.

### Структура проекта

*   **Brainfuck.App** — Основное консольное приложение.
    *   `Core` — Логика интерпретатора (`CowInterpreter`, `MachineState`).
    *   `Parsing` — Парсер кода (`CowParser`).
    *   `IO` — Обработка ввода-вывода (`ConsoleIOHandler`).
*   **Brainfuck.Tests** — Проект с тестами.

## Используемые библиотеки

*   [xUnit](https://xunit.net/) — Фреймворк для тестирования.
*   [Moq](https://github.com/moq/moq4) — Библиотека для Mock-объектов.
*   [coverlet.collector](https://github.com/coverlet-coverage/coverlet) — Сборщик данных о покрытии кода.