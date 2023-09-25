using DB_Setup;
using Npgsql;

class SetupingDB
{
    static void Main()
    {
        SetupScript setupScript = new SetupScript();
        setupScript.SetupingScript();
        Tables tables = new Tables();
        tables.Fill_Tables();
        tables.Print_Tables();
    }
}
