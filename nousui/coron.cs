string input = "2:1,2,3";

// 「:」より後ろを取得
string valuesPart = input.Split(':')[1];

// カンマで分割してリスト化
var valueList = valuesPart
    .Split(',')
    .Select(v => $"'{v}'") // '1' のように囲む
    .ToList();

// 出力確認
foreach (var v in valueList)
{
    Console.WriteLine(v);
}