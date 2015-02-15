REM this script reserves urls for embedded magnum webserver
REM usage:
REM		namespace_reservation add 33026
REM		namespace_reservation delete 33026
REM http://stackoverflow.com/questions/4019466/httplistener-access-denied/4115328#4115328
REM port number should be synced with app.config!
REM run as admin, but from current user!

netsh http %1 urlacl url=http://+:%2/ user="%USERNAME%"

rem details can be found here - https://msdn.microsoft.com/en-us/library/windows/desktop/cc307236(v=vs.85).aspx