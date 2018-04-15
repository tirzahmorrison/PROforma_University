using System;

public class Class1
{
    protected DbConnection dbConn;
    public BaseAction(DbConnection dbConnection)
	{
        dbConn = dbConnection;
    }
}
