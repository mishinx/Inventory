using DB_Setup;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Reflection.Metadata;
using Inventory_Context;

class SetupingDB
{
    static void Main()
    {
        //SetupScript setupScript = new SetupScript();
        //setupScript.SetupingScript();
        //Tables tables = new Tables();
        //tables.Fill_Tables();
        //tables.Print_Tables();
        var optionsBuilder = new DbContextOptionsBuilder<InventoryContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=inventarium;Username=postgres;Password=bochka2004;");

        
            using var db = new InventoryContext();

        //    Console.WriteLine($"Database path: {db.DbPath}.");

        //    Console.WriteLine("Inserting a new blog");
        //    //db.Add(new Administrator { company_name = "Asasina 23", email_address = "asdasdasd@gmail.com", admin_password = "asdadsad", full_name = "Asa Din", phone_number = "+380987667845"});
        //    //db.SaveChanges();

        //    Console.WriteLine("Querying for a blog");
        //    var admin = db.administrators
        //        .OrderBy(b => b.admin_id)
        //        .First();
        //    var warehouse = new Warehouse { addres = "sdf", admins_id = admin.admin_id };
        //    //db.Add(warehouse);// Update
        //    //db.SaveChanges();
        //    //db.Add(
        //    Goods g= new Goods { category="asd", short_description="asdasd", full_name="dasdasd", price=23, quantity=3, subcategory="sdf", warehouses_id=warehouse.warehouse_id};// Update
        //    //db.SaveChanges();
        //    Console.WriteLine("Updating the name of company");
        //    admin.company_name = "New Company Name";
        //    db.SaveChanges();

        //    // Delete
        //    Console.WriteLine("Delete the blog");
        //    db.Remove(db.goods.OrderBy(b => b.goods_id).First());
        //    db.SaveChanges();
        }
    }
