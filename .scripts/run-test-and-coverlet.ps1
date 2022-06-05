# cf. Microsoft Documents
# https://docs.microsoft.com/ja-jp/dotnet/core/testing/unit-testing-code-coverage?tabs=windows
# cf. 上記の dotnet tool ~ コマンドは間違っている。以下の URL のコマンドを実行すること。
# https://www.nuget.org/packages/dotnet-reportgenerator-globaltool
# cf. OpenCover については、以下のページを参照すると欲しい情報はまとまっている。
# https://github.com/coverlet-coverage/coverlet
# https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/VSTestIntegration.md

$runSettings = ".\Tests\.coverlet\runsettings.xml"
$resultDirectory = ".\.TestResults"
$reportsDirectory = ".\.TestReports"

# 既存のパスは前回の情報を残したくないので削除する。
# カバレッジはその時にしか使わないからこれでいい。
if (Test-Path $resultDirectory) { Remove-Item $resultDirectory -Recurse }
if (Test-Path $reportsDirectory) { Remove-Item $reportsDirectory -Recurse }

# Azure.Cost.Notification.Tests のカバレッジを収集する。
dotnet test .\Tests\Azure.Cost.Notification.Tests\Azure.Cost.Notification.Tests.csproj --collect:"XPlat Code Coverage" --results-directory $resultDirectory --settings $runSettings

# 先頭のファイル名を取得したかった。他にいい方法が思いつかなかったので妥協する。
$xmlFileName = (Get-ChildItem $resultDirectory -Filter *.xml -Recurse -File)[0].FullName

# 出力先も分けてないとどっちのレポートなのかわからなくなるので。
reportgenerator -reports:$xmlFileName -targetdir:($reportsDirectory + "\Azure.Cost.Notification.Tests") -reporttypes:Html

# xml ファイルの出力先は、ディレクトリ名がハッシュ値になっている。
# このパスの取得方法が少し調べてもわからなかった。（ちゃんと調べたら出てくるかも）
# 現状は毎回以下のディレクトリを削除すれば、出力されるファイルは１つだけとなる。
# 結構力技なので、もう少しいいやり方があったらそちらを採用したい。
if (Test-Path $resultDirectory) { Remove-Item $resultDirectory -Recurse }

# Azure.RestApi.Tests のカバレッジを収集する。
dotnet test .\Tests\Azure.RestApi.Tests\Azure.RestApi.Tests.csproj --collect:"XPlat Code Coverage" --results-directory $resultDirectory --settings $runSettings

$xmlFileName = (Get-ChildItem $resultDirectory -Filter *.xml -Recurse -File)[0].FullName

reportgenerator -reports:$xmlFileName -targetdir:($reportsDirectory + "\Azure.RestApi.Tests") -reporttypes:Html

if (Test-Path $resultDirectory) { Remove-Item $resultDirectory -Recurse }