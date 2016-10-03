Welcome to the ASP.Net Core workshop
===================

You can participate in this workshop from any computer running Windows, Mac OS X or Linux.

During this workshop the following dependencies need to be installed:

- Visual Studio Code (http://code.visualstudio.com/)
- .NET Core 1.0.1 (http://dot.net)

During this workshop we'll be creating a RSVP application for an upcoming wedding.

**0) Download & Restore**

Download or clone this repository and make sure the directory is called "MyWedding". After this execute the following command within the MyWedding directory using the command-line or terminal:

    dotnet restore

After this all dependencies should restore and you're ready to do some programming.

**1) Let's create a database**

To start of we'll have to create a database. Within ASP.Net core there is a library called Entity Framework. This allows us to generate the database tables & columns based on a POCO (Plain Old C# Object). Entify Framwork also allows us to query the database using Linq instead of SQL. 

For example:

    _dbContext.MyTable.Where(x => x.Id == 10).First();

Instead of

    SELECT * FROM MyTable WHERE Id is 10;

We'll start by creating our datamodel. Open the Guest.cs file in the Models directory.

Within this Model (table) we'll define our properties (columns)

        public int Id {get;set;}
        public string Code {get;set;}
        public string Name {get;set;}
        public bool IsAttending {get;set;}
        public bool HasResponded {get;set;}
        public EMealType MealType {get;set;}
        public string Comments {get;set;}
      
To make sure Entity Framework knows about our data model we'll need to define our Guest class within the ApplicationDatabase context.

To do this, go to ApplicationDbContext.cs within the Data directory and add the following:

    public DbSet<Guest> Guests {get; set;}

Good, now Entity Framework is aware of our Guests table. Now let's execute some commands to generate this database.

Using the command-line inside inside the project directory execute the following commands:

    dotnet ef database migrations add GuestMigration
    dotnet ef database update

The first command generates a migration script to create/update the database. The second command executes all pending migrations.

**2) Let's create our Admin Area**

For our application to perform we'll need to be able to add guests to our database and view their responses.

Go to the AdminController.cs file within the Controllers directory.

Let's start by creating the following 2 functions:

    
        public IActionResult Index()
        {
            return View(_dbContext.Guests.ToList());
        }
         
        [HttpPost]
        public IActionResult AddGuest([FromForm] string code, string name)
        {
             var guest = new Guest();
            guest.Code = code;
            guest.Name = name;
            _dbContext.Guests.Add(guest);
            _dbContext.SaveChanges();

            return View("Index", _dbContext.Guests.ToList());
        }

The first method returns an Index View on the path "http://mywedding.com/Admin/" the second method is a method we'll be calling from a form we'll be creating next.

Open the following view "Index.cshtml" within the Views/Admin folder and add the following code:

    <form class="ui form" asp-action="AddGuest">
	  <div class="field">
	    <label>Name</label>
	    <input type="text" name="name" placeholder="Name">
	  </div>
	  <div class="field">
	    <label>Code</label>
	    <input type="text" name="code" placeholder="Code">
	  </div>
	
	  <button class="ui button" type="submit">Save</button>
	</form>

Great, let's see if all our hard work paid of.

**3) Testing the admin area**

Using the command-line inside the project directory execute the following command:

    dotnet run

After this, the application will start. If everything was successful go to http://localhost:5000/Admin/ to open the admin area. Try to add some guests.

**4) Lets allow people RSVP**

Off course people need to be able to RSVP as well, so let's create our logic. Open the GuestController.cs within the Controllers directory and add the following methods:

        [HttpPost]
        public IActionResult Index([FromForm] string code)
        {
            var guest = _dbContext.Guests.FirstOrDefault(x => x.Code == code);

            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        [HttpPost]
        public IActionResult SaveResponse([FromForm] int id, bool isAttending, EMealType mealType, string comments)
        {
            var guest = _dbContext.Guests.FirstOrDefault(x => x.Id == id);
            
            guest.IsAttending = isAttending;
            guest.MealType = mealType;
            guest.Comments = comments;
            guest.HasResponded = true;

             _dbContext.Guests.Update(guest);
            _dbContext.SaveChanges();

            return View();
        }

This logic allows us to verify someone's welcome code and save their preferences. Now let's add the corrosponding view.

Open the Index.cshtml within the Views/Guest directory and add the following code:

     <form class="ui form" asp-action="SaveResponse">
            <input type="hidden" name="id" value="@Model.Id">
            <div class="field">
                <label>Welcome @Model.Name</label>
            </div>
            <div class="field">
                <label>Will you be attending?</label>
                <select name="isAttending" class="ui dropdown">
                    <option value=true>Yes</option>
                    <option value=false>No</option>
                </select>
            </div>

             <div class="grouped fields">
                 <label>Meal preference</label>
                 <select name="mealType" class="ui dropdown">
                     <option value=0>Fish</option>
                     <option value=1>Meat</option>
                     <option value=2>Vegetarian</option>
                </select>
               
            </div>
            <div class="field">
                <label>Comments (food allergies, special arrangements, etc)</label>
                <textarea name="comments" rows="3"></textarea>
            </div>
            <button class="ui button" type="submit">Submit</button>
     </form>

**5) Lets test**

Using the command-line inside the project directory execute the following command:

    dotnet run

After this, the application will start. If everything was successful go to http://localhost:5000/ to open the RSVP website. Try to enter a welcome code to see if you can enter your preferences.

**Congratulations, you have a working ASP.NET core application.**
