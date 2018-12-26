Welcome to the ASP.Net Core workshop
===================

You can participate in this workshop from any computer running Windows, Mac OS X or Linux.

During this workshop the following dependencies need to be installed:

- Visual Studio Code (http://code.visualstudio.com/)
- Or Visual Studio 2017 (https://www.visualstudio.com/)
- .NET Core 2.X (http://dot.net)

During this workshop we'll be creating a RSVP application for an upcoming wedding.

**0) Download & Restore**

Download or clone this repository and make sure the directory is called "MyWedding". Both Visual Studio Code and Visual Studio should automatically restore packages.
If they don't, please execute the following command within the MyWedding directory using the command-line or terminal:

    dotnet restore

After this all dependencies should restore and you're ready to do some programming.

**1) Let's create a database**

To start off we'll have to create a database. Within ASP.Net core there is a library called Entity Framework. This allows us to generate the database tables & columns based on a POCO (Plain Old C# Object). Entity Framework also allows us to query the database using Linq instead of SQL. 

For example:

    _dbContext.MyTable.Where(x => x.Id == 10).First();

Instead of

    SELECT * FROM MyTable WHERE Id is 10;

We'll start by creating our datamodel. Open the Guest.cs file in the Models directory.

Within this Model (table) we'll define our properties (columns)

    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public bool IsAttending { get; set; }
    public bool HasResponded { get; set; }
    public EMealType MealType { get; set; }
    public string Comments { get; set; }
      
To make sure Entity Framework knows about our data model we'll need to define our Guest class within the ApplicationDatabase context.

To do this, go to ApplicationDbContext.cs within the Data directory and add the following:

    public DbSet<Guest> Guests {get; set;}

Good, now Entity Framework is aware of our Guests table. Now let's execute some commands to generate this database.

Within the command-line inside the project directory execute the following commands:

    dotnet ef migrations add GuestMigration
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
	
	  <button class="ui primary button" type="submit">Save</button>
	</form>

Great, let's see if all our hard work has paid off.

**3) Testing the admin area**

You can now run the application from Visual Studio Code or Visual Studio. If you want to run it from console, execute the following command inside the project directory:

    dotnet run

After this, the application will start. If everything was successful go to http://localhost:5000/Admin/ to open the admin area. Try to add some guests.

**4) Let's allow people RSVP**

Of course people need to be able to RSVP as well, so let's create our logic. Open the GuestController.cs within the Controllers directory and add the following methods:

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

This logic allows us to verify someone's welcome code and save their preferences. Now let's add the corresponding view.

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
                     <option value=0>Meat</option>
                     <option value=1>Fish</option>
                     <option value=2>Vegetarian</option>
                </select>
               
            </div>
            <div class="field">
                <label>Comments (food allergies, special arrangements, etc)</label>
                <textarea name="comments" rows="3"></textarea>
            </div>
            <button class="ui primary button" type="submit">Submit</button>
     </form>

**5) Let's test**

Run the project again from VS Code, VS or from the command-line inside the project directory with the following command:

    dotnet run

After this, the application will start. If everything was successful go to http://localhost:5000/ to open the RSVP website. Try to enter a welcome code to see if you can enter your preferences.

**6) Let's delete some guests**

Everyone makes mistakes, deleting is of course essential within the admin view.

Let's add the following logic to the AdminController.cs within the Controllers directory.

		[HttpPost]
        public IActionResult DeleteGuest([FromForm] int id)
        {
            var guest = _dbContext.Guests.FirstOrDefault(x => x.Id == id);
            _dbContext.Guests.Remove(guest);
            _dbContext.SaveChanges();
            return View("Index",_dbContext.Guests.ToList());
        }

And the following logic to the Index.cshtml in the Views/Admin directory within a new td tag.

    <form class="ui form" asp-action="DeleteGuest">
         <input type="hidden" value="@guest.Id" name="id" />
         <button class="ui red button" type="submit">Delete</button>
    </form>

Don't forget to add a corresponding column header in the correct place withing the table header

    <th>Modify</th>

Now we have a delete button. Let's run the application and see if this works.

**7) Let's change some values**

But what if one of your guests calls you and he prefers Meat instead of Fish. Let's add some edit functionality.

Go to the Index.cshtml in the Views/Admin directory and add the following line of code inside the delete form:

      <a asp-action="Edit" asp-route-id="@guest.Id" class="ui primary button">Edit</a>

Ok now let's create the Edit view. Let's create a new file called Edit.cshtml within the Views/Admin directory.

Within this file add the following code:

    @using MyWedding.Models.Enums
	@model Guest
	
	<div class="ui segment">
	
	    <div class="ui text centered container">
	         <form class="ui form" asp-action="Edit">
	        <input type="hidden" name="id" value="@Model.Id">
	        <div class="field">
	            <label>Name</label>
	            <input class="ui input" name="name" type="text" value="@Model.Name" />
	        </div>
	        <div class="field">
	            <label>Code</label>
	            <input class="ui input" name="code" type="text" value="@Model.Code" />
	        </div>
	        <div class="field">
	            <label>Will you be attending?</label>
	            <select name="isAttending" class="ui dropdown">
	                @if (Model.HasResponded)
	                {
	                    @if (Model.IsAttending)
	                    {
	                        <option value=true selected>Yes</option>
	                        <option value=false>No</option>
	                    } 
	                    else 
	                    {
	                        <option value=true>Yes</option>
	                        <option value=false selected>No</option>
	                    }
	                }
	                else 
	                {
	                    <option value="null">Unknown</option>
	                    <option value=true>Yes</option>
	                    <option value=false>No</option>
	                }
	             
	            </select>
	        </div>
	
	         <div class="grouped fields">
	             <label>Meal preference</label>
	             <select name="mealType" class="ui dropdown">
	                 @if (Model.MealType == EMealType.Meat)
	                 {
	                    <option value=0 selected>Meat</option>
	                 }
	                 else
	                 {
	                    <option value=0>Meat</option>
	                 }
	
	                 @if (Model.MealType == EMealType.Fish)
	                 {
	                    <option value=1 selected>Fish</option>
	                 }
	                 else
	                 {
	                    <option value=1>Fish</option>
	                 }
	
	                 @if (Model.MealType == EMealType.Vegetarian)
	                 {
	                    <option value=2 selected>Vegetarian</option>
	                 }
	                 else
	                 {
	                    <option value=2>Vegetarian</option>
	                 }
	                 
	                </select>
	
	        </div>
	        <div class="field">
	            <label>Comments (food allergies, special arrangements, etc)</label>
	            <textarea name="comments" rows="3">@Model.Comments</textarea>
	        </div>
	        <button class="ui primary button" type="submit">Save</button>
	    </form>
	
	        
	    </div>
	
	</div>

Great, the views are done. Now let's add some logic in our AdminController. Go ahead and open the AdminController.cs in the Controllers directory and add the following code.

	    public IActionResult Edit(int id)
        {
            var guest = _dbContext.Guests.FirstOrDefault(x => x.Id == id);
            return View(guest);
        }

        [HttpPost]
        public IActionResult Edit([FromForm] int id, string name, string code, bool? isAttending, EMealType mealType, string comments)
        {
              var guest = _dbContext.Guests.FirstOrDefault(x => x.Id == id);
              guest.Name = name;
              guest.Code = code;
              if (isAttending != null)
              {
                    guest.IsAttending = (bool)isAttending;
              }
              guest.MealType = mealType;
              guest.Comments = comments;
              guest.HasResponded = true;

              _dbContext.Guests.Update(guest);
              _dbContext.SaveChanges();

             return View("Index",_dbContext.Guests.ToList());
        }

Great. Now let's test this the application by executing dotnet run and see if the edit functionality is working as intended.

**8) Authentication**

Now we have all this great functionality, but we're not there yet. Everyone can now navigate to the /admin page and manage all your guests. This is of course not secure.

Go ahead and add the following line of code on top of the AdminController class within the AdminController.cs file.

    [Authorize]

Now run the application and navigate to http://localhost:5000/Admin and you'll see you now require a username & password to login.

Navigate to http://localhost:5000/Acccount/Register to create an account and see if you're able to login afterwards.

To disable registration remove the relevant methods in the AccountController.cs file within the Controllers directory.

**Congratulations, you now have a fully functional ASP.NET Core web application with a working database & working authentication**
