# SPORT_SHOP

Информационная система для магазина ООО «Спортивные товары», которая позволяет просматривать, управлять и заказывать спортивные товары и аксессуары.

## Начало работы

Эти инструкции предоставят вам копию проекта и помогут запустить на вашем локальном компьютере для разработки и тестирования.

### Необходимые условия

Для установки программного обеспечения вам потребуется:

• Visual Studio 2019 или выше: Убедитесь, что у вас установлена версия Visual Studio с поддержкой WPF.

• .NET Framework 4.7.2 или выше: Проверьте, что нужная версия .NET Framework установлена на вашем компьютере.


#### Пример установки

Пример установки Visual Studio:

1. Перейдите на официальный сайт Visual Studio.

2. Выберите версию (Community, Professional или Enterprise) и нажмите "Download".

3. Запустите установщик и выберите необходимые компоненты (WPF, .NET Desktop Development).

### Установка

#### 1. Создание проекта на Visual Studio (WPF, .NET Framework)

1. Откройте Visual Studio.
2. На стартовом экране выберите **Create a new project**.
3. В поиске шаблонов введите **WPF App (.NET Framework)** и выберите соответствующий шаблон.
4. Нажмите **Next**.
5. Укажите имя проекта (например, `Sport_Shop`), выберите расположение, и нажмите **Create**.
6. В появившемся окне выберите версию **.NET Framework** (например, 4.7.2) и подтвердите выбор.

На этом этапе проект будет создан, и вы сможете приступить к разработке интерфейса и логики.

---

#### 2. Подключение к базе данных SQL Server (SSMS) через ADO.NET

1. **Создание базы данных**:
   - Откройте **SQL Server Management Studio (SSMS)**.
   - Подключитесь к серверу.
   - Создайте новую базу данных (например, `SportShopDb`) через контекстное меню **Databases > New Database**.
   - Выполните SQL-скрипт (если имеется) для создания таблиц. Для этого выберите вашу базу данных, откройте **New Query**, вставьте скрипт и выполните его.

2. **Добавление ADO.NET модели в проект**:
   - В Visual Studio в **Solution Explorer** щелкните правой кнопкой мыши по вашему проекту и выберите **Add > New Item**.
   - Выберите **Data** в списке категорий и найдите элемент **ADO.NET Entity Data Model**.
   - Укажите имя модели (например, `SportShopModel`) и нажмите **Add**.
   - В открывшемся мастере выберите **EF Designer from Database** и нажмите **Next**.
   - Укажите подключение к базе данных:
     - Нажмите **New Connection**.
     - Укажите имя сервера, метод аутентификации и выберите вашу базу данных (`SportShopDb`).
     - Проверьте подключение и нажмите **OK**.
   - Убедитесь, что строка подключения сохранена в файле `App.config`.
   - Выберите таблицы для импорта и завершите создание модели.

3. **Использование базы данных через ADO.NET**:
   - После создания модели ADO.NET вы сможете использовать автоматически сгенерированные классы для взаимодействия с таблицами базы данных.
   - Пример работы с моделью:

     ```csharp
     using (var context = new SportShopEntities())
     {
         var products = context.Products.ToList();
         foreach (var product in products)
         {
             Console.WriteLine($"{product.Name} - {product.Price} руб.");
         }
     }
     ```

4. **Настройка строки подключения**:
   - Проверьте строку подключения в файле `App.config` и убедитесь, что она корректно указывает на ваш SQL Server:

     ```xml
     <connectionStrings>
         <add name="SportShopEntities"
              connectionString="metadata=res://*/SportShopModel.csdl|res://*/SportShopModel.ssdl|res://*/SportShopModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=YourServerName;initial catalog=SportShopDb;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;"
              providerName="System.Data.EntityClient" />
     </connectionStrings>
     ```

После выполнения этих шагов ваш проект будет готов к взаимодействию с базой данных через ADO.NET.

## Авторы

* **Filimonova Anastasia** - *Initial work* -(https://github.com/AnastasiaFilimonova)


