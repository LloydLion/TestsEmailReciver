using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestsEmailReciver.Parsers
{
	class OnlineTestPadParser : TestRecordParser
	{
		public OnlineTestPadParser() : base("no-reply@onlinetestpad.info") { }


		public override TestRecord Parse(string premessage, out bool canParse)
		{
			var staticPart = "<html><body>    <div class=\"main\" style=\"background:#fff; font-size: 14px; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0;color: #333;\">" +
				"        <div class=\"content\" style=\"width:100%;max-width:600px;margin:10px auto;\">            <div class=\"header\" style=\"background-color: #3e8ef7;\">" +
				"                <table style=\"background-color: #3e8ef7; width: 100%;\">                    <tr>" +
				"                        <td style=\"width:65px;\">                            " +
				"<a href=\"http://onlinetestpad.com\" style=\"text-decoration:none; display:block;margin: 5px;\">" +
				"                                <img src=\"http://onlinetestpad.com/images/site/logo/logo55.png\" style=\"border:none;\" alt=\"OTP\" />" +
				"                            </a>                        </td>                        <td>" +
				"                            <a href=\"http://onlinetestpad.com\" style=\"text-decoration:none; display:block;margin: 5px;\">" +
				"                                <span style=\"font-size:35px; color:#fff; margin-top:5px;text-decoration:none;display: block;\">Online Test Pad </span>" +
				"                            </a>                        </td>                    </tr>                </table>            </div>" +
				"            <div class=\"body\" style=\"border: solid 1px #ccc; border-top: none; line-height: 20px;\">                <div></div>" +
				"                <div style=\"background-color: #f7f7f7\">                    <table style=\"width:100%\" cellpadding=\"0\" cellspacing=\"0\">" +
				"                        <tr>                            <td>                                <h2 style=\"color: #4765a0; margin: 10px; font-size: 20px; font-weight: normal; margin: 10px;\">" +
				"                                    &#x420;&#x435;&#x437;&#x443;&#x43B;&#x44C;&#x442;&#x430;&#x442; &#x442;&#x435;&#x441;&#x442;&#x430; ";

			var message = premessage[staticPart.Length..];

			var testName = Regex.Match(message, @"\A.*                                </h2>").Value[..^"                                </h2>".Length];

			message = message[testName.Length..];

			var passDate = DateTime.Parse(Regex.Match(message, @"Дата завершения: \d{2}-\d{2}-\d{4} \d{2}:\d{2}").Value["Дата завершения: ".Length..]);
			var passingTime = TimeSpan.Parse(Regex.Match(message, @"Потрачено времени: \d{2}:\d{2}:\d{2}").Value["Потрачено времени: ".Length..]);
			var className = Regex.Match(message, @"Класс: \d").Value["Класс: ".Length..] + Regex.Match(message, @"Параллель: \w").Value["Параллель: ".Length..];
			var studentName = Regex.Match(message, @"Фамилия Имя: \w*\b\s\b\w*").Value["Фамилия Имя: ".Length..];
			var scores = int.Parse(Regex.Match(message, @"Количество правильных ответов: \w*").Value["Количество правильных ответов: ".Length..]);
			var persentage = int.Parse(Regex.Match(message, @"Процент правильных ответов \(%\): \w*").Value["Процент правильных ответов (%): ".Length..]);
			var mark = int.Parse(Regex.Match(message, @"Ваша оценка:: \w").Value["Ваша оценка:: ".Length..]);
			var url = Regex.Match(message, "<a href=\"https://onlinetestpad.com/\\w*\">Ссылка на результат</a>").Value["<a href=\"".Length..^"\">Ссылка на результат</a>".Length];

			canParse = true;
			return new TestRecord(studentName, className, testName, mark, persentage, scores) { OriginMessage = premessage, PassDate = passDate, PassingTime = passingTime, Url = url };
		}
	}
}
