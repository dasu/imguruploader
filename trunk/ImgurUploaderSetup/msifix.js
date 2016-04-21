var msiOpenDatabaseModeTransact = 1;
if (WScript.Arguments.Length != 1) {
    WScript.StdErr.WriteLine(WScript.ScriptName + " file");
    WScript.Quit(1);
}
WScript.Echo(WScript.Arguments(0));
var filespec = WScript.Arguments(0);
var installer = WScript.CreateObject("WindowsInstaller.Installer");
var database = installer.OpenDatabase(filespec, msiOpenDatabaseModeTransact);
var sql;
var view;
//try {
    sql = "INSERT INTO Property (Property,Value) VALUES ('DISABLEADVTSHORTCUTS', '1')";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();

    database.Commit();
//}
/*catch (e) {
    WScript.StdErr.WriteLine('error: ' + e);
    WScript.Quit(1);
}*/