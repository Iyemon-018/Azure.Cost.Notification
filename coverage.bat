# Execute coverage output and report.
powershell -File .\.scripts\run-test-and-coverlet.ps1

# View report page.
start .TestReports\Azure.Cost.Notification.Tests\index.html